namespace ExamGenerator.Service.EF
{
    using ExamGeneratorModel;

    public interface IDataModelEF
    {
        ExamGeneratorDBContext CreateNew();
    }
}
