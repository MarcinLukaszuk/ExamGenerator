namespace ExamGenerator.DocumentManager.DocumentGenerator
{
    using ExamGenerator.DocumentManager.QRCodeGenerator;
    using ExamGeneratorModel.DTO;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Xceed.Words.NET;

    public class WordDocument
    {
        DocX _document;
        QrCodeED _qrCode;
        int exerciseCounter;
        string MD5String; 
        Footers footers;
        public DocX Document
        {
            get
            {
                return _document;
            }
        }

        public WordDocument(ExamDTO exam)
        {
            MD5String = GetMD5(exam.Id.ToString());
            _qrCode = new QrCodeED();
            _qrCode.Encode("chuj").Save(MD5String + ".jpg");
            
            _document = DocX.Create(MD5String + ".docx");
            _document.AddHeaders();
            _document.AddFooters();
            exerciseCounter = 1;
            
            // Add the page number in the odd Footers.
             _document.Footers.First.InsertParagraph("Page #").AppendPageNumber(PageNumberFormat.normal);
            //var picture = _document.AddImage(MD5String + ".jpg").CreatePicture(50, 50);
            _document.Footers.Even.InsertParagraph("Page #").AppendPageNumber(PageNumberFormat.normal);
            _document.Footers.Odd.InsertParagraph("Page #").AppendPageNumber(PageNumberFormat.normal);

        }
        public void dzial()
        {
            var lol = DocX.Load(MD5String + ".docx");
                }
        public void AddExercise(QuestionDTO question)
        {
            var rowNumber = question.Answers.Count + 1;
            var columnNumber = 2;
            var table = _document.AddTable(rowNumber, columnNumber);
            table.Rows[0].MergeCells(0, 1);
            table.Rows[0].Cells[0].Paragraphs[0].Append(exerciseCounter + ". " + question.QuestionText);
            table.Alignment = Alignment.center;
            table.AutoFit = AutoFit.Window;

            //for (int i = 0; i < 5; i++)
            //{
            //    table.SetBorder((TableBorderType)i, new Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
            //}


            for (int i = 1; i < rowNumber; i++)
            {
                table.Rows[i].Cells[0].Paragraphs[0].Append(((char)0x000025A1).ToString());
                table.Rows[i].Cells[0].Paragraphs[0].Alignment = Alignment.right;
                table.Rows[i].Cells[0].Width = 0;

                table.Rows[i].Cells[1].Paragraphs[0].Append(question.Answers[i - 1].TextAnswer);
            }

            // _document.InsertTable();
            var lol = _document.InsertParagraph();
            lol.InsertTableBeforeSelf(table);
            lol.SpacingBefore(1);
          
            exerciseCounter++;
        }




        private string GetMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
