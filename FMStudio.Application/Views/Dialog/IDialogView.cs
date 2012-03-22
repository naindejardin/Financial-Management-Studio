using BigEgg.Framework.Applications;

namespace FMStudio.Application.Views
{
    public interface IDialogView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
