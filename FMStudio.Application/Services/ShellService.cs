using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;

namespace FMStudio.Application.Services
{
    [Export(typeof(IShellService)), Export]
    internal class ShellService : DataModel, IShellService
    {
        private string documentName;
        private object shellView;


        [ImportingConstructor]
        public ShellService()
        {
        }

        public string DocumentName
        {
            get { return documentName; }
            set
            {
                if (documentName != value)
                {
                    documentName = value;
                    RaisePropertyChanged("DocumentName");
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
