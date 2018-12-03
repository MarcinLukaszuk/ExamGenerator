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
            builder.RegisterType<ExamCoreService>().As<IExamCoreService>();
            builder.RegisterType<QuestionService>().As<IQuestionService>();
            builder.RegisterType<AnswerService>().As<IAnswerService>();
            builder.RegisterType<AnswerPositionService>().As<IAnswerPositionService>();
            builder.RegisterType<StudentService>().As<IStudentService>();
            builder.RegisterType<StudentGroupService>().As<IStudentGroupService>();
            builder.RegisterType<StudentGroupStudentService>().As<IStudentGroupStudentService>();
            builder.RegisterType<ExamCoreStudentGroupService>().As<IExamCoreStudentGroupService>();
            
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
            CreateMap<ExamCore, ExamDTO>()
                 .ForMember(destination => destination.QuestionsDTO, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamDTO, ExamCore>()
                .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.QuestionsDTO));
            CreateMap<ExamCoreViewModel, ExamCore>()
                    .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamCore, ExamCoreViewModel>()
                               .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.Questions));


            CreateMap<Question, QuestionDTO>()
             .ForMember(destination => destination.AnswersDTO,
            opts => opts.MapFrom(source => source.Answers));
            CreateMap<QuestionDTO, Question>();
            CreateMap<QuestionViewModel, Question>()
               .ForMember(destination => destination.Answers, opts => opts.MapFrom(source => source.Answers));
            CreateMap<Question, QuestionViewModel>()
                .ForMember(destination => destination.Answers, opts => opts.MapFrom(source => source.Answers));



            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();
            CreateMap<AnswerViewModel, Answer>();
            CreateMap<Answer, AnswerViewModel>();



            CreateMap<AnswerPosition, AnswerPositionDTO>()
                 .ForMember(destination => destination.AnswerDTO, opts => opts.MapFrom(source => source.Answer)); ;
            CreateMap<AnswerPositionDTO, AnswerPosition>();

        }
    }
}
