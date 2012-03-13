using BigEgg.Framework.Applications;

namespace FMStudio.Application.Views
{
    public interface INewSolutionView : IView
    {
        void ShowDialog(object owner);

        void Close();
    }
}
