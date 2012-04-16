using System.ComponentModel.Composition;
using System.Windows;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Presentation.Views.Dialogs
{
    [Export(typeof(INewDocumentDialogView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class NewDocumentWindow : DialogWindow, INewDocumentDialogView
    {
        public NewDocumentWindow()
        {
            InitializeComponent();
        }
    }
}
