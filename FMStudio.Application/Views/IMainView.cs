using BigEgg.Framework.Applications;

namespace FMStudio.Applications.Views
{
    public interface IMainView : IView
    {
        ContentViewState ContentViewState { get; set; }
    }
}
