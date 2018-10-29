using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using ExamGenerator.DocumentManager.ZIPValidator;
using System.Drawing;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.DTO;

namespace ExamGenerator.DocumentManager
{
    public class DocumentValidator
    {
        List<Bitmap> bitmaps;

        public DocumentValidator(string filenamePath)
        {
            bitmaps = ZIPDirectory.GetAllBitmapsOfZipArchive(filenamePath);
        }

        public List<int> GetExamIDs()
        {
            return bitmaps.Select(x => new BitmapAnalyzer(x).ExamID).Distinct().ToList();
        }

        public void validateExam(int examID, List<AnswerPositionDTO> answerPositions)
        {
            foreach (var bitmapAnalyzer in bitmaps.Select(x => new BitmapAnalyzer(x)).Where(x => x.ExamID == examID).ToList())
            {
                var answerPos = answerPositions.Where(x => x.PageNumber == bitmapAnalyzer.PageNumber).ToList();
                foreach (var answer in answerPos)
                {
                    var answerResult = bitmapAnalyzer.CheckAnswer(answer);

                    Console.WriteLine(answer.AnswerDTO.IfCorrect == answerResult ? "Dobrze" : "Zle");
                }
            }
        }
    }
}
