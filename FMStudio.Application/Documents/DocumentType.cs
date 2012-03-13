using System;
using System.IO;
using BigEgg.Framework.Applications;

namespace BillList.Applications.Documents
{
    public abstract class DocumentType : DataModel, IDocumentType
    {
        private string description;
        private string fileExtension;


        protected DocumentType(string description, string fileExtension)
        {
            if (string.IsNullOrEmpty(description)) { throw new ArgumentException("description must not be null or empty."); }
            if (string.IsNullOrEmpty(fileExtension)) { throw new ArgumentException("fileExtension must not be null or empty"); }
            if (fileExtension[0] != '.') { throw new ArgumentException("The argument fileExtension must start with the '.' character."); }

            this.description = description;
            this.fileExtension = fileExtension;
        }


        public string Description { get { return description; } }

        public string FileExtension { get { return fileExtension; } }


        public virtual bool CanNew() { return false; }

        public IDocument New(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath)) { throw new ArgumentException("fullFilePath must not be null or empty."); }
            if (!CanNew()) { throw new NotSupportedException("The New operation is not supported. CanNew returned false."); }

            if (fullFilePath.Substring(fullFilePath.Length-4-1,4).CompareTo(FileExtension) != 0)
                fullFilePath = fullFilePath + FileExtension;

            if (File.Exists(fullFilePath)) 
                { throw new ArgumentException("The file has already exist."); };

            IDocument document = NewCore(fullFilePath);
            if (document != null)
            {
                document.FullFilePath = fullFilePath;
            }
            return document;
        }

        public virtual bool CanOpen() { return false; }

        public IDocument Open(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath)) { throw new ArgumentException("fullFilePath must not be null or empty."); }
            if (!CanOpen()) { throw new NotSupportedException("The Open operation is not supported. CanOpen returned false."); }
            if (!File.Exists(fullFilePath)) { throw new FileNotFoundException("The file: " + fullFilePath + " dose not exist."); };

            IDocument document = OpenCore(fullFilePath);
            if (document != null) 
            {
                document.FullFilePath = fullFilePath;
            }
            return document;
        }

        public virtual bool CanSave(IDocument document) { return false; }

        public void Save(IDocument document, string fullFilePath)
        {
            if (document == null) { throw new ArgumentNullException("document"); }
            if (string.IsNullOrEmpty(fullFilePath)) { throw new ArgumentException("fileName must not be null or empty."); }
            if (!CanSave(document)) { throw new NotSupportedException("The Save operation is not supported. CanSave returned false."); }

            if (fullFilePath.Substring(fullFilePath.Length - 4 - 1, 4).CompareTo(FileExtension) != 0)
                fullFilePath = fullFilePath + FileExtension;

            SaveCore(document, fullFilePath);

            if (CanOpen())
            {
                document.FullFilePath = fullFilePath;
                document.Modified = false;
            }
        }

        protected virtual IDocument NewCore(string fullFilePath)
        {
            throw new NotSupportedException();
        }

        protected virtual IDocument OpenCore(string fullFilePath)
        {
            throw new NotSupportedException();
        }

        protected virtual void SaveCore(IDocument document, string fullFilePath)
        {
            throw new NotSupportedException();
        }
    }
}
