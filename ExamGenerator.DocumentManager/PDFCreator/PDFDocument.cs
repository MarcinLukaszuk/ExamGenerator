using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ExamGenerator.DocumentManager.PDFCreator
{
    public class PDFDocument : IPDFDocument
    {
        Document _document;
        PdfWriter _writer;
        string _filepath;
        string _filename;
        LinkedList<PdfPTable> tables;
        LinkedList<QuestionDTO> questions;
        List<AnswerPositionDTO> examAnswerPositions;
        public int ExamID { get; set; }
        public string Filename
        {
            get
            {
                return _filename;
            }
        }
        public string Filepath
        {
            get
            {
                return _filepath;
            }
        }
        public List<AnswerPositionDTO> ExamAnswerPositions
        {
            get
            {
                return examAnswerPositions;
            }
        }

        public PDFDocument(ExamDTO exam, string path)
        {
            ExamID = exam.Id;
            _filename =exam.StudentFullName+ PDFHelpers.GetMD5(ExamID.ToString()) + ".pdf";
            _filepath = path + "\\";
            _document = new Document(PageSize.A4, 36, 36, 36, 36);
            _writer = PdfWriter.GetInstance(_document, new FileStream(_filepath + _filename, FileMode.Create));
            _writer.PageEvent = PDFHelpers.CreatePageEventHelper(exam);
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
                currentAnswerPositions = PDFHelpers.getAbsolutePositionOfAnswers(ExamID,table, question, _document, _writer);

                examAnswerPositions.AddRange(currentAnswerPositions);
            }
        }
    }
}
