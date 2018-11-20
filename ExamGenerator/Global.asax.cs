using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using ExamGenerator.Service.Interfaces;
using ExamGenerator.Service.Services;
using ExamGeneratorModel;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ExamGenerator
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //autofac
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();

            builder.RegisterType<ExamGeneratorDBContext>().As<IDbContext>();
            builder.RegisterType<ExamService>().As<IExamService>();
            builder.RegisterType<QuestionService>().As<IQuestionService>();
            builder.RegisterType<AnswerService>().As<IAnswerService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //automapper
            Mapper.Initialize(cfg => cfg.AddProfile<DTOProfile>());
        }
    }
    public class DTOProfile : Profile
    {
        public DTOProfile()
        {
            CreateMap<Exam, ExamDTO>()
                 .ForMember(destination => destination.QuestionsDTO, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamDTO, Exam>()
                .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.QuestionsDTO));
            CreateMap<ExamViewModel, Exam>()
                .ForMember(destination => destination.Questions, opts => opts.Ignore());


            CreateMap<Question, QuestionDTO>()
             .ForMember(
             destination => destination.AnswersDTO,
            opts => opts.MapFrom(source => source.Answers));
            CreateMap<QuestionDTO, Question>();
            CreateMap<QuestionViewModel, Question>()
                .ForMember(destination => destination.Answers, opts => opts.Ignore());



            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();
            CreateMap<AnswerViewModel, Answer>();




            CreateMap<AnswerPosition, AnswerPositionDTO>()
                 .ForMember(destination => destination.AnswerDTO, opts => opts.MapFrom(source => source.Answer)); ;
            CreateMap<AnswerPositionDTO, AnswerPosition>();
        }
    }
}
