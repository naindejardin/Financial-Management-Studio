using System.ComponentModel;
using BigEgg.Framework.Applications;
using BigEgg.Framework.UnitTesting;
using FMStudio.Applications.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test.Services
{
    [TestClass]
    public class FileServiceTest : TestClassBase
    {
        [TestMethod]
        public void RecentSolutionListTest()
        {
            FileService fileService = Container.GetExportedValue<FileService>();

            RecentFileList recentSolutionList = new RecentFileList();
            recentSolutionList.AddFile("TestFile");
            AssertHelper.PropertyChangedEvent(fileService, x => x.RecentSolutionList, () => fileService.RecentSolutionList = recentSolutionList);
            Assert.AreEqual(recentSolutionList, fileService.RecentSolutionList);
        }
    }
}
