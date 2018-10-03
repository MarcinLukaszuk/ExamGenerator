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


            AnswerService serviceA = new AnswerService(new DataModelEF());
            QuestionService serviceQ = new QuestionService(new DataModelEF());
            ExamService serviceE = new ExamService(new DataModelEF());


            var exammm = Mapper.Map<ExamDTO>( serviceE.GetByID(3));
            DocumentCreator dcr = new DocumentCreator(exammm);
            
            Console.WriteLine("koniec");
            //  Console.Read();
        }
    }

    public class DTOProfile : Profile
    {
        public DTOProfile()
        {
            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();

            CreateMap<Question, QuestionDTO>();
            CreateMap<QuestionDTO, Question>();

            CreateMap<Exam, ExamDTO>();
            CreateMap<ExamDTO, Exam>();
        }
    }
}
