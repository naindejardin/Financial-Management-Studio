using FMStudio.Application.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.ViewModels
{
    [TestClass]
    public class StartViewModelTest : TestClassBase
    {
        [TestMethod]
        public void GetFileService()
        {
            StartViewModel startViewModel = Container.GetExportedValue<StartViewModel>();
            Assert.IsNotNull(startViewModel.FileService);
        }
    }
}
