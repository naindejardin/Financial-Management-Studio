using System;
using System.IO;
using System.Xml.Linq;
using BigEgg.Framework.UnitTesting;
using FMStudio.Applications.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test.Documents
{
    [TestClass]
    public class SolutionDocumentTypeTest
    {
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution.fmsln")))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution.fmsln"));
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2.fmsln")))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution2.fmsln"));
        }


        [TestMethod]
        public void DocumentTypeTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            Assert.AreEqual(".fmsln", documentType.FileExtension);
            Assert.AreEqual("FMStuido Solution Documents (*.fmsln)", documentType.Description);
        }

        [TestMethod]
        public void NewDocumentTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            Assert.IsTrue(documentType.CanNew());
            SolutionDocument document =
                documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution")) as SolutionDocument;
            Assert.IsTrue(File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution.fmsln")));
            Assert.IsNotNull(document);
            XDocument xdoc = new XDocument(
                new XComment("Financial Management Studio Solution File, Format Version 1.00"),
                new XComment("FM Studio 1.0.0"),
                new XElement("Solution",
                    new XElement("SolutionProperty",
                        new XAttribute("AliasName", "TestSolution")
                    )
                )
            );
            Assert.AreEqual(xdoc.ToString(), document.SolutionXML.ToString());
        }

        [TestMethod]
        public void SaveAndOpenDocumentTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            SolutionDocument document = documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution")) as SolutionDocument;

            Assert.IsTrue(documentType.CanSave(document));
            documentType.Save(document, Path.Combine(Environment.CurrentDirectory, "TestSolution2"));
            Assert.IsTrue(File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2.fmsln")));

            Assert.IsTrue(documentType.CanOpen());
            SolutionDocument openedDocument = documentType.Open("TestSolution2.fmsln") as SolutionDocument;

            Assert.AreEqual(document.AliasName, openedDocument.AliasName);

            //  Modify the alias name
            document.AliasName = "NewAlias";
            openedDocument = null;
            Assert.AreEqual("NewAlias", document.AliasName);
            documentType.Save(document, Path.Combine(Environment.CurrentDirectory, "TestSolution2"));
            openedDocument = documentType.Open("TestSolution2.fmsln") as SolutionDocument;
            Assert.AreEqual("NewAlias", openedDocument.AliasName);
        }

        [TestMethod]
        public void DocumentTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            IDocument document = documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution"));

            Assert.AreEqual(document.DocumentType, documentType);

            Assert.IsFalse(document.Modified);
            AssertHelper.PropertyChangedEvent(document, x => x.Modified, () => document.Modified = true);
            Assert.IsTrue(document.Modified);
        }

        [TestMethod]
        public void AliasModifyTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            Assert.IsTrue(documentType.CanNew());
            SolutionDocument document =
                documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution")) as SolutionDocument;
            document.AliasName = "NewAlias";
            
            XDocument xdoc = new XDocument(
                new XComment("Financial Management Studio Solution File, Format Version 1.00"),
                new XComment("FM Studio 1.0.0"),
                new XElement("Solution",
                    new XElement("SolutionProperty",
                        new XAttribute("AliasName", "NewAlias")
                    )
                )
            );
            Assert.AreEqual(xdoc.ToString(), document.SolutionXML.ToString());
        }
    }
}
