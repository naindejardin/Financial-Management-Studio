using FMStudio.Documents;
using FMStudio.SolutionTemplate.Properties;

namespace FMStudio.SolutionTemplate.Templates
{
    public class PersonalBillListTemplate : Template
    {
        public PersonalBillListTemplate()
            : base(
                Resources.PersonalBillListTemplateName,
                Resources.PersonalBillListTemplateDescription,
                Resources.PrivateCategoryName)
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

            //  Not fully support now.
            return document;
        }
    }
}
