using System.IO;
using System.Xml.Linq;
using BillList.Applications.Documents;
using FMStudio.Application.Properties;

namespace FMStudio.Application.Documents
{
    public class SolutionDocumentType:DocumentType
    {
        public SolutionDocumentType()
            : base(Resources.SolutionDocuments, ".fmsln")
        {
        }


        public override bool CanNew() { return true; }

        public override bool CanOpen() { return true; }

        public override bool CanSave(IDocument document) { return true; }

        protected override IDocument NewCore(string fullFilePath)
        {
            XDocument xdoc = new XDocument(
                new XComment("Financial Management Studio Solution File, Format Version 1.00"),
                new XComment("FM Studio 1.0.0"),
                new XElement("Solution",
                    new XElement("SolutionProperty",
                        new XAttribute("AliasName", Path.GetFileNameWithoutExtension(fullFilePath))
                    )
                )
            );
            if (!Directory.Exists(Path.GetDirectoryName(fullFilePath)))
            {
                string directory = Path.GetDirectoryName(fullFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));
            }

            xdoc.Save(fullFilePath);

            return OpenCore(fullFilePath);
        }

        protected override IDocument OpenCore(string fullFilePath)
        {
            XDocument solutionXML = XDocument.Load(fullFilePath);

            SolutionDocument document = new SolutionDocument(this, solutionXML);
            return document;
        }

        protected override void SaveCore(IDocument document, string fullFilePath)
        {
            XDocument solutionXML = ((SolutionDocument)document).SolutionXML;

            solutionXML.Save(fullFilePath);
        }
    }
}
