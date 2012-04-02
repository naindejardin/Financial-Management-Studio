using System.ComponentModel.Composition;
using FMStudio.Applications.Services;

namespace FMStudio.Applications.Test.Services
{
    [Export(typeof(IEnvironmentService)), Export]
    public class MockEnvironmentService : IEnvironmentService
    {
        public string SolutionPath { get; set; }
    }
}
