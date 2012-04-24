using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Documents;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public class SaveChangesDialogViewModel : DialogViewModel<ISaveChangesDialogView>
    {
        private readonly DelegateCommand yesCommand;
        private readonly DelegateCommand noCommand;
        private readonly IEnumerable<IDocument> documents;


        public SaveChangesDialogViewModel(ISaveChangesDialogView view, IEnumerable<IDocument> documents)
            : base(view)
        {
            if (!documents.Any()) { throw new ArgumentException("documents"); }
            this.documents = documents;

            this.yesCommand = new DelegateCommand(() => Close(true));
            this.noCommand = new DelegateCommand(() => Close(false));
        }


        public ICommand YesCommand { get { return this.yesCommand; } }

        public ICommand NoCommand { get { return this.noCommand; } }

        public IEnumerable<IDocument> Documents { get { return this.documents; } }
    }
}
