using System.Linq;
using BigEgg.Framework.Applications;
using FMStudio.Applications.ViewModels;
using FMStudio.Applications.Views;

namespace FMStudio.Presentation.DesignData
{
    public class SampleStartViewModel : StartViewModel
    {
        public SampleStartViewModel()
            : base(new MockStartView(), new MockFileService())
        {
            ((MockFileService)FileService).RecentSolutionList = new RecentFileList();
            FileService.RecentSolutionList.AddFile(@"C:\Users\Admin\My Documents\Document 1.rtf");
            FileService.RecentSolutionList.AddFile(@"C:\Users\Admin\My Documents\WPF Application Framework (WAF).rtf");
            FileService.RecentSolutionList.AddFile(@"C:\Users\Admin\My Documents\WAF Writer\Readme.rtf");
            FileService.RecentSolutionList.RecentFiles.First().IsPinned = true;
        }


        private class MockStartView : MockView, IStartView
        {
        }
    }
}
