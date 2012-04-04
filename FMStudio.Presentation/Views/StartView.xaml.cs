using System.ComponentModel.Composition;
using System.Windows.Controls;
using FMStudio.Applications.Views;
using System.Windows;

namespace FMStudio.Presentation.Views
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    [Export(typeof(IStartView))]
    public partial class StartView : UserControl, IStartView
    {
        public StartView()
        {
            InitializeComponent();

            //newButton.IsVisibleChanged += NewButtonIsVisibleChanged;
        }


        private void NewButtonIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if (newButton.IsVisible)
            //{
            //    newButton.Focus();
            //}
        }
    }
}
