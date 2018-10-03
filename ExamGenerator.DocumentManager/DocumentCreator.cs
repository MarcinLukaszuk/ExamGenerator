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
        ExamDTO _exam;
        public DocumentCreator(ExamDTO exam)
        {
            IPDFDocument doc = new PDFDocument(exam.Id);

            for (int i = 0; i < 30; i++)
            {
                doc.AddExercise(exam.Questions.First());
            }
            doc.SaveDocument();
        }
    }
}
