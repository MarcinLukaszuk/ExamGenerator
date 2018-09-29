namespace ExamGenerator.DocumentManager.DocumentGenerator
{
    using Xceed.Words.NET;

    public class WordDocument
    {
        DocX _document;

        public DocX Document
        {
            get
            {
                return _document;
            }
        }

        public WordDocument(string fileName)
        {
            _document = DocX.Create(fileName);
        } 




    }
}
