using System.ComponentModel.Composition;
using FMStudio.Application.Services;

namespace FMStudio.Application.Test.Services
{
    [Export(typeof(IEnvironmentService)), Export]
    public class MockEnvironmentService : IEnvironmentService
    {
        public string SolutionPath { get; set; }
    }
}
