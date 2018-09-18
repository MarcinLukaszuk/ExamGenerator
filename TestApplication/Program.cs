using Autofac;
using ExamGenerator.Service.EF;
using ExamGenerator.Service.Interfaces;
using ExamGenerator.Service.Services;
using ExamGeneratorModel.Model;
using System;


namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DataModelEF>().As<IDataModelEF>();
            builder.RegisterType<ExamService>().As<IExamService>();
            builder.RegisterType<QuestionService>().As<IQuestionService>(); 
            builder.Build();

            ExamService examService = new ExamService(new DataModelEF());
            QuestionService questionService = new QuestionService(new DataModelEF());
            var exam = examService.GetByID(7);
     
            foreach (var item in exam.Questions)
            {
                Console.WriteLine(item.QuestionText);
                foreach (var answer in item.Answers)
                {
                    Console.WriteLine(answer.TextAnswer);
                } 
                Console.WriteLine(); 
            }

            Console.WriteLine("koniec");
            Console.Read();
        }
    }
}
