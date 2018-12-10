using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Services
{
    public class ResultService : Service<Result>, IResultService
    {
        private readonly IDbContext _context;
        public ResultService(IDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
