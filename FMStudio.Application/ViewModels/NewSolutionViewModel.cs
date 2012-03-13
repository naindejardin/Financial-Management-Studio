using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
{
    public class NewSolutionViewModel : ViewModel<INewSolutionView>
    {
        private readonly DelegateCommand yesCommand;
        private readonly DelegateCommand noCommand;
        private bool? dialogResult;


        public NewSolutionViewModel(INewSolutionView view, string defaultSolutionName, string defaultLocation)
            : base(view)
        {
            SolutionName = defaultSolutionName;
            Location = defaultLocation;
            this.yesCommand = new DelegateCommand(() => Close(true));
            this.noCommand = new DelegateCommand(() => Close(false));
        }


        public static string Title { get { return "New Solution"; } }

        public string SolutionName { get; set; }

        public string Location { get; set; }

        public ICommand YesCommand { get { return yesCommand; } }

        public ICommand NoCommand { get { return noCommand; } }


        public bool? ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
            return this.dialogResult;
        }

        private void Close(bool? dialogResult)
        {
            this.dialogResult = dialogResult;
            ViewCore.Close();
        }
    }
}
