using BigEgg.Framework.Applications;

namespace FMStudio.Applications.Views.Dialogs
{
    public interface IDialogView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
