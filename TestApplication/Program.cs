using Autofac;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.QRCodeGenerator;
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
            //var builder = new ContainerBuilder();

            //builder.RegisterType<DataModelEF>().As<IDataModelEF>();
            //builder.RegisterType<ExamService>().As<IExamService>();
            //builder.RegisterType<QuestionService>().As<IQuestionService>(); 
            //builder.Build();

            QrCodeED qrcodeGenerator = new QrCodeED();
           var zapisanyQRcode= qrcodeGenerator.Encode("Modzel chuj");
            DocumentCreator dcr = new DocumentCreator();



            Console.WriteLine("koniec");
          //  Console.Read();
        }
    }
}
