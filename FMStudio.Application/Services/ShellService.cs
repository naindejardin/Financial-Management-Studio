using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;

namespace FMStudio.Application.Services
{
    [Export(typeof(IShellService)), Export]
    internal class ShellService : DataModel, IShellService
    {
        private string solutionName;
        private object shellView;


        [ImportingConstructor]
        public ShellService()
        {
            this.solutionName = string.Empty;
        }

        public string SolutionName
        {
            get { return this.solutionName; }
            set
            {
                if (this.solutionName != value)
                {
                    this.solutionName = value;
                    RaisePropertyChanged("SolutionName");
                }
            }
        }

        public object ShellView
        {
            get { return shellView; }
            set
            {
                if (shellView != value)
                {
                    shellView = value;
                    RaisePropertyChanged("ShellView");
                }
            }
        }
    }
}
