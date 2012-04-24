using System;
using System.Xml.Linq;
using FMStudio.Documents;
using FMStudio.SolutionTemplate.Templates;

namespace FMStudio.SolutionTemplate.Test.Templates
{
    public class MockTemplate : Template
    {
        public MockTemplate(string name, string description, string category)
            : base(name, description, category)
        {
        }


        public bool ThrowException { get; set; }

        public void Clear()
        {
        }

        public override bool CanNewSolution() { return true; }


        protected override SolutionDocument NewCore(string fullFilePath)
        {
            CheckThrowException();

            XDocument xdoc = new XDocument(
                new XComment("Financial Management Studio Solution File, Format Version 1.00"),
                new XComment("FM Studio 1.0.0"),
                new XElement("Solution",
                    new XElement("SolutionProperty",
                        new XAttribute("AliasName", "TestSolution")
                    )
                )
            );

            return new SolutionDocument(new SolutionDocumentType(), xdoc);
        }


        private void CheckThrowException()
        {
            if (ThrowException) { throw new InvalidOperationException("ThrowException has been activated on the MockTemplate."); }
        }
    }
}
