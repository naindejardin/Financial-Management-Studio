using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using FMStudio.Application.Services;

namespace FMStudio.Application.Test.Services
{
    [Export(typeof(IPresentationService))]
    public class MockPresentationService : IPresentationService
    {
        public bool InitializeCulturesCalled { get; set; }

        public double VirtualScreenWidth { get; set; }

        public double VirtualScreenHeight { get; set; }


        public void InitializeCultures()
        {
            InitializeCulturesCalled = true;
        }
    }
}
