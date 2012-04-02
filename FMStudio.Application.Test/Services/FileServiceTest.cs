using System.ComponentModel;
using BigEgg.Framework.Applications;
using BigEgg.Framework.UnitTesting;
using FMStudio.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.Services
{
    [TestClass]
    public class FileServiceTest : TestClassBase
    {
        [TestMethod]
        public void RecentFileList()
        {
            FileService fileService = Container.GetExportedValue<FileService>();

            RecentFileList recentFileList = new RecentFileList();
            recentFileList.AddFile("TestFile");
            AssertHelper.PropertyChangedEvent(fileService, x => x.RecentSolutionList, () => fileService.RecentSolutionList = recentFileList);
            Assert.AreEqual(recentFileList, fileService.RecentSolutionList);
        }
    }
}
