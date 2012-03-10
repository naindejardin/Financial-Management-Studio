using System.Xml.Linq;
using BillList.Applications.Documents;
using System;

namespace FMStudio.Application.Documents
{
    public class SolutionDocument : Document
    {
        private readonly XDocument solutionXML;


        public SolutionDocument(SolutionDocumentType documentType, XDocument solutionXML)
            : base(documentType)
        {
            this.solutionXML = solutionXML;
        }


        public XDocument SolutionXML { get { return this.solutionXML; } }


        public void AddDocument(IDocument document)
        {
            throw new NotSupportedException();
        }

        public void RemoveDocument(IDocument document)
        {
            throw new NotSupportedException();
        }

        public void ModifyAliasName(string fullFilePath, string newAliasName)
        {
            throw new NotSupportedException();
        }
    }
}
