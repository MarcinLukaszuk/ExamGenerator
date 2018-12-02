using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AutoMapper;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.PDFCreator;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;

namespace ExamGenerator.Controllers
{
    public class ExamCoresController : Controller
    {
        private IExamCoreService _examService;
        private IAnswerService _answerService;
        private IQuestionService _questionService;
        private IAnswerPositionService _answerPositionService;

        public ExamCoresController() { }
        public ExamCoresController(IExamCoreService examService, IAnswerService answerService, IQuestionService questionService, IAnswerPositionService answerPositionService)
        {
            _examService = examService;
            _answerService = answerService;
            _questionService = questionService;
            _answerPositionService = answerPositionService;
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
            ExamCore exam = _examService.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ExamCoreViewModel examVM = Mapper.Map<ExamCoreViewModel>(exam);
            return View(examVM);
        }

        // GET: Exams/Create
        public ActionResult Create()
        {
            var tmp = new ExamCoreViewModel();
            return View(tmp);
        }



        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Questions")] ExamCoreViewModel examViewModel)
        {
            ExamCore tmpExam;
            Question tmpQuestion;
            Answer tmpAnswer;

            if (ModelState.IsValid)
            {
                tmpExam = Mapper.Map<ExamCore>(examViewModel);
                _examService.Insert(tmpExam);
                //foreach (var question in examViewModel.Questions)
                //{
                //    tmpQuestion = Mapper.Map<Question>(question);
                //    _examService.AddQuestionToExam(tmpExam, tmpQuestion);
                //    foreach (var answer in question.Answers)
                //    {
                //        tmpAnswer = Mapper.Map<Answer>(answer); ;
                //        _questionService.AddAnswerToQuestion(tmpQuestion, tmpAnswer);
                //    }
                //}
                return RedirectToAction("Index");
            }

            return View(examViewModel);
        }
        // GET: Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamCore exam = _examService.Find(id);
            var examViewModel = Mapper.Map<ExamCoreViewModel>(exam);
            if (examViewModel == null)
            {
                return HttpNotFound();
            }
            return View(examViewModel);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Questions")] ExamCoreViewModel examViewModel)
        {
            ExamCore editedExam = Mapper.Map<ExamCore>(examViewModel);
            if (ModelState.IsValid)
            {
                _examService.Update(editedExam);
                return RedirectToAction("Index");
            }
            return View(examViewModel);
        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamCore exam = _examService.Find(id);
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

        public JsonResult GetProducts(int id)
        { 
            ExamCore exam = _examService.Find(id);
            if (exam==null)
            {
                return null;
            }
            var path = HostingEnvironment.MapPath("~/GeneratedExams");
            ExamDTO examDTO = Mapper.Map<ExamDTO>(exam);
            DocumentCreator creator = new DocumentCreator(examDTO, path);
            var answerPositions = creator.AnswerPositionDTO;
           // serviceAP.InsertRange(6, Mapper.Map<List<AnswerPosition>>(pozycje));



            return Json("aaaa", JsonRequestBehavior.AllowGet);
        }
        public FileResult GetPdfExam(int id)
        {
            ExamCore exam = _examService.Find(id);
            if (exam == null)
            {
                return null;
            }
            var path = HostingEnvironment.MapPath("~/GeneratedExams");
            ExamDTO examDTO = Mapper.Map<ExamDTO>(exam);
            DocumentCreator creator = new DocumentCreator(examDTO, path);
            var answerPositions = creator.AnswerPositionDTO;
           _answerPositionService.InsertRange(6, Mapper.Map<List<AnswerPosition>>(answerPositions));


            string fullPath = Path.Combine(path, creator.Filename);
            return File(fullPath, "application/pdf", creator.Filename); 
        }

    }
}
