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

        public MainView()
        {
            InitializeComponent();
            //VisualStateManager.GoToElementState(rootContainer, ContentViewState.ToString(), false);

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


        private void FileMenuItemSubmenuOpened(object sender, RoutedEventArgs e)
        {
            if (recentSolutionMenuItem.HasItems)
                return;

            if (ViewModel.FileService.RecentSolutionList.RecentFiles.Any())
            {
                for (int i = 0; i < ViewModel.FileService.RecentSolutionList.RecentFiles.Count; i++)
                {
                    RecentFile recentSolution = ViewModel.FileService.RecentSolutionList.RecentFiles[i];
                    MenuItem menuItem = new MenuItem()
                    {
                        Header = GetNumberText(i) + " "
                            + MenuFileNameConverter.Default.Convert(recentSolution.Path, null, null, CultureInfo.CurrentCulture),
                        ToolTip = recentSolution.Path,
                        Command = ViewModel.FileService.OpenSolutionCommand,
                        CommandParameter = recentSolution.Path,
                        IsEnabled = true
                    };
                    recentSolutionMenuItem.Items.Add(menuItem);
                }
            }
            else
            {
                MenuItem menuItem = new MenuItem()
                {
                    Header = FMStudio.Presentation.Properties.Resources.RecentSolutionNullMenu,
                    IsEnabled = false
                };
                recentSolutionMenuItem.Items.Add(menuItem);
            }
        }

        private void FileMenuItemSubmenuClosed(object sender, RoutedEventArgs e)
        {
            if (fileMenu.IsSubmenuOpen)
                return;

            recentSolutionMenuItem.Items.Clear();
        }

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
