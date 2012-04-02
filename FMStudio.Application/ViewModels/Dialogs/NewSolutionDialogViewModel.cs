using FMStudio.Applications.Properties;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public class NewSolutionDialogViewModel : DialogViewModel<INewSolutionDialogView>
    {
        public NewSolutionDialogViewModel(INewSolutionDialogView view)
            : base(view)
        {
            SolutionName = Resources.DefaultNewSolutionName;
            Location = Settings.Default.DefaultNewSolutionLocation;
        }


        public string SolutionName { get; set; }

        public string Location { get; set; }
    }
}
