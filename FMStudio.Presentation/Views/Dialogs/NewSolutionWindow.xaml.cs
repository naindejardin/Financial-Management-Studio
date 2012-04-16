using System.ComponentModel.Composition;
using System.Windows;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Presentation.Views.Dialogs
{
    [Export(typeof(INewSolutionDialogView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class NewSolutionWindow : DialogWindow, INewSolutionDialogView
    {
        public NewSolutionWindow()
        {
            InitializeComponent();
        }
    }
}
