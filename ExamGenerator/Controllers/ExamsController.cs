using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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
        private IExamCoreService _examCoreService;
        private IAnswerService _answerService;
        private IQuestionService _questionService;
        private IAnswerPositionService _answerPositionService;
        private IStudentGroupService _studentGroupService;
        private IGeneratedExamService _generatedExamService;
        public ExamCoresController() { }
        public ExamCoresController
            (
            IExamCoreService examCoreService,
            IAnswerService answerService,
            IQuestionService questionService,
            IAnswerPositionService answerPositionService,
            IStudentGroupService studentGroupService,
            IGeneratedExamService generatedExamService
            )
        {
            _examCoreService = examCoreService;
            _answerService = answerService;
            _questionService = questionService;
            _answerPositionService = answerPositionService;
            _studentGroupService = studentGroupService;
            _generatedExamService = generatedExamService;
        }

        // GET: Exams
        public ActionResult Index()
        {
            return View(_examCoreService.GetAll().ToList());
        }

        // GET: Exams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamCore exam = _examCoreService.Find(id);
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
                _examCoreService.Insert(tmpExam);
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
            ExamCore exam = _examCoreService.Find(id);
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
                _examCoreService.Update(editedExam);
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
            ExamCore exam = _examCoreService.Find(id);
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
            _examCoreService.Delete(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetProducts(int id)
        {
            ExamCore exam = _examCoreService.Find(id);
            if (exam == null)
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
            ExamCore exam = _examCoreService.Find(id);
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

        [HttpPost, ActionName("GenerateExam")]
        public ActionResult GenerateExamPost(int? ExamCoreID, int? studentGruopID, int? questionNumber)
        {
            if (ExamCoreID != null && studentGruopID != null && questionNumber != null)
            {
                var core = _examCoreService.Find(ExamCoreID);
                var studentsGroupsStudentsIDList = _studentGroupService.GetStudentsGroupStudentID(studentGruopID);
                var random = new Random();
                var path = HostingEnvironment.MapPath("~/GeneratedExams");

                DocumentCreator creator = new DocumentCreator(path);
                foreach (var studentGroupStudentID in studentsGroupsStudentsIDList)
                {
                    var generatedExamDTO = GenerateExamForStudent(core, studentGroupStudentID, (int)questionNumber);
                    creator.AddExamToGenerate(generatedExamDTO);

                }
                creator.Generate();
                foreach (var pdfDocument in creator.PDFDocuments)
                {
                    _answerPositionService.InsertRange(pdfDocument.ExamID, Mapper.Map<List<AnswerPosition>>(pdfDocument.ExamAnswerPositions));
                }
            }

            return RedirectToAction("Details", "StudentGroups", new { id = (int)studentGruopID });
        }

        private ExamDTO GenerateExamForStudent(ExamCore examCore, int studentGroupStudentID, int questionNumber)
        {
            var generatedExam = new GeneratedExam()
            {
                ExamCoreID = examCore.Id,
                StudentGroupStudentID = studentGroupStudentID
            };
            _generatedExamService.Insert(generatedExam);
            _generatedExamService
                .AssociateQuestionsToGeneratedExam(generatedExam, examCore.Questions.OrderBy(a => Guid.NewGuid())
                .Take(questionNumber)
                .ToList());
            return getExamDTO(generatedExam.Id);
        }
        private ExamDTO getExamDTO(int generatedExamID)
        {

            var generatedExam = _generatedExamService.Find(generatedExamID);
            var examCore = _examCoreService.Find(generatedExam?.ExamCoreID);
            var student = _generatedExamService.GetStudentByGeneratedExamID(generatedExam?.StudentGroupStudentID);

            var questions = _generatedExamService.GetQuestionsByGeneratedExamID(generatedExamID);
            ExamDTO examDTO = new ExamDTO()
            {
                Id = generatedExam.Id,
                Name = examCore.Name,
                StudentFullName = student.SurName + " " + student.Name,
                QuestionsDTO = Mapper.Map<List<QuestionDTO>>(questions)
            };
            return examDTO;
        }
    }
}
