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
        public List<AnswerPositionDTO> AnswerPositionDTO
        {
            get
            {
                return doc.ExamAnswerPositions;
            }
        }

        public DocumentCreator(ExamDTO exam)
        {
            doc = new PDFDocument(exam.Id);
            foreach (var item in exam.QuestionsDTO)
            {
                doc.AddExercise(item);
            }
            doc.SaveDocument();
        }
    }
}
