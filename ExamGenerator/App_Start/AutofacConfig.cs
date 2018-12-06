using Autofac;
using Autofac.Integration.Mvc;
using ExamGenerator.Service.Interfaces;
using ExamGenerator.Service.Services;
using ExamGeneratorModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ExamGenerator
{
    public class AutofacConfig
    {
        public static void RegisterAutofacConfig()
        {
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
            builder.RegisterType<GeneratedExamService>().As<IGeneratedExamService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}