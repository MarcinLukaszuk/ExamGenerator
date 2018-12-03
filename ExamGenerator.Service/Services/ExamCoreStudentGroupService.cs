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
                    ExamCoreStudentGroup sgs = new ExamCoreStudentGroup() { ExamCoreID = examCore.Id, StudentGroupID = studentGroup.Id };
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
    }
}
