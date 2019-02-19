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
    public class ExamCoreStudentGroupService : Service<ExamCoreStudentGroup>, IExamCoreStudentGroupService
    {
        private readonly IDbContext _context;
        public ExamCoreStudentGroupService(IDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public bool AssociateExamToStudentGroup(ExamCore examCore, StudentGroup studentGroup)
        {
            if (examCore != null && studentGroup != null)
            {
                var existingAssociate = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroup.Id && x.ExamCoreID == examCore.Id).FirstOrDefault();
                if (existingAssociate == null)
                {
                    ExamCoreStudentGroup sgs = new ExamCoreStudentGroup() { ExamCoreID = examCore.Id, StudentGroupID = studentGroup.Id, Version = 1 };
                    Insert(sgs);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void DisassociateExamFromStudentGroup(ExamCore examCore, StudentGroup studentGroup)
        {
            if (examCore != null && studentGroup != null)
            {
                var existingAssociate = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroup.Id && x.ExamCoreID == examCore.Id).FirstOrDefault();
                if (existingAssociate != null)
                {
                    Delete(existingAssociate);
                }
            }
        }

        public bool CheckIfExamCoreIsGenerated(int examCoreID, int studentGroupID)
        {
            var existingAssociate = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID && x.ExamCoreID == examCoreID).FirstOrDefault();
            if (existingAssociate?.IsGenerated == true)
                return true;
            return false;
        }

        public bool CheckIfExamCoreIsValidated(int examCoreID, int studentGroupID)
        {
            var existingAssociate = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID && x.ExamCoreID == examCoreID).FirstOrDefault();

            if (existingAssociate?.IsValidated == true)
                return true;
            return false;
        }


        public string GetGenerategExamArchivePath(int examCoreID, int studentGroupID)
        {
            var existingAssociate = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID && x.ExamCoreID == examCoreID).FirstOrDefault();
            if (existingAssociate?.ZIPArchiveName != "")
            {
                return existingAssociate.ZIPArchiveName;
            }
            return string.Empty;
        }

        public void SetExamArchivePath(int examCoreStudentGroup, string path)
        {
            var existingAssociate = _context.ExamCoreStudentGroups.Where(x => x.Id == examCoreStudentGroup).FirstOrDefault();
            if (existingAssociate != null)
            {
                existingAssociate.ZIPArchiveName = path;
                existingAssociate.IsGenerated = true;
                _context.SaveChanges();
            }
        }

        public int GetVersionsOfGenerategExam(int examCoreID, int studentGroupID)
        {
            var associatedExamsCore = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID && x.ExamCoreID == examCoreID).ToList();
            return associatedExamsCore.Max(x => x.Version);
        }
    }
}
