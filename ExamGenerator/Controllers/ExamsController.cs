using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AutoMapper;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.PDFCreator;
using ExamGenerator.DocumentManager.UnZipArchive;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;
using Microsoft.AspNet.Identity;

namespace ExamGenerator.Controllers
{
    [Authorize]
    public class ExamCoresController : Controller
    {
        private IExamCoreService _examCoreService;
        private IAnswerService _answerService;
        private IQuestionService _questionService;
        private IAnswerPositionService _answerPositionService;
        private IStudentGroupService _studentGroupService;
        private IGeneratedExamService _generatedExamService;
        private IExamCoreStudentGroupService _examCoreStudentGroupService;
        private IResultService _resultService;
        public ExamCoresController() { }
        public ExamCoresController
            (
            IExamCoreService examCoreService,
            IAnswerService answerService,
            IQuestionService questionService,
            IAnswerPositionService answerPositionService,
            IStudentGroupService studentGroupService,
            IGeneratedExamService generatedExamService,
            IExamCoreStudentGroupService examCoreStudentGroupService,
            IResultService resultService
            )
        {
            _examCoreService = examCoreService;
            _answerService = answerService;
            _questionService = questionService;
            _answerPositionService = answerPositionService;
            _studentGroupService = studentGroupService;
            _generatedExamService = generatedExamService;
            _examCoreStudentGroupService = examCoreStudentGroupService;
            _resultService = resultService;
        }

        // GET: Exams
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            return View(_examCoreService.GetAll().Where(x => x.Owner == userID).ToList());
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

        public ActionResult Validate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Validate(HttpPostedFileBase FileUpload)
        {
            string path = HostingEnvironment.MapPath("~/UserBitmaps");
            string fullPath = path + "//" + FileUpload.FileName + PDFHelpers.GetMD5(new Random().Next().ToString()); ;
            FileUpload.SaveAs(fullPath);

            var bitmaps = ArchiveUnZiper.GetBitmapsFromZipArchive(fullPath);
            var validator = new DocumentValidator(bitmaps);
            var examIDs = validator.GetExamIDs();

            foreach (var examID in examIDs)
            {
                var egzaminAP = _answerPositionService.GetAllAnswersPositionsByExamID(examID);
                var studentID = _resultService.GetStudentIDByExamID(examID);
                var examResults = validator.CheckExam(examID, Mapper.Map<List<AnswerPositionDTO>>(egzaminAP));
                examResults.GeneratedExamID = examID;
                if (studentID != null)
                    examResults.StudentID = (int)studentID;
                _resultService.DeletePreviousResults(examID);
                _resultService.Insert(Mapper.Map<Result>(examResults));
            }
            if (examIDs.Count > 0)
            {
                _resultService.SetIsValidatetFlagByExamID(examIDs.FirstOrDefault());
                var examCoreStudentGroupID = _generatedExamService.GetByID(examIDs.FirstOrDefault()).ExamCoreStudentGroupID;
                return RedirectToAction("Index/", "Results", new { examCoreStudentGroupID = examCoreStudentGroupID.ToString()});
            }
            return View();
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

            if (ModelState.IsValid)
            {
                tmpExam = Mapper.Map<ExamCore>(examViewModel);
                tmpExam.Owner = User.Identity.GetUserId();
                _examCoreService.Insert(tmpExam);
                return RedirectToAction("Details", new { id = tmpExam.Id });
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
        public ActionResult Edit([Bind(Include = "Id,Name,Questions,Owner")] ExamCoreViewModel examViewModel)
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


        public FileResult GetPdfExam(int id)
        {
            ExamCore exam = _examCoreService.Find(id);
            if (exam == null)
            {
                return null;
            }
            var path = Request.MapPath("~/GeneratedExams");


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
                var path = Request.MapPath("~/GeneratedExams");

                if (!Directory.Exists(Request.MapPath("~/GeneratedExams")))
                {
                    Directory.CreateDirectory(path);
                }
                DocumentCreator creator = new DocumentCreator(path);
                foreach (var studentGroupStudentID in studentsGroupsStudentsIDList)
                {
                    var generatedExamDTO = GenerateExamForStudent(core, studentGroupStudentID, (int)questionNumber);
                    creator.AddExamToGenerate(generatedExamDTO);

                }
                creator.Generate();

                ArchiveUnZiper.PackFileToArchive(path + "//" + PDFHelpers.GetMD5(creator.PDFDocuments.FirstOrDefault()?.Filename) + ".zip", creator.PDFDocuments.Select(x => x.Filepath + x.Filename).ToList());

                foreach (var pdfDocument in creator.PDFDocuments)
                {
                    _answerPositionService.InsertRange(pdfDocument.ExamID, Mapper.Map<List<AnswerPosition>>(pdfDocument.ExamAnswerPositions));
                }
                _examCoreStudentGroupService.SetExamArchivePath((int)ExamCoreID, (int)studentGruopID, PDFHelpers.GetMD5(creator.PDFDocuments.FirstOrDefault()?.Filename) + ".zip");
            }
            return RedirectToAction("Details", "StudentGroups", new { id = (int)studentGruopID });
        }

        [HttpPost, ActionName("GenerateExam2")]
        public ActionResult GenerateExamPost2(int? ExamCoreStudentGroupID, int? questionNumber)
        {
            if (ExamCoreStudentGroupID != null && questionNumber != null)
            {
                var examCoreStudentGroup = _examCoreStudentGroupService.GetByID((int)ExamCoreStudentGroupID);
                var core = _examCoreService.Find(examCoreStudentGroup.ExamCoreID);
                var studentsGroupsStudentsIDList = _studentGroupService.GetStudentsGroupStudentID(examCoreStudentGroup.StudentGroupID);
                var random = new Random();
                var path = Request.MapPath("~/GeneratedExams");

                if (!Directory.Exists(Request.MapPath("~/GeneratedExams")))
                {
                    Directory.CreateDirectory(path);
                }
                DocumentCreator creator = new DocumentCreator(path);
                foreach (var studentGroupStudentID in studentsGroupsStudentsIDList)
                {
                    var generatedExamDTO = GenerateExamForStudent2(core, studentGroupStudentID, (int)questionNumber, (int)ExamCoreStudentGroupID);
                    creator.AddExamToGenerate(generatedExamDTO);

                }
                creator.Generate();

                ArchiveUnZiper.PackFileToArchive(path + "//" + PDFHelpers.GetMD5(creator.PDFDocuments.FirstOrDefault()?.Filename) + ".zip", creator.PDFDocuments.Select(x => x.Filepath + x.Filename).ToList());

                foreach (var pdfDocument in creator.PDFDocuments)
                {
                    _answerPositionService.InsertRange(pdfDocument.ExamID, Mapper.Map<List<AnswerPosition>>(pdfDocument.ExamAnswerPositions));
                }
                _examCoreStudentGroupService.SetExamArchivePath2(examCoreStudentGroup.Id, PDFHelpers.GetMD5(creator.PDFDocuments.FirstOrDefault()?.Filename) + ".zip");
                return RedirectToAction("Details", "StudentGroups", new { id = (int)examCoreStudentGroup.StudentGroupID });
            }
            return RedirectToAction("Index", "StudentGroups");

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
        private ExamDTO GenerateExamForStudent2(ExamCore examCore, int studentGroupStudentID, int questionNumber, int examCoreStudentGroup)
        {
            var generatedExam = new GeneratedExam()
            {
                ExamCoreID = examCore.Id,
                StudentGroupStudentID = studentGroupStudentID,
                ExamCoreStudentGroupID= examCoreStudentGroup
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

        [HttpPost]
        public ActionResult SaveAsyncQuestion(string examID, string questionID, AsyncEditedDataViewModel questionData, List<AsyncEditedDataViewModel> answersData)
        {
            if (int.TryParse(questionID, out int questionIntID))
            {
                var editedQuestion = _questionService.GetByID(questionIntID);
                if (questionData != null && questionData.oldValue != null && questionData.newValue != null)
                {
                    editedQuestion.QuestionText = questionData.newValue;
                }

                if (questionData != null && questionData.secondNewValue != null && questionData.secondNewValue == "true")
                {
                    _questionService.Delete(editedQuestion);
                    return Json(
                   new
                   {
                       success = true,
                       responseText = "Question has been successfuly deleted!",
                       deleted = true
                   }, JsonRequestBehavior.AllowGet);

                }
                var editedAnswers = new List<Answer>();
                var addedAnswers = new List<Answer>();
                if (answersData != null && answersData.Any())
                {
                    foreach (var answerData in answersData)
                    {
                        var editedAnswer = editedQuestion.Answers.Where(x => x.TextAnswer == answerData.oldValue).FirstOrDefault();

                        if (editedAnswer != null)
                        {
                            editedAnswer.TextAnswer = answerData.newValue;
                            editedAnswer.IfCorrect = answerData.secondNewValue == "true" ? true : false;
                            editedAnswers.Add(editedAnswer);
                        }
                        else
                        {
                            var addedAnswer = new Answer()
                            {
                                IfCorrect = answerData.secondNewValue == "true" ? true : false,
                                TextAnswer = answerData.newValue
                            };
                            _questionService.AddAnswerToQuestion(questionIntID, addedAnswer);
                            addedAnswers.Add(addedAnswer);
                        }
                    }

                    var allAnswers = _questionService.GetByID(questionIntID).Answers.ToList();
                    var deletedAnswers = allAnswers.Except(editedAnswers).ToList();
                    deletedAnswers = deletedAnswers.Except(addedAnswers).ToList();

                    foreach (var deletedAnswerr in deletedAnswers)
                    {
                        _answerService.Delete(deletedAnswerr.Id);
                    }
                    _questionService.Update(editedQuestion);
                }

                var returnedAnswers = editedAnswers.ToList();
                if (addedAnswers.Any())
                {
                    returnedAnswers = returnedAnswers.Concat(addedAnswers.ToList()).ToList();
                }
                return Json(
                    new
                    {
                        success = true,
                        responseText = "Questions have been successfuly updated!",
                        data = returnedAnswers.Select(x => new
                        {
                            TextAnswer = x.TextAnswer,
                            IfCorrect = x.IfCorrect
                        }).ToList(),
                        newQuestionID = editedQuestion.Id
                    }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                if (int.TryParse(examID, out int examIntID))
                {
                    var editedQuestion = new Question();
                    if (questionData != null && questionData.newValue != null)
                    {
                        editedQuestion.QuestionText = questionData.newValue;
                        _examCoreService.AddQuestionToExam(examIntID, editedQuestion);
                    }
                    if (answersData != null && answersData.Any())
                    {
                        foreach (var answerData in answersData)
                        {
                            var addedAnswer = new Answer()
                            {
                                IfCorrect = answerData.secondNewValue == "true" ? true : false,
                                TextAnswer = answerData.newValue
                            };
                            _questionService.AddAnswerToQuestion(editedQuestion.Id, addedAnswer);
                        }
                    }
                    return Json(
                        new
                        {
                            success = true,
                            responseText = "Questions have been successfuly updated!",
                            data = _questionService.GetByID(editedQuestion.Id).Answers.Select(x => new
                            {
                                TextAnswer = x.TextAnswer,
                                IfCorrect = x.IfCorrect
                            }).ToList(),
                            newQuestionID = editedQuestion.Id
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "Error during updating questions." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddQuestionsFromFileAsync(string examID, HttpPostedFileBase FileUpload)
        {
            try
            {
                if (int.TryParse(examID, out int examIntID) && FileUpload != null)
                {
                    var result = await addQuestionsToExamFromFile(examIntID, FileUpload);
                    if (result == 0)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                responseText = "Questions have been successfuly updated!"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error during updating questions." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Error during updating questions." }, JsonRequestBehavior.AllowGet);
        }

        private async Task<int> addQuestionsToExamFromFile(int examID, HttpPostedFileBase FileUpload)
        {
            foreach (var que in readQuestionsFile(FileUpload))
            {
                var questionn = Mapper.Map<Question>(que);
                _examCoreService.AddQuestionToExam(examID, questionn);
            }
            return 0;
        }

        [HttpPost]
        public ActionResult CheckUploadExams(HttpPostedFileBase FileUpload)
        {
            string path = Request.MapPath("~/UserBitmaps");
            string fullPath = path + "//" + FileUpload.FileName + PDFHelpers.GetMD5(new Random().Next().ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileUpload.SaveAs(fullPath);

            var bitmaps = ArchiveUnZiper.GetBitmapsFromZipArchive(fullPath);
            var validator = new DocumentValidator(bitmaps);
            var examIDs = validator.GetExamIDs();

            foreach (var item in examIDs)
            {
                var egzaminAP = _answerPositionService.GetAllAnswersPositionsByExamID(item);
                var examResults = validator.CheckExam(item, Mapper.Map<List<AnswerPositionDTO>>(egzaminAP));


            }
            return RedirectToAction("Details", "StudentGroups", new { id = 0 });
        }
        private List<QuestionDTO> readQuestionsFile(HttpPostedFileBase FileUpload)
        {
            LinkedList<QuestionDTO> questions = new LinkedList<QuestionDTO>();
            using (StreamReader reader = new StreamReader(FileUpload.InputStream, Encoding.Default, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var lineArray = line.Split(';');
                    //question
                    if (lineArray[0] != "")
                    {
                        questions.AddLast(new QuestionDTO() { QuestionText = lineArray[0] });
                    }
                    //answer
                    if (lineArray[0] == "")
                    {
                        var lastQuestion = questions.Last();

                        lastQuestion.AnswersDTO.Add(new AnswerDTO() { TextAnswer = lineArray[1], IfCorrect = lineArray[2] == "1" });
                    }
                }
            }

            return questions.ToList();
        }



    }
}
