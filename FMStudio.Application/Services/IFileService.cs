using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using BillList.Applications.Documents;

namespace FMStudio.Application.Services
{
    public interface IFileService : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<IDocument> Documents { get; }

        IDocument SolutionDocument { get; set; }

        IDocument ActiveDocument { get; set; }

        RecentFileList RecentFileList { get; }

        ICommand NewCommand { get; }

        ICommand OpenCommand { get; }

        ICommand CloseCommand { get; }

        ICommand SaveCommand { get; }

        ICommand NewSolutionCommand { get; }

        ICommand OpenSolutionCommand { get; }

        ICommand CloseSolutionCommand { get; }

        ICommand SaveSolutionCommand { get; }
    }
}
