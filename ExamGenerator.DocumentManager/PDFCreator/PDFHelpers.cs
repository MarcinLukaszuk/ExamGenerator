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

        public static PdfPTable PDFTableCreator(QuestionDTO question, int questionNumber)
        {
            PdfPTable table = new PdfPTable(2) {
                KeepTogether=true,
                WidthPercentage=100,
            };
            table.SetWidths(new float[] { 1f, 12f });

            table.AddCell(new PdfPCell(new Paragraph(questionNumber + ". " + question.QuestionText, _polishFont))
            {
                Border = Rectangle.NO_BORDER,
                Colspan = 2,
                HorizontalAlignment = 0,//0=Left, 1=Centre, 2=Right
            });

            foreach (var answer in question.Answers)
            {
                table.AddCell(new PdfPCell(new Paragraph("a"))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = (float)_fontsize / 2,
                    PaddingRight = (float)_fontsize / 2,
                    Border = Rectangle.NO_BORDER
                });
                table.AddCell(new PdfPCell(new Paragraph(answer.TextAnswer, _polishFont))
                {
                    Border = Rectangle.NO_BORDER
                });
            }
            return table;
        }


        public static PDFHeader CreatePageEventHelper(int testID)
        {
            return new PDFHeader(testID);
        }
    }

    public class PDFHeader: PdfPageEventHelper
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
