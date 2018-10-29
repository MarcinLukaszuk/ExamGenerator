using ExamGenerator.DocumentManager.QRCodeGenerator;
using ExamGeneratorModel.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.DocumentManager.PDFCreator
{
    public static class PDFHelpers
    {
        static int _fontsize = 12;
        static Font _polishFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, _fontsize, Font.NORMAL);
        static Image image = null;
        static int _border = Rectangle.NO_BORDER;

        public static PdfPTable PDFTableCreator(QuestionDTO question, int questionNumber)
        {
            GetEmptySquare();
            PdfPTable table = new PdfPTable(2)
            {
                KeepTogether = true,
                WidthPercentage = 100,
            };
            table.SetWidths(new float[] { 1f, 12f });

            table.AddCell(new PdfPCell(new Paragraph(questionNumber + ". " + question.QuestionText, _polishFont))
            {
                Border = _border,
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_LEFT

            });

            foreach (var answer in question.AnswersDTO)
            {
                table.AddCell(new PdfPCell(GetEmptySquare())
                {
                    Border = _border,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 4
                });
                table.AddCell(new PdfPCell(new Paragraph(answer.TextAnswer, _polishFont))
                {
                    Border = _border,
                });
            }
            return table;
        }
        public static PDFHeader CreatePageEventHelper(int testID)
        {
            return new PDFHeader(testID);
        }

        public static Image GetEmptySquare()
        {
            if (image == null)
            {
                int size = 100;
                int thickness = 10;
                System.Drawing.Bitmap flag = new System.Drawing.Bitmap(size, size);
                System.Drawing.Graphics flagGraphics = System.Drawing.Graphics.FromImage(flag);
                flagGraphics.FillRectangle(System.Drawing.Brushes.White, 0, 0, size, size);
                flagGraphics.FillRectangle(System.Drawing.Brushes.Black, 0, 0, thickness, size);
                flagGraphics.FillRectangle(System.Drawing.Brushes.Black, size - thickness, 0, thickness, size);
                flagGraphics.FillRectangle(System.Drawing.Brushes.Black, 0, 0, size, thickness);
                flagGraphics.FillRectangle(System.Drawing.Brushes.Black, 0, size - thickness, size, thickness);
                image = Image.GetInstance(flag, System.Drawing.Imaging.ImageFormat.Jpeg);
                image.ScaleAbsolute(_fontsize, _fontsize);
            }
            return image;
        }

        public static LinkedList<AnswerPositionDTO> getAbsolutePositionOfAnswers(PdfPTable table, QuestionDTO question, Document document, PdfWriter writer)
        {
            var answerPositions = new LinkedList<AnswerPositionDTO>();
            var answersDTO = question.AnswersDTO;
            answersDTO.Reverse();
            var end = writer.GetVerticalPosition(false);
            var rows = table.Rows;
            rows.RemoveAt(0);
            rows.Reverse();

            var lol = BaseColor.YELLOW;
            int i = 0;
            foreach (var row in rows)
            {
                answerPositions.AddFirst(new AnswerPositionDTO()
                {
                    X = document.Left,
                    Y = end + row.GetMaxRowHeightsWithoutCalculating(),
                    Height = row.GetMaxRowHeightsWithoutCalculating(),
                    Width = row.GetCells().ElementAt(0).Width,
                    PageNumber = writer.CurrentPageNumber,
                    AnswerID = answersDTO.ElementAt(i).Id
                });

                lol = lol == BaseColor.YELLOW ? BaseColor.BLUE : BaseColor.YELLOW;
               
                end = answerPositions.First().Y;
                ++i;
            }
            return answerPositions;
        }
        public static string GetMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class PDFHeader : PdfPageEventHelper
    {
        int _testID;
        public PDFHeader(int testID)
        {
            _testID = testID;
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            string qrcodeString = string.Join("/",
                new string[] {
                    _testID.ToString(),
                    writer.PageNumber.ToString()
                });

            QrCodeED qrcode = new QrCodeED();
            System.Drawing.Bitmap btm = qrcode.Encode(qrcodeString);

            Image pdfImage = Image.GetInstance(btm, System.Drawing.Imaging.ImageFormat.Jpeg);
            pdfImage.ScaleAbsolute(50f, 50f);

            PdfPTable table = new PdfPTable(3)
            {
                HorizontalAlignment = (Element.ALIGN_CENTER),
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 1f, 6f, 1f });

            PdfPCell leftCell = new PdfPCell(pdfImage)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = Rectangle.NO_BORDER
            };


            PdfPCell centerCell = new PdfPCell(new Paragraph("NAZWA TESTU \nDATA \nNAZWISKO IMIE"));

            PdfPCell rightCell = new PdfPCell(pdfImage)
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = Rectangle.NO_BORDER
            };

            table.AddCell(leftCell);
            table.AddCell(centerCell);
            table.AddCell(rightCell);
            document.Add(table);
        }
    }





}
