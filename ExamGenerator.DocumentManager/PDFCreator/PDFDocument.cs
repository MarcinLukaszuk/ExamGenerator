using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamGeneratorModel.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ExamGenerator.DocumentManager.PDFCreator
{
    public class PDFDocument : IPDFDocument
    {
        Document _document;
        PdfWriter _writer;
        string _filename;
        int questionCounter;
        LinkedList<PdfPTable> tables;

        public string Filename
        {
            get
            {
                return _filename;
            }
        }
        public PDFDocument(int examID)
        {
            _filename = GetMD5(examID.ToString()) + ".pdf";
            _document = new Document(PageSize.A4, 36, 36, 36, 36);
            _writer = PdfWriter.GetInstance(_document, new FileStream(Filename, FileMode.Create));
            _writer.PageEvent = PDFHelpers.CreatePageEventHelper(examID);
            _document.Open();
            questionCounter = 1;
            tables = new LinkedList<PdfPTable>();
        }
        public void AddExercise(QuestionDTO question)
        {
            tables.AddLast(PDFHelpers.PDFTableCreator(question, questionCounter++));
        }

        public void SaveDocument()
        {
            buildDocument();
            _document.Close();
        }

        private void buildDocument()
        {
            float poczatek = 0;
            float koniec = 0;
            var lol = BaseColor.BLACK;
            foreach (var table in tables)
            {
                poczatek = _writer.GetVerticalPosition(false);
                _document.Add(table);
                koniec = _writer.GetVerticalPosition(false);
                if (poczatek < koniec)
                {
                    _document.Add(new Chunk("\n"));
                    koniec = _writer.GetVerticalPosition(false);
                }
                if (lol == BaseColor.BLACK)
                    lol = BaseColor.BLUE;
                else
                    lol = BaseColor.BLACK;

                _document.Add(new Rectangle(10, poczatek, 60, koniec, 0) { BackgroundColor = lol });
            }
        }
        private string GetMD5(string input)
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
}
