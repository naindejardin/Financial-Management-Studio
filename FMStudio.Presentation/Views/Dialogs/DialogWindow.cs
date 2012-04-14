using System.Windows;
using System.Windows.Controls;

namespace FMStudio.Presentation.Views.Dialogs
{
    public abstract class DialogWindow : Window
    {
        public DialogWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Style = (Style)FindResource("DialogStyle"); 

            ControlTemplate baseWindowTemplate = (ControlTemplate)App.Current.Resources["DialogTemplate"];

            Button closeBtn = (Button)baseWindowTemplate.FindName("CloseButton", this);
            closeBtn.Click += delegate
            {
                this.Close();
            };

            Border tp = (Border)baseWindowTemplate.FindName("HeadBar", this);
            tp.MouseLeftButtonDown += delegate
            {
                this.DragMove();
            };


        }
    }
}
