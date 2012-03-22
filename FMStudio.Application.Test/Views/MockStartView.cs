using System.ComponentModel.Composition;
using FMStudio.Application.Views;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(IStartView))]
    public class MockStartView : MockView, IStartView
    {
    }
}
