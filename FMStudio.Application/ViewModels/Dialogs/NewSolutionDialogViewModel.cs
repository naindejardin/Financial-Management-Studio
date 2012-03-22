using FMStudio.Application.Properties;
using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
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
