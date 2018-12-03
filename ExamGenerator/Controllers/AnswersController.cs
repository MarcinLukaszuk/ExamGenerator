using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;

namespace ExamGenerator.Controllers
{
    public class AnswersController : Controller
    {
        private ExamGeneratorDBContext db = new ExamGeneratorDBContext();

        // GET: Answers
        public ActionResult Index()
        {
            var answer = db.Answer.Include(a => a.Question);
            return View(answer.ToList());
        }
        [HttpPost]
        public ActionResult CreateAnswerPartial([Bind(Include = "Id,Name")] ExamCoreViewModel model, int? questionID, int? index)
        {
            ViewBag.tmpQuestionID = questionID ?? 0;

            return PartialView(model);
        }


        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        public ActionResult AddAnswerCreate([Bind(Include = "Id,Name,Questions")] ExamCoreViewModel examViewModel, int? questionID)
        {
            ModelState.Clear();
            examViewModel.Questions[(int)questionID].Answers.Add(new AnswerViewModel() { QuestionID = examViewModel.Questions[(int)questionID].Id });
            return View("~/Views/ExamCores/Create.cshtml", examViewModel);
        }

        public ActionResult RemoveAnswerCreate([Bind(Include = "Id,Name,Questions")] ExamCoreViewModel examViewModel, int? questionID, int? answerID)
        {
            ModelState.Clear();
            if (questionID != null)
                examViewModel.Questions[(int)questionID].Answers.RemoveAt((int)answerID);
            return View("~/Views/ExamCores/Create.cshtml", examViewModel);
        }

        public ActionResult AddAnswerEdit([Bind(Include = "Id,Name,Questions")] ExamCoreViewModel examViewModel, int? questionID)
        {
            ModelState.Clear();
            examViewModel.Questions[(int)questionID].Answers.Add(new AnswerViewModel() { QuestionID = examViewModel.Questions[(int)questionID].Id });
            return View("~/Views/ExamCores/Edit.cshtml", examViewModel);
        }

        public ActionResult RemoveAnswerEdit([Bind(Include = "Id,Name,Questions")] ExamCoreViewModel examViewModel, int? questionID, int? answerID)
        {
            ModelState.Clear();
            if (questionID != null)
                examViewModel.Questions[(int)questionID].Answers.RemoveAt((int)answerID);
            return View("~/Views/ExamCores/Edit.cshtml", examViewModel);
        }
        // GET: Answers/Create
        public ActionResult Create()
        {
            ViewBag.QuestionID = new SelectList(db.Questions, "Id", "QuestionText");
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TextAnswer,QuestionID,IfCorrect")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Answer.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "Id", "QuestionText", answer.QuestionID);
            return View(answer);
        }

        // GET: Answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionID = new SelectList(db.Questions, "Id", "QuestionText", answer.QuestionID);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TextAnswer,QuestionID,IfCorrect")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionID = new SelectList(db.Questions, "Id", "QuestionText", answer.QuestionID);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answer.Find(id);
            db.Answer.Remove(answer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
