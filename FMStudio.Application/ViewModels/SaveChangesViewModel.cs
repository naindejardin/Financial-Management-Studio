using System.Collections.Generic;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using BillList.Applications.Documents;
using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
{
    public class SaveChangesViewModel : ViewModel<ISaveChangesView>
    {
        private readonly IEnumerable<IDocument> documents;
        private readonly DelegateCommand yesCommand;
        private readonly DelegateCommand noCommand;
        private bool? dialogResult;


        public SaveChangesViewModel(ISaveChangesView view, IEnumerable<IDocument> documents)
            : base(view)
        {
            this.documents = documents;
            this.yesCommand = new DelegateCommand(() => Close(true));
            this.noCommand = new DelegateCommand(() => Close(false));
        }


        public static string Title { get { return ApplicationInfo.ProductName; } }

        public IEnumerable<IDocument> Documents { get { return documents; } }

        public ICommand YesCommand { get { return yesCommand; } }

        public ICommand NoCommand { get { return noCommand; } }


        public bool? ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
            return dialogResult;
        }

        private void Close(bool? dialogResult)
        {
            this.dialogResult = dialogResult;
            ViewCore.Close();
        }
    }
}
