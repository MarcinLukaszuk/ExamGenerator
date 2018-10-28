namespace ExamGenerator.Service.EF
{
    using ExamGeneratorModel;

    public class DataModelEF : IDataModelEF
    {
        ExamGeneratorDBContext dbContext;
        public DataModelEF GetNewInstance()
        {
            return new DataModelEF();
        }
        public ExamGeneratorDBContext GetContext()
        {
            if (dbContext == null)
                dbContext = new ExamGeneratorDBContext();
            return dbContext;
        }
    }
}
