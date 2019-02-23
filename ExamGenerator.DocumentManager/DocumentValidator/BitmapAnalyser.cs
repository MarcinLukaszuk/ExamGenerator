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
        static readonly float standardDocumentWidth = 595;
        static readonly double standardTopMarginPercentage = 0.08;// in %
        static readonly double standardBottomMarginPercentage = 0.1;// in %
        static readonly double standardLeftRightMarginPercentage = 0.08;// in %
        static readonly double distanceBeetweenTopQRCodes = 0.84;// in %
        static readonly double distanceBeetweenLeftQRCodes = 0.86;// in %


        public static bool CheckValue(Bitmap bitmap)
        {
            List<RotatedRect> boxList = new List<RotatedRect>();

            using (var image = new Image<Gray, byte>(bitmap).Resize(2, Inter.Linear))
            {
                CvInvoke.Threshold(image, image, 200, 255, ThresholdType.Otsu);
                using (var cannyEdges = getEdgesOfImage(image))
                using (var contours = getContoursOfImage(cannyEdges))
                {
                    image.Save("C:\\Users\\Marcin\\source\\repos\\trapez\\trapez\\bin\\Debug\\output\\out.jpg");

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
                        using (var btm = getCheckboxFromBinarizedImage(image, boxList, bitmap.PixelFormat))
                        {
                            btm.Save("C:\\Users\\Marcin\\source\\repos\\trapez\\trapez\\bin\\Debug\\output\\out2.jpg");

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
            }
            return false;
        }

        public static Bitmap ResizeToStandard(Bitmap bitmap)
        {
            var qrCodesPositions2 = QrCodeEncoderDecoder.DecodeMultiple(bitmap).ToList();
            if (bitmap.Width > bitmap.Height)
            {
                bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            var qrCodesPositions = QrCodeEncoderDecoder.DecodeMultiple(bitmap).ToList().OrderBy(x => x.ResultPoints.FirstOrDefault().Y).Select(x => x.ResultPoints.FirstOrDefault()).ToList();

            if (qrCodesPositions.Count() == 3 && (qrCodesPositions[2].Y - (qrCodesPositions[1].Y + qrCodesPositions[0].Y)) < 0)
            {
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }

            var qrCodesNewPositions = QrCodeEncoderDecoder.DecodeMultiple(bitmap).ToList().OrderBy(x => x.ResultPoints.FirstOrDefault().X).Select(x => x.ResultPoints).ToList();
            // leftTopQRCode
            var leftTopQRCode = qrCodesNewPositions.Take(2).OrderBy(x => x.FirstOrDefault().Y).FirstOrDefault().OrderBy(x => x.X + x.Y).FirstOrDefault();
            var leftBottomQRCode = qrCodesNewPositions.Take(2).OrderByDescending(x => x.FirstOrDefault().Y).FirstOrDefault().OrderBy(x => x.X).FirstOrDefault();
            var rightTopQRCode = qrCodesNewPositions[2].OrderByDescending(x => x.X).FirstOrDefault();

            //     var topQRCodeDistance= leftTopQRCode

            var source = new PointF[4];

            source[0] = new PointF(leftTopQRCode.X, leftTopQRCode.Y);
            source[1] = new PointF(leftBottomQRCode.X, leftBottomQRCode.Y);
            source[2] = new PointF(rightTopQRCode.X, leftBottomQRCode.Y);
            source[3] = new PointF(rightTopQRCode.X, rightTopQRCode.Y);

            var topQRDistance = rightTopQRCode.X - leftBottomQRCode.X;
            var leftQRDistance = leftBottomQRCode.Y - leftTopQRCode.Y;

            int offsideX = (int)((topQRDistance / distanceBeetweenTopQRCodes) * standardLeftRightMarginPercentage);
            int offsideTop = (int)((leftQRDistance / distanceBeetweenLeftQRCodes) * standardTopMarginPercentage);
            int offsideBot = (int)((leftQRDistance / distanceBeetweenLeftQRCodes) * standardBottomMarginPercentage);

            source[0] = addOffsideToPoint(source[0], -offsideX, -offsideTop, bitmap);
            source[1] = addOffsideToPoint(source[1], -offsideX, offsideBot, bitmap);
            source[2] = addOffsideToPoint(source[2], offsideX, offsideBot, bitmap);
            source[3] = addOffsideToPoint(source[3], offsideX, -offsideTop, bitmap);


            var target = new PointF[] {
                new PointF(0,0),
                new PointF(0,standardDocumentHeight),
                new PointF(standardDocumentWidth,standardDocumentHeight),
                new PointF(standardDocumentWidth,0)
            };



            using (var image = new Image<Gray, byte>(bitmap))
            {
                var tran = CvInvoke.GetPerspectiveTransform(source, target);
                CvInvoke.WarpPerspective(image, image, tran, new Size((int)standardDocumentWidth, (int)standardDocumentHeight));
                return image.ToBitmap((int)standardDocumentWidth, (int)standardDocumentHeight);
            }
        }

        private static PointF addOffsideToPoint(PointF point, int offsideX, int offsideY, Bitmap bitmap)
        {
            var maxWidth = bitmap.Width;
            var maxHeight = bitmap.Height;

            point.X += offsideX;
            point.Y += offsideY;
            if (point.X > maxWidth)
                point.X = maxWidth;
            else if (point.X < 0)
                point.X = 0;

            if (point.Y > maxHeight)
                point.Y = maxHeight;
            else if (point.Y < 0)
                point.Y = 0;
            return point;
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
            var results = QrCodeEncoderDecoder.DecodeMultiple(bitmap);

            if (results != null && results.Length > 0)
            {
                var res = results.FirstOrDefault();
                if (int.TryParse(res.ToString().Split('/')?[1], out int result))
                    return result;
            }
            return 0;
        }
        public static int GetExamID(Bitmap bitmap)
        {
            var results = QrCodeEncoderDecoder.DecodeMultiple(bitmap);

            if (results != null && results.Length > 0)
            {
                var res = results.FirstOrDefault();
                if (int.TryParse(res.ToString().Split('/')?[0], out int result))
                    return result;
            }
            return 0;
        }

        public static Bitmap extractDocumentFromBitmap(Bitmap bitmap)
        {
            if (bitmap.Width>bitmap.Height)
            {
                bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            var image = new Image<Bgr, byte>(bitmap);
            var imageGray = image.Convert<Gray, byte>();
            var filteredImage = new Image<Bgr, byte>(bitmap);

            CvInvoke.BilateralFilter(imageGray, filteredImage, 9, 75, 75);
            CvInvoke.AdaptiveThreshold(filteredImage, filteredImage, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 115, 4);
            CvInvoke.MedianBlur(filteredImage, filteredImage, 11);
            CvInvoke.CopyMakeBorder(filteredImage, filteredImage, 5, 5, 5, 5, BorderType.Constant, new MCvScalar(0, 0, 0));

            UMat cannyEdges = new UMat();
            CvInvoke.Canny(filteredImage, cannyEdges, 200, 250);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            var cannyEdgesHeight = cannyEdges.Bitmap.Height;
            var cannyEdgesWidth = cannyEdges.Bitmap.Width;
            var areaContour = (cannyEdgesHeight - 10) * (cannyEdgesWidth - 10);
            var areaCount = areaContour * 0.5;
            double areaContour2;

            VectorOfPoint sourcePointsVector = new VectorOfPoint();
            for (int i = 0; i < contours.Size; i++)
            {
                var cont = contours[i];

                CvInvoke.ApproxPolyDP(cont, cont, CvInvoke.ArcLength(cont, true) * 0.05, true);
                if (cont.Size == 4 && CvInvoke.IsContourConvex(cont)
                    && areaCount < CvInvoke.ContourArea(cont)
                    && CvInvoke.ContourArea(cont) < areaContour)
                {
                    sourcePointsVector = cont;
                    areaContour2 = CvInvoke.ContourArea(cont);
                    sortVector(sourcePointsVector);
                    break;
                }
            }
            var posortowany = sortVector(sourcePointsVector);
            var zOffsetem = addOffsetToVector(sourcePointsVector, -5);
            
            var wysokoscjakas = new int[] { getEuclideanDistance(zOffsetem[0], zOffsetem[1]), getEuclideanDistance(zOffsetem[2], zOffsetem[3]) }.Max();
            var szerokoscjakas = new int[] { getEuclideanDistance(zOffsetem[0], zOffsetem[2]), getEuclideanDistance(zOffsetem[1], zOffsetem[3]) }.Max();
 
            VectorOfPoint targetPoints2 = new VectorOfPoint(new Point[] {
                new Point(0, 0),
                new Point(0, szerokoscjakas),
                new Point(wysokoscjakas, szerokoscjakas),
                new Point(wysokoscjakas, 0) }.ToArray());


            var source = sortVector(zOffsetem).ToArray().Select(x => new PointF(x.X, x.Y)).ToArray();
            var target = sortVector(targetPoints2).ToArray().Select(x => new PointF(x.X, x.Y)).ToArray();
            var tran = CvInvoke.GetPerspectiveTransform(source, target);
            CvInvoke.WarpPerspective(image, image, tran, new Size(wysokoscjakas, szerokoscjakas));

            return image.ToBitmap((int)standardDocumentWidth * 4, (int)standardDocumentHeight * 4);
        }

        private static int getEuclideanDistance(Point p1, Point p2)
        {
            return (int)Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
        }

        private static VectorOfPoint sortVector(VectorOfPoint vector)
        {
            var output = new VectorOfPoint();
            var left = vector.ToArray().OrderBy(x => x.X).Take(2).ToArray();
            var top = vector.ToArray().OrderBy(x => x.Y).Take(2).ToArray();

            var leftTop = left.Intersect(top).ToArray();
            var leftBottom = left.Except(top).ToArray();
            var rightTop = top.Except(left).ToArray();
            var rightBottom = vector.ToArray().Except(left.Concat(top)).ToArray();

            output.Push(leftTop);
            output.Push(rightBottom);
            output.Push(leftBottom);
            output.Push(rightTop);

            return output;
        }

        static VectorOfPoint addOffsetToVector(VectorOfPoint tmpppp, int offset)
        {
            return new VectorOfPoint(tmpppp.ToArray().Select(x => new Point() { X = x.X + offset, Y = x.Y + offset }).ToArray());
        }

        public static Bitmap getBinarizedBitmap(Bitmap bitmap)
        {
            var image = new Image<Bgr, byte>(bitmap);
            var uimage = new UMat();
            var pyrDown = new UMat();
            CvInvoke.CvtColor(image, uimage, ColorConversion.Bgr2Gray);
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);
            var imageBynarize = image.Convert<Gray, Byte>();
            CvInvoke.AdaptiveThreshold(imageBynarize, imageBynarize, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 255, 32);
            return imageBynarize.ToBitmap(bitmap.Width, bitmap.Height);
        }


        private static Image<Gray, byte> getBinarizedImage(Bitmap bitmap)
        {
            var image = new Image<Bgr, byte>(bitmap);
            var uimage = new UMat();
            var pyrDown = new UMat();
            CvInvoke.CvtColor(image, uimage, ColorConversion.Bgr2Gray);
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);
            var imageBynarize = image.Convert<Gray, Byte>().PyrUp().PyrDown();
            CvInvoke.AdaptiveThreshold(imageBynarize, imageBynarize, 255, AdaptiveThresholdType.MeanC, ThresholdType.Binary, 115, 4);
            CvInvoke.Threshold(image, image, 200, 255, ThresholdType.Otsu);
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
                var qrCodeSize = width / 5;
                var cloneRect = new Rectangle(0, 0, qrCodeSize, qrCodeSize);
                retval = bitmap.Clone(cloneRect, bitmap.PixelFormat);
            }
            return retval;
        }

        private static Bitmap getRightQRCode(Bitmap btm)
        {
            Bitmap retval = null;
            using (Bitmap bitmap = new Bitmap(btm))
            {
                var width = btm.Width;
                var qrCodeSize = width / 5;
                var cloneRect = new Rectangle(width - qrCodeSize, 0, qrCodeSize, qrCodeSize);
                retval = bitmap.Clone(cloneRect, bitmap.PixelFormat);
            }
            return retval;
        }
    }
}
