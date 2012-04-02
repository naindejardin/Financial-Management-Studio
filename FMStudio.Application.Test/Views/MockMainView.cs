using System.ComponentModel.Composition;
using FMStudio.Applications.Views;

namespace FMStudio.Applications.Test.Views
{
    [Export(typeof(IMainView))]
    public class MockMainView : MockView, IMainView
    {
        public ContentViewState ContentViewState { get; set; }
    }
}
