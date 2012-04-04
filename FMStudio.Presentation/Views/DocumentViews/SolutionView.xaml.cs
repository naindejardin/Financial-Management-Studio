using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using BigEgg.Framework.Applications;
using FMStudio.Applications.ViewModels;
using FMStudio.Applications.Views;

namespace FMStudio.Presentation.Views.DocumentViews
{
    [Export(typeof(ISolutionView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SolutionView : UserControl, ISolutionView
    {
        private readonly Lazy<SolutionViewModel> viewModel;
        private bool suppressTextChanged;
        private IEnumerable<Control> dynamicContextMenuItems;


        public SolutionView()
        {
            InitializeComponent();

            viewModel = new Lazy<SolutionViewModel>(() => ViewHelper.GetViewModel<SolutionViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
            IsVisibleChanged += IsVisibleChangedHandler;
        }


        private SolutionViewModel ViewModel { get { return viewModel.Value; } }


        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            // Ensure that this handler is called only once.
            Loaded -= FirstTimeLoadedHandler;

            suppressTextChanged = true;
            //  Do some things
            suppressTextChanged = false;
        }

        private void IsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.IsVisible = IsVisible;
        }
    }
}
