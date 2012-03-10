﻿namespace BillList.Applications.Documents
{
    public interface IDocumentType
    {
        string Description { get; }

        string FileExtension { get; }


        bool CanNew();

        IDocument New(string fullFilePath);

        bool CanOpen();

        IDocument Open(string fullFilePath, string aliasName);

        bool CanSave(IDocument document);

        void Save(IDocument document, string fullFilePath);
    }
}
