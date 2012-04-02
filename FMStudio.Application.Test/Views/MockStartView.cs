using System.ComponentModel.Composition;
using FMStudio.Applications.Views;

namespace FMStudio.Applications.Test.Views
{
    [Export(typeof(IStartView))]
    public class MockStartView : MockView, IStartView
    {
    }
}
