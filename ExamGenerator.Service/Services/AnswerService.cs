using ExamGenerator.Service.EF;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Services
{
    public class AnswerService : Service<Answer>, IAnswerService
    {
        public AnswerService(IDataModelEF dataModelEF) : base(dataModelEF)
        {
        }
    }
}
