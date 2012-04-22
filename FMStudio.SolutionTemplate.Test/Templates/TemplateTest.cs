using System;
using BigEgg.Framework.UnitTesting;
using FMStudio.Applications.Documents;
using FMStudio.SolutionTemplate.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.SolutionTemplate.Test.Templates
{
    [TestClass]
    public class TemplateTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase("", "description", "category"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase("name", "", "category"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase("name", "description", ""));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase("name", "description", null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase("name", null, "category"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase(null, "description", "category"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase("name", null, null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase(null, "description", null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockTemplateBase(null, null, "category"));

            AssertHelper.ExpectedException<ArgumentException>(() => new TemplateCategory(null));
            AssertHelper.ExpectedException<ArgumentException>(() => new TemplateCategory(""));
        }


        private class MockTemplateBase : Template
        {
            public MockTemplateBase(string name, string description, string category)
                : base(name, description, category)
            {
            }


            public SolutionDocument CallNewCore(string fullFilePath) { return NewCore(fullFilePath); }
        }

    }
}
