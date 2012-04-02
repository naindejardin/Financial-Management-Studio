using System;
using System.IO;
using BigEgg.Framework.UnitTesting;
using BillList.Applications.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test
{
    [TestClass()]
    public class DocumentTypeTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("", ".fmsln"));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("FMS Solution Documents", null));
            AssertHelper.ExpectedException<ArgumentException>(() => new MockDocumentTypeBase("FMS Solution Documents", "sln"));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new DocumentBaseMock(null));
        }

        [TestMethod]
        public void CheckBaseImplementation()
        {
            MockDocumentTypeBase documentType = new MockDocumentTypeBase("FMStuido Solution Documents", ".fmsln");
            Assert.IsFalse(documentType.CanNew());
            Assert.IsFalse(documentType.CanOpen());
            Assert.IsFalse(documentType.CanSave(null));

            MockDocumentTypeBase documentType2 = new MockDocumentTypeBase("Financial Data Files", ".fdd");
            AssertHelper.ExpectedException<NotSupportedException>(() =>
                documentType.New(Path.Combine(Environment.CurrentDirectory, "TestDocument1")));
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.Open("TestDocument1.fdd"));
            AssertHelper.ExpectedException<NotSupportedException>(() =>
                documentType.Save(new DocumentBaseMock(documentType2), "TestDocument1.fdd"));

            AssertHelper.ExpectedException<ArgumentException>(() => documentType.Open(""));
            AssertHelper.ExpectedException<ArgumentException>(() =>
                documentType.Save(new DocumentBaseMock(documentType2), ""));
            AssertHelper.ExpectedException<ArgumentNullException>(() => documentType.Save(null, "TestDocument1.fdd"));

            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallNewCore(null));
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallOpenCore(null));
            AssertHelper.ExpectedException<NotSupportedException>(() => documentType.CallSaveCore(null, null));
        }

        private class MockDocumentTypeBase : DocumentType
        {
            public MockDocumentTypeBase(string description, string fileExtension)
                : base(description, fileExtension)
            {
            }


            public IDocument CallNewCore(string fullFilePath) { return NewCore(fullFilePath); }

            public IDocument CallOpenCore(string fullFilePath) { return OpenCore(fullFilePath); }

            public void CallSaveCore(IDocument document, string fullFilePath)
            {
                SaveCore(document, fullFilePath);
            }
        }

        private class DocumentBaseMock : Document
        {
            public DocumentBaseMock(MockDocumentTypeBase documentType)
                : base(documentType)
            {
            }
        }
    }
}
