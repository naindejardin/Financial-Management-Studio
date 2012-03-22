using System.ComponentModel.Composition;
using FMStudio.Application.Views;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(IMainView))]
    public class MockMainView : MockView, IMainView
    {
        public ContentViewState ContentViewState { get; set; }
    }
}
