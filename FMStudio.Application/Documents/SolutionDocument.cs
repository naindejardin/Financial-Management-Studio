using System;
using System.Xml.Linq;
using BillList.Applications.Documents;

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


        public void ModifyAliasName(string fullFilePath, string newAliasName)
        {
            throw new NotSupportedException();
        }
    }
}
