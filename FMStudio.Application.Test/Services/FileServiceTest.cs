using System.ComponentModel;
using BigEgg.Framework.UnitTesting;
using FMStudio.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigEgg.Framework.Applications;

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
            AssertHelper.PropertyChangedEvent(fileService, x => x.RecentFileList, () => fileService.RecentFileList = recentFileList);
            Assert.AreEqual(recentFileList, fileService.RecentFileList);
        }
    }
}
