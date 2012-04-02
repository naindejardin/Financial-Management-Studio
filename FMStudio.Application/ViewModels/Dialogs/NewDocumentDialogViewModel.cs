﻿using System;
using System.Collections.Generic;
using System.Linq;
using BillList.Applications.Documents;
using FMStudio.Application.Properties;
using FMStudio.Application.Views.Dialogs;

namespace FMStudio.Application.ViewModels.Dialogs
{
    public class NewDocumentDialogViewModel : DialogViewModel<INewDocumentDialogView>
    {
        private IEnumerable<IDocumentType> documentTypes;


        public NewDocumentDialogViewModel(INewDocumentDialogView view, IEnumerable<IDocumentType> documentTypes)
            : base(view)
        {
            if (!documentTypes.Any()) { throw new ArgumentException("documentTypes"); }
            this.documentTypes = documentTypes;
            FileName = Resources.DefaultNewDocumentName;
            SelectDocumentType = this.DocumentTypes.First();
        }


        public string FileName { get; set; }

        public IEnumerable<IDocumentType> DocumentTypes { get { return this.documentTypes; } }

        public IDocumentType SelectDocumentType { get; set; }
    }
}
