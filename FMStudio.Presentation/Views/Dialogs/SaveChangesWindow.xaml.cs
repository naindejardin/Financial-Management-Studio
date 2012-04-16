using System.ComponentModel.Composition;
using System.Windows;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Presentation.Views.Dialogs
{
    [Export(typeof(ISaveChangesDialogView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SaveChangesWindow : DialogWindow, ISaveChangesDialogView
    {
        public SaveChangesWindow()
        {
            InitializeComponent();
        }
    }
}
