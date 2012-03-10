using System;
using BigEgg.Framework.Applications;

namespace BillList.Applications.Documents
{
    public abstract class Document : DataModel, IDocument
    {
        private readonly IDocumentType documentType;
        private string fullFilePath;
        private string aliasName;
        private bool modified;


        protected Document(IDocumentType documentType)
        {
            if (documentType == null) { throw new ArgumentNullException("documentType"); }
            this.documentType = documentType;
        }


        public IDocumentType DocumentType { get { return this.documentType; } }

        public string FullFilePath
        {
            get { return this.fullFilePath; }
            set
            {
                if (this.fullFilePath != value)
                {
                    this.fullFilePath = value;
                    RaisePropertyChanged("FullFilePath");
                }
            }
        }

        public string AliasName
        {
            get { return this.aliasName; }
            set
            {
                if (this.aliasName != value)
                {
                    this.aliasName = value;
                    RaisePropertyChanged("AliasName");
                }
            }
        }

        public bool Modified
        {
            get { return this.modified; }
            set
            {
                if (this.modified != value)
                {
                    this.modified = value;
                    RaisePropertyChanged("Modified");
                }
            }
        }
    }
}
