using System.ComponentModel.Composition;
using FMStudio.Application.Views;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(ISolutionView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockSolutionView : MockView, ISolutionView
    {
    }
}
