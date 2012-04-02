using System.ComponentModel.Composition;
using FMStudio.Applications.Views;

namespace FMStudio.Applications.Test.Views
{
    [Export(typeof(ISolutionView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockSolutionView : MockView, ISolutionView
    {
    }
}
