using BigEgg.Framework.Applications;
using FMStudio.Application.Controllers;
using FMStudio.Application.Services;
using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels;
using FMStudio.Application.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMStudio.Application.Test.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using FMStudio.Application.ViewModels.Dialogs;
using BigEgg.Framework.Applications.Services;
using FMStudio.Application.Properties;
using System.Globalization;

namespace FMStudio.Application.Test.Controllers
{
    [TestClass]
    public class ApplicationControllerTest : TestClassBase
    {
        protected override void OnTestCleanup()
        {
            base.OnTestCleanup();
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "NewSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "NewSolution"), true);
        }


        [TestMethod]
        public void ControllerLifecycle()
        {
            IApplicationController applicationController = Container.GetExportedValue<IApplicationController>();

            applicationController.Initialize();
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            Assert.IsNotNull(mainViewModel.ExitCommand);

            applicationController.Run();
            MockShellView shellView = (MockShellView)Container.GetExportedValue<IShellView>();
            Assert.IsTrue(shellView.IsVisible);
            ShellViewModel shellViewModel = ViewHelper.GetViewModel<ShellViewModel>(shellView);
            Assert.AreEqual(mainViewModel.View, shellViewModel.ContentView);

            mainViewModel.ExitCommand.Execute(null);
            Assert.IsFalse(shellView.IsVisible);

            applicationController.Shutdown();
        }

        [TestMethod]
        public void OpenFileViaCommandLine()
        {
            IFileService fileService = Container.GetExportedValue<IFileService>();

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            fileService.CloseSolutionCommand.Execute(null);

            MockEnvironmentService environmentService = Container.GetExportedValue<MockEnvironmentService>();
            environmentService.SolutionPath = Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln");

            IApplicationController applicationController = Container.GetExportedValue<IApplicationController>();
            applicationController.Initialize();

            // Open the 'Document.mock' file
            applicationController.Run();
            Assert.IsNotNull(fileService.SolutionDoc);
            Assert.AreEqual(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"),
                fileService.SolutionDoc.FullFilePath);

            // Open a file with an unknown file extension and check if an error message is shown.
            environmentService.SolutionPath = "Unknown.fileExtension";
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();

            applicationController.Run();

            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
        }

        [TestMethod]
        public void SaveChangesTest()
        {
            IApplicationController applicationController = Container.GetExportedValue<IApplicationController>();
            applicationController.Initialize();
            applicationController.Run();

            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            mainViewModel.FileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            mainViewModel.FileService.ActiveDocument = mainViewModel.FileService.SolutionDoc;

            SolutionViewModel solutionViewModel = ViewHelper.GetViewModel<SolutionViewModel>((IView)mainViewModel.ActiveDocumentView);
            solutionViewModel.Document.Modified = true;

            bool showDialogCalled = false;
            MockSaveChangesDialogView saveChangesView = Container.GetExportedValue<MockSaveChangesDialogView>();
            saveChangesView.ShowDialogAction = (view) =>
            {
                showDialogCalled = true;
                Assert.IsTrue(ViewHelper.GetViewModel<SaveChangesDialogViewModel>(view).Documents.SequenceEqual(
                    new[] { solutionViewModel.Document }));
                view.Close();
            };

            // When we try to close the ShellView then the ApplicationController shows the SaveChangesView because the
            // modified document wasn't saved.
            mainViewModel.ExitCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            MockShellView shellView = (MockShellView)Container.GetExportedValue<IShellView>();
            Assert.IsTrue(shellView.IsVisible);

            showDialogCalled = false;
            saveChangesView.ShowDialogAction = (view) =>
            {
                showDialogCalled = true;
                view.ViewModel.YesCommand.Execute(null);
            };

            // This time we let the SaveChangesView to save the modified document
            mainViewModel.ExitCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            Assert.IsFalse(shellView.IsVisible);
        }

        [TestMethod]
        public void SettingsTest()
        {
            Settings.Default.Culture = "zh-CN";
            Settings.Default.UICulture = "zh-CN";

            IApplicationController applicationController = Container.GetExportedValue<IApplicationController>();

            Assert.AreEqual(new CultureInfo("zh-CN"), CultureInfo.CurrentCulture);
            Assert.AreEqual(new CultureInfo("zh-CN"), CultureInfo.CurrentUICulture);

            applicationController.Initialize();
            applicationController.Run();

            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            mainViewModel.EnglishCommand.Execute(null);
            Assert.AreEqual(new CultureInfo("en-US"), mainViewModel.NewLanguage);

            bool settingsSaved = false;
            Settings.Default.SettingsSaving += (sender, e) =>
            {
                settingsSaved = true;
            };

            applicationController.Shutdown();
            Assert.AreEqual("en-US", Settings.Default.UICulture);
            Assert.IsTrue(settingsSaved);
        }
    }
}
