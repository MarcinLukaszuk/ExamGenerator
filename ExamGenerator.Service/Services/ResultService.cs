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
                var generatedExam = _context.GeneratedExams.Where(x => x.ExamCoreStudentGroup.ExamCoreID == examCoreID && x.StudentGroupStudentID == studentGroupStudentID.Id).FirstOrDefault();

                if (generatedExam == null) return results;

                var result = _context.Results.Where(x => x.StudentID == student.Id && x.GeneratedExamID == generatedExam.Id).FirstOrDefault();
                if (result != null) results.Add(result);
            }
            return results;
        }

        public List<Result> GetResultsByStudentGroupAndExam2(int? examCoreStudentGroupID)
        {
            var examCoreStudentGroup = _context.ExamCoreStudentGroups.FirstOrDefault(x => x.Id == examCoreStudentGroupID);

            var generatedExams = _context.GeneratedExams.Where(x => x.ExamCoreStudentGroupID == examCoreStudentGroupID).ToList();
            List<Result> results = new List<Result>();

            foreach (var generatedExam in generatedExams)
            {
                var result = _context.Results.Where(x => x.StudentID == generatedExam.StudentGroupStudent.Student.Id && x.GeneratedExamID == generatedExam.Id).FirstOrDefault();
                if (result != null) results.Add(result);

            }
            return results;
        }


        public int? GetStudentIDByExamID(int? examID)
        {
            if (examID != null)
            {
                var generatedExam = _context.GeneratedExams.Find(examID);
                var studentGroupStudents = _context.StudentGroupStudents.Find(generatedExam.StudentGroupStudentID);
                if (studentGroupStudents != null)
                {
                    return studentGroupStudents.StudentID;
                }
            }
            return null;
        }


        public void DeletePreviousResults(int? examID)
        {
            var deletedResults = _context.Results.Where(x => x.GeneratedExamID == examID).ToList();
            _context.Results.RemoveRange(deletedResults);
            _context.SaveChanges();
        }

        public void SetIsValidatetFlagByExamID(int? examID)
        {
            var generatedExam = _context.GeneratedExams.Find(examID);
            var studentGroupStudent = _context.StudentGroupStudents.Find(generatedExam.StudentGroupStudentID);
            var examCoreStudentsgroup = _context.ExamCoreStudentGroups
                .Where(x => x.Id == generatedExam.ExamCoreStudentGroupID)
                .FirstOrDefault();

            examCoreStudentsgroup.IsValidated = true;
            _context.SaveChanges();
        }

        public int? GetExamCoreIDByExamID(int? examID)
        {
            return _context.GeneratedExams.Find(examID)?.ExamCoreStudentGroup.ExamCoreID;
        }

        public int? GetStudentGroupIDByExamID(int? examID)
        {
            var generatedExam = _context.GeneratedExams.Find(examID);
            var studentGroupStudent = _context.StudentGroupStudents.Find(generatedExam.StudentGroupStudentID);
            return studentGroupStudent.StudentGroupID;
        }
    }
}
