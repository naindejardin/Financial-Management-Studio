using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace FMStudio.Applications.Services
{
    [Export(typeof(IEnvironmentService))]
    internal class EnvironmentService : IEnvironmentService
    {
        private readonly Lazy<string> solutionPath = new Lazy<string>(() => Environment.GetCommandLineArgs().ElementAtOrDefault(1));


        public string SolutionPath { get { return solutionPath.Value; } }
    }
}
