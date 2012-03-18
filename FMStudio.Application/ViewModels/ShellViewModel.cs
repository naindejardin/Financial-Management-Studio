﻿using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Application.Properties;
using FMStudio.Application.Services;
using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
{
    [Export]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private readonly IShellService shellService;
        private object contentView;


        [ImportingConstructor]
        public ShellViewModel(IShellView view, IPresentationService presentationService, IShellService shellService)
            : base(view)
        {
            this.shellService = shellService;
            view.Closing += ViewClosing;
            view.Closed += ViewClosed;

            // Restore the window size when the values are valid.
            if (Settings.Default.Left >= 0 && Settings.Default.Top >= 0 && Settings.Default.Width > 0 && Settings.Default.Height > 0
                && Settings.Default.Left + Settings.Default.Width <= presentationService.VirtualScreenWidth
                && Settings.Default.Top + Settings.Default.Height <= presentationService.VirtualScreenHeight)
            {
                ViewCore.Left = Settings.Default.Left;
                ViewCore.Top = Settings.Default.Top;
                ViewCore.Height = Settings.Default.Height;
                ViewCore.Width = Settings.Default.Width;
            }
            ViewCore.IsMaximized = Settings.Default.IsMaximized;
        }


        public string Title { get { return ApplicationInfo.ProductName; } }

        public IShellService ShellService { get { return this.shellService; } }

        public object ContentView
        {
            get { return contentView; }
            set
            {
                if (contentView != value)
                {
                    contentView = value;
                    RaisePropertyChanged("ContentView");
                }
            }
        }


        public event CancelEventHandler Closing;


        public void Show()
        {
            ViewCore.Show();
        }

        public void Close()
        {
            ViewCore.Close();
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            if (Closing != null) { Closing(this, e); }
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            OnClosing(e);
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            Settings.Default.Left = ViewCore.Left;
            Settings.Default.Top = ViewCore.Top;
            Settings.Default.Height = ViewCore.Height;
            Settings.Default.Width = ViewCore.Width;
            Settings.Default.IsMaximized = ViewCore.IsMaximized;
        }
    }
}
