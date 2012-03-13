using BigEgg.Framework.Applications;

namespace FMStudio.Application.Views
{
    public interface ISaveChangesView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
