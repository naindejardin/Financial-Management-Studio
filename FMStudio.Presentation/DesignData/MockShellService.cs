﻿using BigEgg.Framework.Applications;
using FMStudio.Applications.Services;

namespace FMStudio.Presentation.DesignData
{
    public class MockShellService : DataModel, IShellService
    {
        public MockShellService()
        {
            SolutionName = "New Solution";
        }
        
        public object ShellView { get; set; }

        public string SolutionName { get; set; }
    }
}
