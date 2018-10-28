using Autofac;
using AutoMapper;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.QRCodeGenerator;
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
            //var iloscpytan = Mapper.Map<ExamDTO>(serviceE.GetByID(5)).QuestionsDTO.Where(x => x.AnswersDTO.Count == 0).ToList().Count();
            //foreach (var item in Mapper.Map<ExamDTO>(serviceE.GetByID(5)).QuestionsDTO)
            //{
            //    var tmpQ = new Question() { QuestionText = item.QuestionText };
            //    serviceE.AddQuestionToExam(serviceE.GetByID(5).Id, tmpQ);
            //}

            //var iloscpytan2 = Mapper.Map<ExamDTO>(serviceE.GetByID(5)).QuestionsDTO.Where(x => x.AnswersDTO.Count == 0).ToList().Count();
            //foreach (var item in Mapper.Map<ExamDTO>(serviceE.GetByID(5)).QuestionsDTO.Where(x => x.AnswersDTO.Count == 0).ToList())
            //{
            //    serviceQ.Delete(item.Id);
            //}
            //var iloscpytan3 = Mapper.Map<ExamDTO>(serviceE.GetByID(5)).QuestionsDTO.Where(x => x.AnswersDTO.Count == 0).ToList().Count();

           // DocumentCreator dcr = new DocumentCreator(Mapper.Map<ExamDTO>(serviceE.GetByID(5)));
            // serviceAP.InsertRange(Mapper.Map<List<AnswerPosition>>(dcr.AnswerPositionDTO));

            var costam = serviceAP.GetAllAnswersPositionsByExamID(5);


            //Console.Read();
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

            CreateMap<AnswerPosition, AnswerPositionDTO>();
            CreateMap<AnswerPositionDTO, AnswerPosition>();
        }
    }
}
