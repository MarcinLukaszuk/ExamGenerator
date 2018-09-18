namespace ExamGenerator.Service.EF
{
    using ExamGeneratorModel;

    public class DataModelEF : IDataModelEF
    {
        public ExamGeneratorDBContext CreateNew()
        {
            return new ExamGeneratorDBContext();
        }
    }
}
