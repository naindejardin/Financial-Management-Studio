using System.ComponentModel.Composition;
using System.Windows;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Presentation.Views.Dialogs
{
    [Export(typeof(INewDocumentDialogView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class NewDocumentWindow : Window, INewDocumentDialogView
    {
        public NewDocumentWindow()
        {
            InitializeComponent();
        }


        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }
    }
}
