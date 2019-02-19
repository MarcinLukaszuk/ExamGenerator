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
        private Document _document;
        private PdfWriter _writer;
        private string _filepath;
        private string _filename;
        private int _examID;
        private LinkedList<QuestionDTO> _questions;
        private List<AnswerPositionDTO> _examAnswerPositions;
        public int ExamID
        {
            get
            {
                return _examID;
            }
        }
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
                return _examAnswerPositions;
            }
        }

        public PDFDocument(ExamDTO examDTO, string path)
        {
            _document = new Document(PageSize.A4, 36, 36, 36, 36);

            _examID = examDTO.Id;
            _filename = examDTO.StudentFullName + PDFHelpers.GetMD5(_examID.ToString()) + ".pdf";
            _filepath = path + "\\";

            _writer = PdfWriter.GetInstance(_document, new FileStream(_filepath + _filename, FileMode.Create));
            _writer.PageEvent = PDFHelpers.CreatePageEventHelper(examDTO);
            _document.Open();

            _questions = new LinkedList<QuestionDTO>(examDTO.QuestionsDTO);
            _examAnswerPositions = new List<AnswerPositionDTO>();
        }
        public void AddExercise(QuestionDTO question)
        {
            _questions.AddLast(question);
        }

        public void SaveDocument()
        {
            int questionNumber = 1;
            foreach (var question in _questions)
            {
                var currentAnswerPositions = new LinkedList<AnswerPositionDTO>();
                var table = PDFHelpers.PDFTableCreator(question, questionNumber++);
                _document.Add(table);
                currentAnswerPositions = PDFHelpers.CreateAnswerPositionList(ExamID, table, question, _document, _writer);

                _examAnswerPositions.AddRange(currentAnswerPositions);
            }
            _document.Close();
        }
    }
}
