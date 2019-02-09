using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;
using Microsoft.AspNet.Identity;

namespace ExamGenerator.Controllers
{
    [Authorize]
    public class StudentGroupsController : Controller
    {

        private IStudentService _studentService;
        private IStudentGroupService _studentGroupService;
        private IStudentGroupStudentService _studentGroupStudentService;
        private IExamCoreService _examCoreService;
        private IExamCoreStudentGroupService _examCoreStudentGroupService;
        public StudentGroupsController
            (
            IStudentService studentService,
            IStudentGroupService studentGroupService,
            IStudentGroupStudentService studentGroupStudentService,
            IExamCoreService examCoreService,
            IExamCoreStudentGroupService examCoreStudentGroupService
            )
        {
            _studentService = studentService;
            _studentGroupService = studentGroupService;
            _studentGroupStudentService = studentGroupStudentService;
            _examCoreService = examCoreService;
            _examCoreStudentGroupService = examCoreStudentGroupService;
        }

        // GET: StudentGroups
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            return View(_studentGroupService.GetAll().Where(x => x.Owner == userID).ToList());
        }

        // GET: StudentGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGroup studentGroup = _studentGroupService.Find(id);
            if (studentGroup == null)
            {
                return HttpNotFound();
            }
            var examsCoreVM = _studentGroupService.GetExamsCoreByStudentGroup(studentGroup.Id).ToList().Select(x => new ExamCoreStudentGroupViewModel() { ExamCore = x }).ToList();

            foreach (var item in examsCoreVM)
            {
                item.IsGenerated = _examCoreStudentGroupService.CheckIfExamCoreIsGenerated(item.ExamCore.Id, studentGroup.Id);
                item.IsValidated = _examCoreStudentGroupService.CheckIfExamCoreIsValidated(item.ExamCore.Id, studentGroup.Id);
                item.ZIPArchiveName = _examCoreStudentGroupService.GetGenerategExamArchivePath(item.ExamCore.Id, studentGroup.Id);
            }

            StudentGroupViewModel viewModel = new StudentGroupViewModel()
            {
                StudentGroup = studentGroup,
                Students = _studentGroupService.GetStudentsByStudentGroup(studentGroup.Id).ToList(),
                ExamsCore = examsCoreVM
            };
            return View(viewModel);
        }

        // GET: StudentGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] StudentGroup studentGroup)
        {
            if (ModelState.IsValid)
            {
                studentGroup.Owner = User.Identity.GetUserId();
                _studentGroupService.Insert(studentGroup);

                return RedirectToAction("Index");
            }

            return View(studentGroup);
        }

        // GET: StudentGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userID = User.Identity.GetUserId();
            StudentGroup studentGroup = _studentGroupService.Find(id);
            if (studentGroup == null)
            {
                return HttpNotFound();
            }
            StudentGroupViewModel viewModel = new StudentGroupViewModel()
            {
                StudentGroup = studentGroup,
                Students = _studentGroupService.GetStudentsByStudentGroup(studentGroup.Id).ToList(),
                ExamsCore = _studentGroupService.GetExamsCoreByStudentGroup(studentGroup.Id).Select(x => new ExamCoreStudentGroupViewModel() { ExamCore = x }).ToList()
            };

            ViewBag.StudentsList = new SelectList(_studentGroupService.GetStudentNotInStudentGroup(studentGroup.Id).Where(x => x.Owner == userID).Select(p =>
                                     new SelectListItem
                                     {
                                         Value = p.Id.ToString(),
                                         Text = p.Email
                                     }),
                                  "Value",
                                  "Text");
            ViewBag.ExamsCoreList = new SelectList(_studentGroupService.GetExamsCoreNotInStudentGroup(studentGroup.Id).Where(x => x.Owner == userID).Select(p =>
                           new SelectListItem
                           {
                               Value = p.Id.ToString(),
                               Text = p.Name
                           }),
                           "Value",
                           "Text");

            return View(viewModel);
        }

        // POST: StudentGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Owner")] StudentGroup studentGroup)
        {
            if (ModelState.IsValid)
            {
                _studentGroupService.Update(studentGroup);
                return RedirectToAction("Index");
            }
            return View(studentGroup);
        }
        public class StudentComparer : IEqualityComparer<Student>
        {
            public bool Equals(Student emp1, Student emp2)
            {
                if (emp1.Id == emp2.Id)
                    return true;
                return false;
            }

            public int GetHashCode(Student obj)
            {
                return obj == null ? 0 : obj.Id.GetHashCode();
            }
        }
        // GET: StudentGroups/Edit/5
        public ActionResult EditStudents(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userID = User.Identity.GetUserId();
            StudentGroup studentGroup = _studentGroupService.Find(id);
            if (studentGroup == null)
            {
                return HttpNotFound();
            }

            var checkedStudents = _studentGroupService.GetStudentsByStudentGroup(studentGroup.Id).ToList();
            var uncheckedStudents = _studentService.GetAll().Where(x => x.Owner == userID).ToList().Except(checkedStudents, new StudentComparer()).ToList();

            var checkedStudentsViewModel = AutoMapper.Mapper.Map<List<EditStudentViewModel>>(checkedStudents);
            var uncheckedStudentsViewModel = AutoMapper.Mapper.Map<List<EditStudentViewModel>>(uncheckedStudents);
            checkedStudentsViewModel.ForEach(x => x.IsAssociatedToStudentGroup = true);
            uncheckedStudentsViewModel.ForEach(x => x.IsAssociatedToStudentGroup = false);

            var editStudentsViewModel = checkedStudentsViewModel.Concat(uncheckedStudentsViewModel).OrderBy(x => x.Email).ToList();

            StudentGroupEditStudentsViewModel viewModel = new StudentGroupEditStudentsViewModel()
            {
                StudentGroup = studentGroup,
                Students = editStudentsViewModel
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudents([Bind(Include = "StudentGroup,Students")] StudentGroupEditStudentsViewModel viewModel)
        {

            if (viewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentGroupID = viewModel.StudentGroup?.Id;

            if (studentGroupID != null)
            {
                var studentGroup = _studentGroupService.GetByID((int)studentGroupID);
                foreach (var studentViewModel in viewModel.Students)
                {
                    var student = _studentService.GetByID(studentViewModel.Id);

                    if (studentViewModel.IsAssociatedToStudentGroup == true)
                    {
                        _studentGroupStudentService.AssociateStudentToStudentGroup(student, studentGroup);
                    }
                    else if (studentViewModel.IsAssociatedToStudentGroup == false)
                    {
                        _studentGroupStudentService.DisassociateStudentFromStudentGroup(student, studentGroup);
                    }
                }
            }

            return RedirectToAction("Details", new { id = studentGroupID });
        }

        // GET: StudentGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGroup studentGroup = _studentGroupService.Find(id);
            if (studentGroup == null)
            {
                return HttpNotFound();
            }
            return View(studentGroup);
        }

        // POST: StudentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentGroup studentGroup = _studentGroupService.Find(id);
            _studentGroupService.Delete(studentGroup);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult AddStudentsToGroup(HttpPostedFileBase FileUpload, string studentGroupID)
        {
            StudentGroup studentGroup = null;
            if (int.TryParse(studentGroupID, out var sgID))
            {
                studentGroup = _studentGroupService.Find(sgID);
            }

            if (studentGroup != null)
            {
                using (StreamReader reader = new StreamReader(FileUpload.InputStream, Encoding.Default, true))
                {
                    var line = reader.ReadLine();
                    if (line != "Name;Surname;Email")
                    {
                        reader.Close();
                        return RedirectToAction("Index");
                    }
                    while ((line = reader.ReadLine()) != null)
                    {
                        var array = line.Split(';');
                        var tmpStudent = new Student()
                        {
                            Name = array[0],
                            SurName = array[1],
                            Email = array[2]
                        };
                        if (_studentService.GetStudentByEmail(tmpStudent.Email) == null)
                        {
                            _studentService.Insert(tmpStudent);
                        }
                        else
                        {
                            tmpStudent = _studentService.GetStudentByEmail(tmpStudent.Email);
                        }

                        _studentGroupStudentService.AssociateStudentToStudentGroup(tmpStudent, studentGroup);
                    }
                    reader.Close();
                }
            }
            return RedirectToAction("Edit", "StudentGroups", new { id = studentGroupID });
        }

        [HttpPost]
        public ActionResult AssociateStudentsToGroup(HttpPostedFileBase FileUpload, string studentGroupID)
        {
            return RedirectToAction("Edit", "StudentGroups", new { id = studentGroupID });
        }

        [HttpPost]
        public bool AssociateStudentToGroup(string studentID, string studentGroupID)
        {
            Student student = new Student();
            StudentGroup studentGroup = new StudentGroup();
            bool returnValue = false;
            if (int.TryParse(studentID, out var studentIDINT))
                student = _studentService.Find(studentIDINT);

            if (int.TryParse(studentGroupID, out var studentGroupIDINT))
                studentGroup = _studentGroupService.Find(studentGroupIDINT);

            if (student != null && student.Id != 0 && studentGroup != null && studentGroup.Id != 0)
            {
                returnValue = _studentGroupStudentService.AssociateStudentToStudentGroup(student, studentGroup);
            }
            return returnValue;
        }

        [HttpPost]
        public ActionResult DisassociateStudentsToGroup(string studentID, string studentGroupID)
        {
            Student student = new Student();
            StudentGroup studentGroup = new StudentGroup();

            if (int.TryParse(studentID, out var studentIDINT))
                student = _studentService.Find(studentIDINT);

            if (int.TryParse(studentGroupID, out var studentGroupIDINT))
                studentGroup = _studentGroupService.Find(studentGroupIDINT);

            if (student != null && student.Id != 0 && studentGroup != null && studentGroup.Id != 0)
                _studentGroupStudentService.DisassociateStudentFromStudentGroup(student, studentGroup);

            return RedirectToAction("Edit", "StudentGroups", new { id = studentGroupID });
        }

        [HttpPost]
        public bool AssociateExamCoreToGroup(int? examCoreID, int? studentGroupID)
        {
            bool returnValue = false;
            if (examCoreID == null || studentGroupID == null)
            {
                return returnValue;
            }
            ExamCore examCore = _examCoreService.Find((int)examCoreID);
            StudentGroup studentGroup = _studentGroupService.Find((int)studentGroupID);

            if (examCore != null && studentGroup != null)
            {
                returnValue = _examCoreStudentGroupService.AssociateExamToStudentGroup(examCore, studentGroup);
            }
            return returnValue;
        }

        [HttpPost]
        public ActionResult DisassociateExamFromGroup(int? examCoreID, int? studentGroupID)
        {
            ExamCore examCore = _examCoreService.Find((int)examCoreID);
            StudentGroup studentGroup = _studentGroupService.Find((int)studentGroupID);

            if (examCore != null && studentGroup != null)
                _examCoreStudentGroupService.DisassociateExamFromStudentGroup(examCore, studentGroup);

            return RedirectToAction("Edit", "StudentGroups", new { id = studentGroupID });
        }

        public FileResult GetExamsArchive(string filename)
        {
            var path = HostingEnvironment.MapPath("~/GeneratedExams");
            return File(path + "//" + filename, "application/zip", filename);
        }
    }
}
