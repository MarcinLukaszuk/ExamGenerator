using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;

namespace ExamGenerator.Controllers
{
    public class ResultsController : Controller
    {

        IResultService _resultService;
        IStudentGroupService _studentGroupService;

        public ResultsController(IResultService resultService, IStudentGroupService studentGroupService)
        {
            _resultService = resultService; ;
            _studentGroupService = studentGroupService;
        }


        // GET: Results
        public ActionResult Index(int? examCoreStudentGroupID)
        {
            List<Result> results = new List<Result>();
            if (examCoreStudentGroupID == null)
                results = _resultService.GetAll().ToList();
            else
                results = _resultService.GetResultsByStudentGroupAndExam2(examCoreStudentGroupID);

            return View(results);
        }

        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = _resultService.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Results/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GeneratedExamID,StudentID,Points")] Result result)
        {
            if (ModelState.IsValid)
            {
                _resultService.Insert(result);

                return RedirectToAction("Index");
            }


            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = _resultService.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }

            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GeneratedExamID,StudentID,Points")] Result result)
        {
            if (ModelState.IsValid)
            {
                _resultService.Update(result);
                return RedirectToAction("Index");
            }

            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = _resultService.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = _resultService.Find(id);
            _resultService.Delete(result);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}
