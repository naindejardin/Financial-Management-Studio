using BigEgg.Framework.Applications;

namespace FMStudio.Application.Views
{
    public interface IMainView : IView
    {
        ContentViewState ContentViewState { get; set; }
    }
}
