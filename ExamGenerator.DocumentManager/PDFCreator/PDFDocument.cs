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
            
            _document.Add(tables.Last());
            _document.Add(new Paragraph("\n"));
        }

        public void SaveDocument()
        {
            _document.Close();
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
