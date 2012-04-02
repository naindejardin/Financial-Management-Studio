using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Threading;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Properties;
using FMStudio.Applications.Services;
using FMStudio.Applications.ViewModels;

namespace FMStudio.Applications.Controllers
{
    /// <summary>
    /// Responsible for the application lifecycle.
    /// </summary>
    [Export(typeof(IApplicationController))]
    internal class ApplicationController : Controller, IApplicationController
    {
        private readonly IEnvironmentService environmentService;
        private readonly FileController fileController;
        private readonly SolutionDocumentController solutionDocumentController;
        private readonly ShellViewModel shellViewModel;
        private readonly MainViewModel mainViewModel;
        private readonly StartViewModel startViewModel;
        private readonly DelegateCommand exitCommand;


        [ImportingConstructor]
        public ApplicationController(CompositionContainer container, IEnvironmentService environmentService,
            IPresentationService presentationService, ShellService shellService, FileController fileController)
        {
            InitializeCultures();
            presentationService.InitializeCultures();

            this.environmentService = environmentService;
            this.fileController = fileController;

            this.solutionDocumentController = container.GetExportedValue<SolutionDocumentController>();
            this.shellViewModel = container.GetExportedValue<ShellViewModel>();
            this.mainViewModel = container.GetExportedValue<MainViewModel>();
            this.startViewModel = container.GetExportedValue<StartViewModel>();

            shellService.ShellView = shellViewModel.View;
            this.shellViewModel.Closing += ShellViewModelClosing;
            this.exitCommand = new DelegateCommand(Close);
        }


        public void Initialize()
        {
            mainViewModel.StartView = startViewModel.View;
            mainViewModel.ExitCommand = exitCommand;

            fileController.Initialize();
        }

        public void Run()
        {
            shellViewModel.ContentView = mainViewModel.View;

            if (!string.IsNullOrEmpty(environmentService.SolutionPath))
            {
                fileController.OpenSolution(environmentService.SolutionPath);
            }

            shellViewModel.Show();
        }

        public void Shutdown()
        {
            fileController.Shutdown();

            if (mainViewModel.NewLanguage != null)
            {
                Settings.Default.UICulture = mainViewModel.NewLanguage.Name;
            }
            try
            {
                Settings.Default.Save();
            }
            catch (Exception)
            {
                // When more application instances are closed at the same time then an exception occurs.
            }
        }


        private static void InitializeCultures()
        {
            if (!String.IsNullOrEmpty(Settings.Default.Culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Culture);
            }
            if (!String.IsNullOrEmpty(Settings.Default.UICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.UICulture);
            }
        }

        private void Close()
        {
            shellViewModel.Close();
        }

        private void ShellViewModelClosing(object sender, CancelEventArgs e)
        {
            // Try to close all documents and see if the user has already saved them.
            e.Cancel = !fileController.CloseSolution();
        }
    }
}
