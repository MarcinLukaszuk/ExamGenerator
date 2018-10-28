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
        LinkedList<PdfPTable> tables;
        LinkedList<QuestionDTO> questions;
        List<AnswerPositionDTO> examAnswerPositions;
        public string Filename
        {
            get
            {
                return _filename;
            }
        }
        public List<AnswerPositionDTO> ExamAnswerPositions
        {
            get
            {
                return examAnswerPositions;
            }
        }

        public PDFDocument(int examID)
        {
            _filename = PDFHelpers.GetMD5(examID.ToString()) + ".pdf";
            _document = new Document(PageSize.A4, 36, 36, 36, 36);
            _writer = PdfWriter.GetInstance(_document, new FileStream(Filename, FileMode.Create));
            _writer.PageEvent = PDFHelpers.CreatePageEventHelper(examID);
            _document.Open();

            tables = new LinkedList<PdfPTable>();
            questions = new LinkedList<QuestionDTO>();
            examAnswerPositions = new List<AnswerPositionDTO>();
        }
        public void AddExercise(QuestionDTO question)
        {
            questions.AddLast(question);
        }

        public void SaveDocument()
        {
            buildDocument();
            _document.Close();
        }

        private void buildDocument()
        {
            int questionCounter = 1;
            foreach (var question in questions)
            {
                var currentAnswerPositions = new LinkedList<AnswerPositionDTO>();
                var table = PDFHelpers.PDFTableCreator(question, questionCounter++);
                _document.Add(table);
                currentAnswerPositions = PDFHelpers.getAbsolutePositionOfAnswers(table, question, _document, _writer);

                examAnswerPositions.AddRange(currentAnswerPositions);
            }
        }
    }
}
