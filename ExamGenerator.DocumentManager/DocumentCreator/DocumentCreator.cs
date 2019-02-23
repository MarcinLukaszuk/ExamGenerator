using ExamGenerator.DocumentManager.QRCodeGenerator;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamGeneratorModel.DTO;
using ExamGenerator.DocumentManager.PDFCreator;

namespace ExamGenerator.DocumentManager
{
    public class DocumentCreator
    {
        IPDFDocument doc;
        private List<ExamDTO> examsToGenerate;  
        private List<PDFDocument> pdfDocuments; 
        private string pathToDirectory;

        public List<PDFDocument> PDFDocuments
        {
            get { return pdfDocuments; }
            set { pdfDocuments = value; }
        }
        public List<AnswerPositionDTO> AnswerPositionDTO
        {
            get
            {
                return doc.ExamAnswerPositions;
            }
        }
        public string Filename
        {
            get
            {
                return doc.Filename;
            }
        }
        public string Filepath
        {
            get
            {
                return doc.Filepath;
            }
        }

        public DocumentCreator(string _pathToDirectory)
        {
            examsToGenerate = new List<ExamDTO>();
            pdfDocuments = new List<PDFDocument>();
            pathToDirectory = _pathToDirectory;
        }

        public void AddExamToGenerate(ExamDTO exam)
        {
            examsToGenerate.Add(exam);
        }
        public void Generate()
        {
            foreach (var examDTO in examsToGenerate)
            {
                var pdfDocument = new PDFDocument(examDTO, pathToDirectory);             
                pdfDocuments.Add(pdfDocument);
                pdfDocument.SaveDocument();
            }
        }
    }
}
