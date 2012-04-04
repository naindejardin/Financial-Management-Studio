using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BigEgg.Framework.Applications;
using FMStudio.Applications.ViewModels;
using FMStudio.Applications.Views;
using FMStudio.Presentation.Converters;

namespace FMStudio.Presentation.Views
{
    [Export(typeof(IMainView))]
    public partial class MainView : UserControl, IMainView
    {
        private readonly Lazy<MainViewModel> viewModel;
        private ContentViewState contentViewState;
        private IEnumerable<Control> dynamicFileMenuItems = new Control[] { };


        public MainView()
        {
            InitializeComponent();
            VisualStateManager.GoToElementState(rootContainer, ContentViewState.ToString(), false);

            viewModel = new Lazy<MainViewModel>(() => ViewHelper.GetViewModel<MainViewModel>(this));
        }


        public ContentViewState ContentViewState
        {
            get { return contentViewState; }
            set
            {
                if (contentViewState != value)
                {
                    contentViewState = value;
                    VisualStateManager.GoToElementState(rootContainer, value.ToString(), true);
                }
            }
        }

        private MainViewModel ViewModel { get { return viewModel.Value; } }


        //private void FileMenuItemSubmenuOpened(object sender, RoutedEventArgs e)
        //{
        //    if (ViewModel.FileService.RecentSolutionList.RecentFiles.Any())
        //    {
        //        List<Control> menuItems = new List<Control>();

        //        menuItems.Add(new Separator());
        //        for (int i = 0; i < ViewModel.FileService.RecentSolutionList.RecentFiles.Count; i++)
        //        {
        //            RecentFile recentFile = ViewModel.FileService.RecentSolutionList.RecentFiles[i];
        //            MenuItem menuItem = new MenuItem()
        //            {
        //                Header = GetNumberText(i) + " "
        //                    + MenuFileNameConverter.Default.Convert(recentFile.Path, null, null, CultureInfo.CurrentCulture),
        //                ToolTip = recentFile.Path,
        //                Command = ViewModel.FileService.OpenSolutionCommand,
        //                CommandParameter = recentFile.Path
        //            };
        //            menuItems.Add(menuItem);
        //        }

        //        foreach (Control item in menuItems)
        //        {
        //            fileMenuItem.Items.Add(item);
        //        }

        //        dynamicFileMenuItems = menuItems;
        //    }
        //}

        //private void FileMenuItemSubmenuClosed(object sender, RoutedEventArgs e)
        //{
        //    foreach (Control menuItem in dynamicFileMenuItems)
        //    {
        //        fileMenuItem.Items.Remove(menuItem);
        //    }
        //}

        private static string GetNumberText(int index)
        {
            if (index >= 0 && index < 9)
            {
                return "_" + (index + 1);
            }
            else
            {
                return " ";
            }
        }
    }
}
