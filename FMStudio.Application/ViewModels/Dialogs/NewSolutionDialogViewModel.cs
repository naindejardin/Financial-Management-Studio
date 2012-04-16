using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Properties;
using FMStudio.Applications.Validations;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.ViewModels.Dialogs
{
    public class NewSolutionDialogViewModel : DialogViewModel<INewSolutionDialogView>
    {
        private readonly DelegateCommand okCommand;


        public NewSolutionDialogViewModel(INewSolutionDialogView view)
            : base(view)
        {
            SolutionName = Resources.DefaultNewSolutionName;
            Location = Settings.Default.DefaultNewSolutionLocation;

            this.okCommand = new DelegateCommand(() => Close(true));
        }


        public ICommand OKCommand { get { return this.okCommand; } }

        [Required(ErrorMessage = "Solution Name is required.")]
        public string SolutionName { get; set; }

        [Required(ErrorMessage = "Solution Location is required.")]
        [PathCheckValidation(ErrorMessage = "Solution Location should be a absolute path.")]
        public string Location { get; set; }
    }
}
