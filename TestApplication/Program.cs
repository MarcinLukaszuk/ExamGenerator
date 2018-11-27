using Autofac;
using AutoMapper;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.QRCodeGenerator;
using ExamGenerator.DocumentManager.UnZipArchive;
using ExamGenerator.Service.Interfaces;
using ExamGenerator.Service.Services;
using ExamGeneratorModel;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<DTOProfile>());
            ExamGeneratorDBContext cont = new ExamGeneratorDBContext();
            AnswerService serviceA = new AnswerService(cont);
            QuestionService serviceQ = new QuestionService(cont);
            ExamService serviceE = new ExamService(cont, serviceA, serviceQ);
            AnswerPositionService serviceAP = new AnswerPositionService(cont);

            var bitmaps = ArchiveUnZiper.GetBitmapsFromZipArchive("skanTestu.zip");
            var validator = new DocumentValidator(bitmaps);
            var examIDs = validator.GetExamIDs();
            var egzaminAP = serviceAP.GetAllAnswersPositionsByExamID(examIDs.FirstOrDefault());
            validator.CheckExam(examIDs.First(), Mapper.Map<List<AnswerPositionDTO>>(egzaminAP));
            Console.Read();
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
