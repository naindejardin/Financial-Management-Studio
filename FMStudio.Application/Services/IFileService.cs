using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using BillList.Applications.Documents;
using FMStudio.Application.Documents;

namespace FMStudio.Application.Services
{
    public interface IFileService : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<IDocument> OpenedDocuments { get; }

        SolutionDocument SolutionDoc { get; set; }

        IDocument ActiveDocument { get; set; }

        string SolutionName { get; }

        RecentFileList RecentSolutionList { get; }

        ICommand NewDocumentCommand { get; }

        ICommand CloseDocumentCommand { get; }

        ICommand SaveDocumentCommand { get; }

        ICommand SaveAllDocumentCommand { get; }

        ICommand NewSolutionCommand { get; }

        ICommand OpenSolutionCommand { get; }

        ICommand CloseSolutionCommand { get; }

        ICommand ShowSolutionCommand { get; }
    }
}
