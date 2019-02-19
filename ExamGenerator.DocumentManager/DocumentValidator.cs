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
                var examID = BitmapAnalyser.GetExamID(bitmap);

                if (!_examsDictionary.ContainsKey(examID))
                    _examsDictionary.Add(examID, new List<Bitmap>() { bitmap });
                else
                    _examsDictionary[examID].Add(bitmap);

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

                foreach (var answer in pageAnswers)
                {
                    var answerBitmap = BitmapAnalyser.GetAnswerBitmap(bitmap, answer);
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
