using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExamGenerator.DocumentManager.QRCodeGenerator;
using ExamGeneratorModel.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.DocumentManager.ZIPValidator
{
    public class BitmapAnalyzer
    {
        public int ExamID { get { return _examID; } }
        public int PageNumber { get { return _pageNumber; } }

        private Bitmap _bitmap;
        private QrCodeED qrcode;
        private float standardDocumentHeight;
        private float standardMultiplicantValue;
        private int _examID;
        private int _pageNumber;

        public BitmapAnalyzer(Bitmap bitmap)
        {
            _bitmap = bitmap;
            standardDocumentHeight = 842;
            standardMultiplicantValue = bitmap.Height / standardDocumentHeight;
            qrcode = new QrCodeED();
            optimalizeBitmap();
        }

        public bool? CheckAnswer(AnswerPositionDTO answerPosDTO)
        {
            Bitmap tmpBym = getAnswerBitmap(answerPosDTO);
            return detectAnswer(tmpBym);
        }

        private void optimalizeBitmap()
        {
            Bitmap _optimalizedBitmap = new Bitmap(_bitmap);
            var width = _optimalizedBitmap.Width;
            var height = _optimalizedBitmap.Height;

            for (int i = 0; i < 4; i++)
            {
                var leftQRCodeString = qrcode.Decode(getLeftQRCode(_optimalizedBitmap));
                var rightQRCodeString = qrcode.Decode(getRightQRCode(_optimalizedBitmap));

                if (leftQRCodeString != null && rightQRCodeString != null && leftQRCodeString == rightQRCodeString)
                {
                    int.TryParse(leftQRCodeString.Split('/')[0], out _examID);
                    int.TryParse(leftQRCodeString.Split('/')[1], out _pageNumber);
                    _bitmap = _optimalizedBitmap;
                    break;
                }
                _optimalizedBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
        }
        private Bitmap getLeftQRCode(Bitmap btm)
        {
            var width = btm.Width;
            var qrCodeSize = width / 7;
            Rectangle cloneRect = new Rectangle(0, 0, qrCodeSize, qrCodeSize);
            System.Drawing.Imaging.PixelFormat format = btm.PixelFormat;
            return btm.Clone(cloneRect, format);
        }
        private Bitmap getRightQRCode(Bitmap btm)
        {
            var width = btm.Width;
            var qrCodeSize = width / 7;
            Rectangle cloneRect = new Rectangle(width - qrCodeSize, 0, qrCodeSize, qrCodeSize);
            System.Drawing.Imaging.PixelFormat format = btm.PixelFormat;
            return btm.Clone(cloneRect, format);
        }

        private Bitmap getAnswerBitmap(AnswerPositionDTO answerPosDTO)
        {
            int absoluteX = (int)(answerPosDTO.X * standardMultiplicantValue);
            int absoluteY = _bitmap.Height - (int)(answerPosDTO.Y * standardMultiplicantValue);
            int absoluteWidth = (int)(answerPosDTO.Width * standardMultiplicantValue);
            int absoluteHeight = (int)(20 * standardMultiplicantValue);

            Rectangle cloneRect = new Rectangle(absoluteX, absoluteY, absoluteWidth, absoluteHeight);
            System.Drawing.Imaging.PixelFormat format = _bitmap.PixelFormat;
            return _bitmap.Clone(cloneRect, format);
        }
        private bool? detectAnswer(Bitmap answerBitmap)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(answerBitmap);
            var imageGray = img.Convert<Gray, byte>();
            var imageBynarize = new Image<Gray, byte>(imageGray.Width, imageGray.Height, new Gray(0));
            CvInvoke.Threshold(imageGray, imageBynarize, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);
            List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle
            UMat cannyEdges = new UMat();
            double cannyThreshold = 180.0;
            double cannyThresholdLinking = 120.0;
            CvInvoke.Canny(imageBynarize, cannyEdges, cannyThreshold, cannyThresholdLinking);
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 250)
                        {
                            if (approxContour.Size == 4)
                            {
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                       edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                if (isRectangle)
                                    boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }
                        }
                    }
                }
            }
            if (!boxList.Any())
            {
                return null;
            }

            var btm = getCheckboxBitmap(boxList, answerBitmap);
            var blackPixels = countBlackPixels(btm);
            var whitePixels = countAllPixels(btm) - blackPixels;

            //inverBinary
            if (blackPixels <= whitePixels)
            {
                return true;
            }
            return false;
        }

        private int countBlackPixels(Bitmap bitmap)
        {
            int pixelNumber = (int)new Image<Bgr, byte>(bitmap).CountNonzero().Average();
            return pixelNumber;
        }

        private int countAllPixels(Bitmap bitmap)
        {
            int pixelNumber = bitmap.Width * bitmap.Height;
            return pixelNumber;
        }

        private Bitmap getCheckboxBitmap(List<RotatedRect> rectangles, Bitmap bitmap)
        {
            var centerX = rectangles.Select(x => x.Center.X).Average();
            var centerY = rectangles.Select(x => x.Center.Y).Average();
            int edgeLenght = (int)rectangles.Select(x => (x.Size.Height + x.Size.Width) / 2).Average();
            int absoluteX = (int)(centerX - (edgeLenght / 2));
            int absoluteY = (int)(centerY - (edgeLenght / 2));
            Rectangle cloneRect = new Rectangle(absoluteX, absoluteY, edgeLenght, edgeLenght);
            System.Drawing.Imaging.PixelFormat format = _bitmap.PixelFormat;
            return bitmap.Clone(cloneRect, format);
        }
    }
}
