using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public abstract class DialogViewModel<TView> : ViewModel<TView> where TView : IDialogView
    {
        private readonly DelegateCommand yesCommand;
        private readonly DelegateCommand noCommand;
        private bool? dialogResult;


        public DialogViewModel(TView view)
            : base(view)
        {
            this.yesCommand = new DelegateCommand(() => Close(true));
            this.noCommand = new DelegateCommand(() => Close(false));
        }


        public static string Title { get { return ApplicationInfo.ProductName; } }

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
