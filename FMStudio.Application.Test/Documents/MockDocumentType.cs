using System;
using System.IO;
using BillList.Applications.Documents;

namespace FMStudio.Application.Test.Documents
{
    public class MockDocumentType : DocumentType
    {
        public MockDocumentType(string description, string fileExtension)
            : base(description, fileExtension)
        {
            CanSaveResult = true;
        }


        public bool CanSaveResult { get; set; }

        public DocumentOperation DocumentOperation { get; private set; }

        public IDocument Document { get; private set; }

        public string FullFilePath { get; private set; }

        public bool ThrowException { get; set; }


        public void Clear()
        {
            DocumentOperation = DocumentOperation.None;
            FullFilePath = null;
            Document = null;
        }

        public override bool CanNew() { return true; }

        protected override IDocument NewCore(string fullFilePath)
        {
            CheckThrowException();
            DocumentOperation = DocumentOperation.New;
            FullFilePath = fullFilePath;
            return new MockDocument(this);
        }

        public override bool CanOpen() { return true; }

        protected override IDocument OpenCore(string fullFilePath)
        {
            CheckThrowException();
            DocumentOperation = DocumentOperation.Open;
            FullFilePath = Path.GetFullPath(fullFilePath);
            return new MockDocument(this);
        }

        public override bool CanSave(IDocument document) { return CanSaveResult && document is MockDocument; }

        protected override void SaveCore(IDocument document, string fullFilePath)
        {
            CheckThrowException();
            DocumentOperation = DocumentOperation.Save;
            Document = document;
            FullFilePath = Path.GetFullPath(fullFilePath);
        }

        private void CheckThrowException()
        {
            if (ThrowException) { throw new InvalidOperationException("ThrowException has been activated on the MockDocumentType."); }
        }
    }

    public enum DocumentOperation
    {
        None,
        New,
        Open,
        Save
    }

    public class MockDocument : Document
    {
        public MockDocument(MockDocumentType documentType)
            : base(documentType)
        {
        }
    }
}
