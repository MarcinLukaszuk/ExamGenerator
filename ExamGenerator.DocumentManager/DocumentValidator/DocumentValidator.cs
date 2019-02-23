using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.DTO;

namespace ExamGenerator.DocumentManager
{
    public class DocumentValidator
    {
        private List<Bitmap> _bitmapList;
        private Dictionary<int, List<Bitmap>> _examsDictionary;
        private List<int> _examIDs;

        public DocumentValidator(List<Bitmap> bitmapList)
        {
            _bitmapList = bitmapList;
            _examsDictionary = new Dictionary<int, List<Bitmap>>();
            _examIDs = new List<int>();

            foreach (var bitmap in _bitmapList)
            { 
                var bitmap2 = BitmapAnalyser.extractDocumentFromBitmap(bitmap);

                bitmap2.Save("C:\\Users\\Marcin\\source\\repos\\trapez\\trapez\\bin\\Debug\\output\\bitmap2.jpg");

                var bitmap3 = BitmapAnalyser.getBinarizedBitmap(bitmap2);
                bitmap3.Save("C:\\Users\\Marcin\\source\\repos\\trapez\\trapez\\bin\\Debug\\output\\bitmap3.jpg");

                var standarizedBitmap = BitmapAnalyser.ResizeToStandard(bitmap3);
                standarizedBitmap.Save("C:\\Users\\Marcin\\source\\repos\\trapez\\trapez\\bin\\Debug\\output\\standarizedBitmap.jpg");

                var examID = BitmapAnalyser.GetExamID(bitmap3);
                  examID = BitmapAnalyser.GetExamID(bitmap);

                if (!_examsDictionary.ContainsKey(examID))
                    _examsDictionary.Add(examID, new List<Bitmap>() { bitmap3 });
                else
                    _examsDictionary[examID].Add(bitmap3);

                _examIDs.Add(examID);
            }
        }

        public List<int> GetExamIDs()
        {
            return _examIDs.Distinct().ToList();
        }

        public ResultDTO CheckExam(int examID, List<AnswerPositionDTO> answerPositionsDTO)
        {
            Dictionary<int, int> bitmapsDictionary = new Dictionary<int, int>();
            int questions = 0;
            var bitmapList = _examsDictionary[examID];
            foreach (var bitmap in bitmapList)
            {
                var pageNumber = BitmapAnalyser.GetExamPage(bitmap);
                var pageAnswers = answerPositionsDTO.Where(x => x.PageNumber == pageNumber).OrderBy(x => x.Y).ToList();
               
                var standarizedBitmap = BitmapAnalyser.ResizeToStandard(bitmap);
                standarizedBitmap.Save("C:\\Users\\Marcin\\source\\repos\\trapez\\trapez\\bin\\Debug\\output\\standard.jpg");

                foreach (var answer in pageAnswers.OrderBy(x => x.Y))
                {
                    var answerBitmap = BitmapAnalyser.GetAnswerBitmap(standarizedBitmap, answer);
                
                    var answerValue = BitmapAnalyser.CheckValue(answerBitmap);
                  
                    if (bitmapsDictionary.ContainsKey(answer.AnswerDTO.QuestionID) == false)
                    {
                        questions++;
                        bitmapsDictionary.Add(answer.AnswerDTO.QuestionID, 1);
                    }
                    //check if value on paper is correct
                    if (!(answerValue == answer.AnswerDTO.IfCorrect))
                        bitmapsDictionary[answer.AnswerDTO.QuestionID] = 0;
                }
            }

            return new ResultDTO() { Points = bitmapsDictionary.Select(x => x.Value).Sum(), MaxPoints = questions, GeneratedExamID = examID };
        }
    }
}
