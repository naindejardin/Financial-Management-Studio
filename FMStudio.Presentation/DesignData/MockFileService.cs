using System.Collections.ObjectModel;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Services;
using FMStudio.Documents;

namespace FMStudio.Presentation.DesignData
{
    public class MockFileService : DataModel, IFileService
    {
        public ReadOnlyObservableCollection<IDocument> OpenedDocuments { get; set; }

        public IDocument ActiveDocument { get; set; }

        public SolutionDocument SolutionDoc { get; set; }

        public string SolutionName { get; set; }

        public RecentFileList RecentSolutionList { get; set; }

        public ICommand NewDocumentCommand { get; set; }

        public ICommand CloseDocumentCommand { get; set; }

        public ICommand SaveDocumentCommand { get; set; }

        public ICommand SaveAllDocumentCommand { get; set; }

        public ICommand NewSolutionCommand { get; set; }

        public ICommand OpenSolutionCommand { get; set; }

        public ICommand CloseSolutionCommand { get; set; }

        public ICommand ShowSolutionCommand { get; set; }
    }
}
