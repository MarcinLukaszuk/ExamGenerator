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
        IStudentGroupService _studentGroupService;
        public ResultService(IDbContext dbContext, IStudentGroupService studentGroupService) : base(dbContext)
        {
            _context = dbContext;
            _studentGroupService = studentGroupService;
        }

        public List<Result> GetResultsByStudentGroupAndExam(int? studentGroupID, int? examCoreID)
        {
            var studentsID = _studentGroupService.GetStudentsByStudentGroup(studentGroupID);
            List<Result> results = new List<Result>();

            foreach (var student in studentsID)
            {
                var studentGroupStudentID = _context.StudentGroupStudents.Where(x => x.StudentID == student.Id && x.StudentGroupID == studentGroupID).FirstOrDefault();
                var generatedExam = _context.GeneratedExams.Where(x => x.ExamCoreID == examCoreID && x.StudentGroupStudentID == studentGroupStudentID.Id).FirstOrDefault();

                if (generatedExam == null) return results;

                var result = _context.Results.Where(x => x.StudentID == student.Id && x.GeneratedExamID == generatedExam.Id).FirstOrDefault();
                if (result != null) results.Add(result);
            }
            return results;
        }
    }
}
