using FMStudio.Applications.ViewModels;
using FMStudio.Applications.Views;
using FMStudio.Presentation.Views;

namespace FMStudio.Presentation.DesignData
{
    public class SampleMainViewModel : MainViewModel
    {
        public SampleMainViewModel()
            : base(new MockMainView() { ContentViewState = ContentViewState.StartViewVisible },
                 null, new MockShellService(), new MockFileService())
        {
            StartView = new StartView();
        }


        private class MockMainView : MockView, IMainView
        {
            public ContentViewState ContentViewState { get; set; }
        }
    }
}
