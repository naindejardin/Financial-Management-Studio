using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
{
    public class NewSolutionViewModel : DialogViewModel<IDialogView>
    {
        public NewSolutionViewModel(IDialogView view, string defaultSolutionName, string defaultLocation)
            : base(view)
        {
            SolutionName = defaultSolutionName;
            Location = defaultLocation;
        }


        public string SolutionName { get; set; }

        public string Location { get; set; }
    }
}
