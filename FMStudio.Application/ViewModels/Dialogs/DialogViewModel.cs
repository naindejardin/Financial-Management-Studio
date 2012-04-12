using BigEgg.Framework.Applications;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public abstract class DialogViewModel<TView> : ViewModel<TView> where TView : IDialogView
    {
        private bool? dialogResult;


        public DialogViewModel(TView view)
            : base(view)
        {
        }


        public static string Title { get { return ApplicationInfo.ProductName; } }


        public bool? ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
            return this.dialogResult;
        }

        protected void Close(bool? dialogResult)
        {
            this.dialogResult = dialogResult;
            ViewCore.Close();
        }
    }
}
