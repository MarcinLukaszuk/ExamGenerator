using ExamGenerator.DocumentManager.QRCodeGenerator;
using ExamGenerator.DocumentManager.DocumentGenerator;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using ExamGeneratorModel.DTO;

namespace ExamGenerator.DocumentManager
{
    public class DocumentCreator
    {
        
        ExamDTO _exam;
        public DocumentCreator(ExamDTO exam)
        {
            _exam = exam;
            WordDocument _wordDocument = new WordDocument(exam);
            for (int i = 0; i < 30; i++)
            {
                foreach (var question in exam.Questions)
                {
                    _wordDocument.AddExercise(question);
                }
            }
            _wordDocument.Document.Save();
            _wordDocument. dzial();





        }
    }
}
