using FMStudio.Documents;
using FMStudio.SolutionTemplate.Properties;

namespace FMStudio.SolutionTemplate.Templates
{
    public class EmptyTemplate : Template
    {
        public EmptyTemplate()
            : base(
                Resources.EmptyTemplateName, 
                Resources.EmptyTemplateDescription,
                Resources.CommonCategoryName)
        {
        }


        public override bool CanNewSolution() { return true; }


        protected override SolutionDocument NewCore(string fullFilePath)
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            if (fullFilePath
                .Substring(fullFilePath.Length - documentType.FileExtension.Length)
                .CompareTo(documentType.FileExtension) != 0)
            {
                fullFilePath = fullFilePath + documentType.FileExtension;
            }

            SolutionDocument document = documentType.New(fullFilePath) as SolutionDocument;
            return document;
        }
    }
}
