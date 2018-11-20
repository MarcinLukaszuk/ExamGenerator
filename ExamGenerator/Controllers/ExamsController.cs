using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;

namespace ExamGenerator.Controllers
{
    public class ExamsController : Controller
    {
        private IExamService _examService;
        private IAnswerService _answerService;
        private IQuestionService _questionService;

        public ExamsController() { }
        public ExamsController(IExamService examService, IAnswerService answerService, IQuestionService questionService)
        {
            _examService = examService;
            _answerService = answerService;
            _questionService = questionService;
        }

        // GET: Exams
        public ActionResult Index()
        {
            return View(_examService.GetAll().ToList());
        }

        // GET: Exams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = _examService.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // GET: Exams/Create
        public ActionResult Create()
        {
            ExamViewModel model = new ExamViewModel();
            model.Questions = new List<QuestionViewModel>();
            return View(model);
        }


        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Questions")] ExamViewModel examViewModel)
        {
            Exam tmpExam;
            Question tmpQuestion;
            Answer tmpAnswer;
            if (ModelState.IsValid)
            {
                tmpExam = Mapper.Map<Exam>(examViewModel);
                _examService.Insert(tmpExam);
                foreach (var question in examViewModel.Questions)
                {
                    tmpQuestion = Mapper.Map<Question>(question);
                    _examService.AddQuestionToExam(tmpExam, tmpQuestion);
                    foreach (var answer in question.Answers)
                    {
                        tmpAnswer= Mapper.Map<Answer>(answer); ;
                        _questionService.AddAnswerToQuestion(tmpQuestion,tmpAnswer);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(examViewModel);
        }

        [HttpPost]
        public ActionResult TMP([Bind(Include = "Id,Name,Questions")] ExamViewModel examViewModel)
        {
            Exam tmpExam;
            Question tmpQuestion;
            Answer tmpAnswer;
            if (ModelState.IsValid)
            {
                 
               
            }

            return RedirectToAction("Index");
        }

        // GET: Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = _examService.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _examService.Update(exam);
                return RedirectToAction("Index");
            }
            return View(exam);
        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = _examService.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _examService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
