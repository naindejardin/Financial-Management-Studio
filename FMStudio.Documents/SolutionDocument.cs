using System;
using System.Xml.Linq;
using FMStudio.Documents;

namespace FMStudio.Documents
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


        public string AliasName
        {
            get 
            {
                return solutionXML.Element("Solution").Element("SolutionProperty").Attribute("AliasName").Value;
            }
            set
            {
                string aliasName = solutionXML.Element("Solution").Element("SolutionProperty").Attribute("AliasName").Value;

                if (aliasName != value)
                {
                    solutionXML.Element("Solution").Element("SolutionProperty").Attribute("AliasName").Value = value;
                    this.Modified = true;
                    RaisePropertyChanged("AliasName");
                }
            }
        }


        private XDocument GenerateSolutionXML()
        {
            XDocument xdoc = new XDocument(
                new XComment("Financial Management Studio Solution File, Format Version 1.00"),
                new XComment("FM Studio 1.0.0"),
                new XElement("Solution",
                    new XElement("SolutionProperty",
                        new XAttribute("AliasName", AliasName)
                    )
                )
            );
            return xdoc;

        }
    }
}
