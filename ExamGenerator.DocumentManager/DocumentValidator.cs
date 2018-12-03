﻿using System;
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
        private Dictionary<int, List<Bitmap>> _exams;

        public DocumentValidator(List<Bitmap> bitmapList)
        {
            _bitmapList = bitmapList;
            _exams = new Dictionary<int, List<Bitmap>>();
        }

        public List<int> GetExamIDs()
        {
            var tmp = new List<int>();
            foreach (var item in _bitmapList)
            {
                var examID = BitmapAnalyser.GetExamID(item);

                if (!_exams.ContainsKey(examID))
                    _exams.Add(examID, new List<Bitmap>() { item });
                else
                    _exams[examID].Add(item);

                tmp.Add(examID);
            }
            return tmp.Distinct().ToList();
        }

        public void CheckExam(int examID, List<AnswerPositionDTO> answerPositionsDTO)
        {
            var bitmapList = _exams[examID];
            foreach (var bitmap in bitmapList)
            {
                var pageNumber = BitmapAnalyser.GetExamPage(bitmap);
                var pageAnswers = answerPositionsDTO.Where(x => x.PageNumber == pageNumber).OrderBy(x=>x.Y).ToList();

                foreach (var answer in pageAnswers)
                {
                    var answerBitmap = BitmapAnalyser.GetAnswerBitmap(bitmap, answer);
                    var answerBitmapp = BitmapAnalyser.CheckAnswerValue(answerBitmap);
                    Console.WriteLine(answerBitmapp==answer.AnswerDTO.IfCorrect?"Dobrze":"Zle");
                }
            }
        }
    }
}
