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
using ExamGeneratorModel.ViewModel;

namespace ExamGenerator.Controllers
{
    public class QuestionsController : Controller
    {
        private IExamService _examService;
        private IAnswerService _answerService;
        private IQuestionService _questionService;

        public QuestionsController() { }
        public QuestionsController(IExamService examService, IAnswerService answerService, IQuestionService questionService)
        {
            _examService = examService;
            _answerService = answerService;
            _questionService = questionService;
        } 
        // GET: Questions
        public ActionResult Index()
        {
            return View(_questionService.GetAll());
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = _questionService.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }
        
        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.ExamID = new SelectList(_examService.GetAll(), "Id", "Name");
            return View();
        }


        public ActionResult AddQuestionCreate([Bind(Include = "Id,Name,Questions")] ExamViewModel examViewModel)
        {
            ModelState.Clear();
            examViewModel.Questions.Add(new QuestionViewModel() { ExamID= examViewModel .Id});
            return View("~/Views/Exams/Create.cshtml", examViewModel);
        }

        public ActionResult RemoveQuestionCreate([Bind(Include = "Id,Name,Questions")] ExamViewModel examViewModel, int? questionID)
        {
            ModelState.Clear();
            if (questionID != null)
                examViewModel.Questions.RemoveAt((int)questionID);
            return View("~/Views/Exams/Create.cshtml", examViewModel);
        }

        public ActionResult AddQuestionEdit([Bind(Include = "Id,Name,Questions")] ExamViewModel examViewModel)
        {
            ModelState.Clear();
            examViewModel.Questions.Add(new QuestionViewModel() { ExamID = examViewModel.Id });
            return View("~/Views/Exams/Edit.cshtml", examViewModel);
        }
        public ActionResult RemoveQuestionEdit([Bind(Include = "Id,Name,Questions")] ExamViewModel examViewModel, int? questionID)
        {
            ModelState.Clear();
            if (questionID != null)
                examViewModel.Questions.RemoveAt((int)questionID);
            return View("~/Views/Exams/Edit.cshtml", examViewModel);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ExamID,QuestionText")] Question question)
        {
            if (ModelState.IsValid)
            {
               _questionService.Insert(question);
               
                return RedirectToAction("Index");
            }

            ViewBag.ExamID = new SelectList(_examService.GetAll(), "Id", "Name", question.ExamID);
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = _questionService.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExamID = new SelectList(_examService.GetAll(), "Id", "Name", question.ExamID);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ExamID,QuestionText")] Question question)
        {
            if (ModelState.IsValid)
            {
                _questionService.Update(question); 
                return RedirectToAction("Index");
            }
            ViewBag.ExamID = new SelectList(_examService.GetAll(), "Id", "Name", question.ExamID);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = _questionService.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = _questionService.Find(id);
            _questionService.Delete(question); 
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        { 
            base.Dispose(disposing);
        }
    }
}
