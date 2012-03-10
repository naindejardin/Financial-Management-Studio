using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMStudio.Application.Documents;
using System.Xml.Linq;
using System.IO;
using BillList.Applications.Documents;
using BigEgg.Framework.UnitTesting;

namespace FMStudio.Application.Test.Documents
{
    [TestClass]
    public class SolutionDocumentTypeTest
    {
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution.sln")))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution.sln"));
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2.sln")))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution2.sln"));
        }


        [TestMethod]
        public void DocumentTypeTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            Assert.AreEqual(".sln", documentType.FileExtension);
            Assert.AreEqual("FMStuido Solution Documents (*.sln)", documentType.Description);
        }

        [TestMethod]
        public void NewDocumentTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            Assert.IsTrue(documentType.CanNew());
            SolutionDocument document =
                documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution")) as SolutionDocument;
            Assert.IsTrue(File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution.sln")));
            Assert.IsNotNull(document);
            Assert.AreEqual("TestSolution", document.AliasName);
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
            IDocument document = documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution"));
            Assert.AreEqual("TestSolution", document.AliasName);

            Assert.IsTrue(documentType.CanSave(document));
            documentType.Save(document, Path.Combine(Environment.CurrentDirectory, "TestSolution2"));
            Assert.AreEqual("TestSolution", document.AliasName);
            Assert.IsTrue(File.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2.sln")));

            Assert.IsTrue(documentType.CanOpen());
            IDocument openedDocument = documentType.Open("TestSolution.sln", "Alias");
            Assert.AreEqual("Alias", openedDocument.AliasName);

            // Note: What's missing is to compare the document content of both documents.
        }

        [TestMethod]
        public void DocumentTest()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            IDocument document = documentType.New(Path.Combine(Environment.CurrentDirectory, "TestSolution"));

            Assert.AreEqual(document.DocumentType, documentType);

            Assert.AreEqual("TestSolution", document.AliasName);
            AssertHelper.PropertyChangedEvent(document, x => x.AliasName, () => document.AliasName = "TestSolution2");
            Assert.AreEqual("TestSolution2", document.AliasName);

            Assert.IsFalse(document.Modified);
            AssertHelper.PropertyChangedEvent(document, x => x.Modified, () => document.Modified = true);
            Assert.IsTrue(document.Modified);
        }
    }
}
