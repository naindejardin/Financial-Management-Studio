using System;
using System.Collections.Generic;
using System.Linq;
using BillList.Applications.Documents;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public class SaveChangesDialogViewModel : DialogViewModel<ISaveChangesDialogView>
    {
        private readonly IEnumerable<IDocument> documents;


        public SaveChangesDialogViewModel(ISaveChangesDialogView view, IEnumerable<IDocument> documents)
            : base(view)
        {
            if (!documents.Any()) { throw new ArgumentException("documents"); }
            this.documents = documents;
        }


        public IEnumerable<IDocument> Documents { get { return documents; } }
    }
}
