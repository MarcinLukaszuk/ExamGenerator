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

namespace ExamGenerator.DocumentManager
{
    public static class BitmapAnalyser
    {
        static readonly float standardDocumentHeight = 842;


        public static bool CheckValue(Bitmap bitmap)
        {
            List<RotatedRect> boxList = new List<RotatedRect>();

            using (var imageBinarized = getBinarizedImage(bitmap))
            using (var cannyEdges = getEdgesOfImage(imageBinarized))
            using (var contours = getContoursOfImage(cannyEdges))
            {
                for (int i = 0; i < contours.Size; i++)
                {
                    using (var contour = contours[i])
                    using (var approxContour = getLinkedContoursOfImage(contour))
                    {
                        if (CvInvoke.ContourArea(approxContour, false) > 200)
                        {
                            var rectangle = getCheckboxPatternFromContoursOfImage(approxContour);
                            if (rectangle != null)
                            {
                                boxList.Add((RotatedRect)rectangle);
                            }
                        }
                    }
                }

                if (boxList.Any())
                {
                    using (var btm = getCheckboxFromBinarizedImage(imageBinarized, boxList, bitmap.PixelFormat))
                    {
                        if (btm != null)
                        {
                            var whitePixels = countBlackPixels(btm);
                            var blackPixels = countAllPixels(btm) - whitePixels;
                            if (blackPixels >= whitePixels)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static Bitmap GetAnswerBitmap(Bitmap bitmap, AnswerPositionDTO answerPosDTO)
        {
            Bitmap retval = null;
            using (var btm = new Bitmap(bitmap))
            {
                float standardMultiplicantValue = btm.Height / standardDocumentHeight;
                int absoluteX = (int)(answerPosDTO.X * standardMultiplicantValue);
                int absoluteY = btm.Height - (int)(answerPosDTO.Y * standardMultiplicantValue);
                int absoluteWidth = (int)(answerPosDTO.Width * standardMultiplicantValue);
                int absoluteHeight = (int)(20 * standardMultiplicantValue);
                Rectangle cloneRect = new Rectangle(absoluteX, absoluteY, absoluteWidth, absoluteHeight);
                retval = btm.Clone(cloneRect, btm.PixelFormat);
            }
            return retval;
        }
        public static int GetExamPage(Bitmap bitmap)
        {
            string leftQRCode;
            string rightQRCode;
            if (bitmap.Width > bitmap.Height)
                bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            using (var leftBtm = getLeftQRCode(bitmap))
            using (var rightBtm = getRightQRCode(bitmap))
            {
                leftQRCode = QrCodeEncoderDecoder.Decode(leftBtm);
                rightQRCode = QrCodeEncoderDecoder.Decode(rightBtm);
            }

            if (leftQRCode == null && rightQRCode == null)
            {
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);

                using (var leftBtm = getLeftQRCode(bitmap))
                using (var rightBtm = getRightQRCode(bitmap))
                {
                    leftQRCode = QrCodeEncoderDecoder.Decode(leftBtm);
                    rightQRCode = QrCodeEncoderDecoder.Decode(rightBtm);
                }
            }
            if (leftQRCode != null && leftQRCode == rightQRCode && int.TryParse(rightQRCode.Split('/')?[1], out int result))
                return result;
            return 0;
        }
        public static int GetExamID(Bitmap bitmap)
        {
            string leftQRCode;
            string rightQRCode;
            if (bitmap.Width > bitmap.Height)
                bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            using (var leftBtm = getLeftQRCode(bitmap))
            using (var rightBtm = getRightQRCode(bitmap))
            {
                leftQRCode = QrCodeEncoderDecoder.Decode(leftBtm);
                rightQRCode = QrCodeEncoderDecoder.Decode(rightBtm);
            }

            if (leftQRCode == null && rightQRCode == null)
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);

            using (var leftBtm = getLeftQRCode(bitmap))
            using (var rightBtm = getRightQRCode(bitmap))
            {
                leftQRCode = QrCodeEncoderDecoder.Decode(leftBtm);
                rightQRCode = QrCodeEncoderDecoder.Decode(rightBtm);
            }

            if (leftQRCode != null && leftQRCode == rightQRCode && int.TryParse(rightQRCode.Split('/')?[0], out int result))
                return result;
            return 0;
        }

        private static Image<Gray, byte> getBinarizedImage(Bitmap bitmap)
        {
            var imageGray = new Image<Bgr, byte>(bitmap).Convert<Gray, byte>();
            var imageBynarize = new Image<Gray, byte>(imageGray.Width, imageGray.Height, new Gray(0));
            CvInvoke.Threshold(imageGray, imageBynarize, 0, 255, ThresholdType.Otsu);

            return imageBynarize;
        }

        private static UMat getEdgesOfImage(Image<Gray, byte> image)
        {
            UMat cannyEdges = new UMat();
            double cannyThreshold = 180.0;
            double cannyThresholdLinking = 120.0;
            CvInvoke.Canny(image, cannyEdges, cannyThreshold, cannyThresholdLinking);

            return cannyEdges;
        }

        private static VectorOfVectorOfPoint getContoursOfImage(UMat cannyEdges)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            return contours;
        }

        private static VectorOfPoint getLinkedContoursOfImage(VectorOfPoint contour)
        {
            VectorOfPoint approxContour = new VectorOfPoint();
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);

            return approxContour;
        }

        private static RotatedRect? getCheckboxPatternFromContoursOfImage(VectorOfPoint approxContour)
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
                    return CvInvoke.MinAreaRect(approxContour);
            }
            return null;
        }

        private static Bitmap getCheckboxFromBinarizedImage(Image<Gray, byte> imageBinarized, List<RotatedRect> boxList, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            var centerX = boxList.Select(x => x.Center.X).Average();
            var centerY = boxList.Select(x => x.Center.Y).Average();
            int edgeLenght = (int)boxList.Select(x => (x.Size.Height + x.Size.Width) / 2).Average();
            int absoluteX = (int)(centerX - (edgeLenght / 2));
            int absoluteY = (int)(centerY - (edgeLenght / 2));
            Rectangle cloneRect = new Rectangle(absoluteX, absoluteY, edgeLenght, edgeLenght);

            return imageBinarized.ToBitmap().Clone(cloneRect, pixelFormat);
        }

        private static int countBlackPixels(Bitmap bitmap)
        {
            int pixelNumber = (int)new Image<Bgr, byte>(bitmap).CountNonzero().Average();
            return pixelNumber;
        }

        private static int countAllPixels(Bitmap bitmap)
        {
            int pixelNumber = bitmap.Width * bitmap.Height;
            return pixelNumber;
        }

        private static Bitmap getLeftQRCode(Bitmap btm)
        {
            Bitmap retval = null;
            using (Bitmap bitmap = new Bitmap(btm))
            {
                var width = btm.Width;
                var qrCodeSize = width / 7;
                var cloneRect = new Rectangle(0, 0, qrCodeSize, qrCodeSize);
                retval = bitmap.Clone(cloneRect, bitmap.PixelFormat);
            }
            return getBinarizedImage(retval).ToBitmap();
        }

        private static Bitmap getRightQRCode(Bitmap btm)
        {
            Bitmap retval = null;
            using (Bitmap bitmap = new Bitmap(btm))
            {
                var width = btm.Width;
                var qrCodeSize = width / 7;
                var cloneRect = new Rectangle(width - qrCodeSize, 0, qrCodeSize, qrCodeSize);
                retval = bitmap.Clone(cloneRect, bitmap.PixelFormat);
            }
            return getBinarizedImage(retval).ToBitmap();
        }
    }
}
