using BigEgg.Framework.Applications;

namespace FMStudio.Application.Views.Dialogs
{
    public interface IDialogView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
