using System.ComponentModel;

namespace FMStudio.Applications.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object ShellView { get; }

        string SolutionName { get; set; }
    }
}
