using Autofac;
using AutoMapper;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.QRCodeGenerator;
using ExamGenerator.DocumentManager.UnZipArchive;
using ExamGenerator.Service.EF;
using ExamGenerator.Service.Interfaces;
using ExamGenerator.Service.Services;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<DTOProfile>());
            //var builder = new ContainerBuilder();
            //builder.RegisterType<DataModelEF>().As<IDataModelEF>();
            //builder.RegisterType<ExamService>().As<IExamService>();
            //builder.RegisterType<QuestionService>().As<IQuestionService>();
            //builder.RegisterType<AnswerService>().As<IAnswerService>();
            //builder.Build();

            var data = new DataModelEF();
            var dataNew = data.GetNewInstance();
            AnswerService serviceA = new AnswerService(dataNew);
            QuestionService serviceQ = new QuestionService(dataNew);
            ExamService serviceE = new ExamService(dataNew);
            AnswerPositionService serviceAP = new AnswerPositionService(dataNew);

            //Exam exam = new Exam() { Name = "Test Przykładowy" };
            //Question q1 = new Question() { QuestionText = "Pytanie1" };
            //Question q2 = new Question() { QuestionText = "Pytanie2" };

            //Answer a1 = new Answer() { TextAnswer = "Tak", IfCorrect = true };
            //Answer a2 = new Answer() { TextAnswer = "Nie", IfCorrect = false };
            //Answer a3 = new Answer() { TextAnswer = "Tak", IfCorrect = true };
            //Answer a4 = new Answer() { TextAnswer = "Nie", IfCorrect = false };

            //serviceE.Insert(exam);

            //serviceE.AddQuestionToExam(exam,q1);
            //serviceE.AddQuestionToExam(exam,q2);

            //serviceQ.AddAnswerToQuestion(q1, a1);
            //serviceQ.AddAnswerToQuestion(q1, a2);
            //serviceQ.AddAnswerToQuestion(q2, a3);
            //serviceQ.AddAnswerToQuestion(q2, a4);


            //DocumentCreator creator = new DocumentCreator(Mapper.Map<ExamDTO>(serviceE.GetByID(6)));
            //var pozycje = creator.AnswerPositionDTO;
            // serviceAP.InsertRange(6, Mapper.Map<List<AnswerPosition>>(pozycje));

            //var examIDs = valid.GetExamIDs();

            //foreach (var examID in examIDs)
            //{
            //    var tmp = Mapper.Map<List<AnswerPositionDTO>>(serviceAP.GetAllAnswersPositionsByExamID(examID));
            //    valid.validateExam(examID, tmp);
            //}

            var bitmaps = ArchiveUnZiper.GetBitmapsFromZipArchive("E4DA3B7FBBCE2345D7772B0674A318D5.zip");
            var validator = new DocumentValidator(bitmaps);
            var examIDs = validator.GetExamIDs();
            var egzaminAP = serviceAP.GetAllAnswersPositionsByExamID(examIDs.FirstOrDefault());
            validator.CheckExam(examIDs.First(), Mapper.Map<List<AnswerPositionDTO>>(egzaminAP));
            
        }
    }

    public class DTOProfile : Profile
    {
        public DTOProfile()
        {
            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();

            CreateMap<Question, QuestionDTO>()
                .ForMember(
                destination => destination.AnswersDTO,
               opts => opts.MapFrom(source => source.Answers));
            CreateMap<QuestionDTO, Question>();

            CreateMap<Exam, ExamDTO>()
                .ForMember(destination => destination.QuestionsDTO, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamDTO, Exam>()
                .ForMember(destination => destination.Questions, opts => opts.Ignore());

            CreateMap<AnswerPosition, AnswerPositionDTO>()
                 .ForMember(destination => destination.AnswerDTO, opts => opts.MapFrom(source => source.Answer)); ;
            CreateMap<AnswerPositionDTO, AnswerPosition>();
        }
    }
}
