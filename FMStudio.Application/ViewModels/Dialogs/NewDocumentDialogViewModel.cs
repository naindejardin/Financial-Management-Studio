using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Documents;
using FMStudio.Applications.Properties;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public class NewDocumentDialogViewModel : DialogViewModel<INewDocumentDialogView>
    {
        private readonly DelegateCommand okCommand;
        private IEnumerable<IDocumentType> documentTypes;


        public NewDocumentDialogViewModel(INewDocumentDialogView view, IEnumerable<IDocumentType> documentTypes)
            : base(view)
        {
            if (!documentTypes.Any()) { throw new ArgumentException("documentTypes"); }
            this.documentTypes = documentTypes;
            FileName = Resources.DefaultNewDocumentName;
            SelectDocumentType = this.DocumentTypes.First();

            this.okCommand = new DelegateCommand(() => Close(true));
        }


        public ICommand OKCommand { get { return this.okCommand; } }

        public string FileName { get; set; }

        public IEnumerable<IDocumentType> DocumentTypes { get { return this.documentTypes; } }

        public IDocumentType SelectDocumentType { get; set; }
    }
}
