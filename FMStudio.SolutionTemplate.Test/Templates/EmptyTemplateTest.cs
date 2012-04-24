using System;
using System.IO;
using FMStudio.Documents;
using FMStudio.SolutionTemplate.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.SolutionTemplate.Test.Templates
{
    [TestClass]
    public class EmptyTemplateTest
    {
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "NewSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "NewSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution2"), true);
        }


        [TestMethod]
        public void TemplateTest()
        {
            EmptyTemplate template = new EmptyTemplate();
            Assert.AreEqual("Empty Template", template.Name);
            Assert.AreEqual("An empty solution.", template.Description);
            Assert.AreEqual("Common", template.Category);
        }

        [TestMethod]
        public void NewDocumentTest()
        {
            EmptyTemplate template = new EmptyTemplate();
            Assert.IsTrue(template.CanNewSolution());
            SolutionDocument document = template.NewSolution(Path.Combine(
                Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"));

            Assert.IsTrue(File.Exists(Path.Combine(
                Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln")));
            Assert.IsNotNull(document);
        }
    }
}
