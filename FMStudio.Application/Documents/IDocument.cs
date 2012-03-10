﻿using System.ComponentModel;

namespace BillList.Applications.Documents
{
    public interface IDocument : INotifyPropertyChanged
    {
        IDocumentType DocumentType { get; }

        string FullFilePath { get; set; }

        string AliasName { get; set; }

        bool Modified { get; set; }
    }
}
