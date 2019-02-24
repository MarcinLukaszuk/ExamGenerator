using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using Microsoft.AspNet.Identity;

namespace ExamGenerator.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        // GET: Students
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            return View(_studentService.GetAll().Where(x => x.Owner == userID).ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentService.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,SurName,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Owner = User.Identity.GetUserId();
                _studentService.Insert(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudentsFromFile(HttpPostedFileBase FileUpload)
        {
            try
            {
                if (FileUpload != null)
                {
                    using (StreamReader reader = new StreamReader(FileUpload.InputStream, Encoding.Default, true))
                    {
                        var userID = User.Identity.GetUserId();
                        await addStudentsFromFile(reader, userID);
                        return Json(
                        new
                        {
                            success = true,
                            failure = false,
                            responseText = "Students have been successfuly updated!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, failure = true, responseText = "Error during updating questions." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, failure = true, responseText = "Error during updating questions." }, JsonRequestBehavior.AllowGet);


        }

        private async Task<int> addStudentsFromFile(StreamReader reader, string userID)
        {
            var line = string.Empty;
            
            while ((line = reader.ReadLine()) != null)
            {
                var array = line.Split(';');
                var tmpStudent = new Student()
                {
                    Name = array[0],
                    SurName = array[1],
                    Email = array[2],
                    Owner = userID
                };
                _studentService.Insert(tmpStudent);
            }

            return 0;
        }
        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentService.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SurName,Email,Owner")] Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.Update(student);

                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentService.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = _studentService.Find(id);
            _studentService.Delete(student);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}
