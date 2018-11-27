using ExamGeneratorModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.DocumentManager
{
    public interface IPDFDocument
    {
        void AddExercise(QuestionDTO question);
        void SaveDocument();
        List<AnswerPositionDTO> ExamAnswerPositions { get; }
        string Filename { get; }
        string Filepath{ get; }
    }
}
