using FMStudio.Applications.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMStudio.Applications.Services;
using BigEgg.Framework.UnitTesting;
using System.IO;
using System;
using System.Collections.Generic;
using FMStudio.Applications.Test.Services;
using BigEgg.Framework.Applications.Services;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using System.Linq;

namespace FMStudio.Applications.Test.ViewModels
{
    [TestClass]
    public class MainViewModelTest : TestClassBase
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
        public void SolutionDocumentViewTest()
        {
            IFileService documentManager = Container.GetExportedValue<IFileService>();
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();

            Assert.IsFalse(mainViewModel.DocumentViews.Any());
            Assert.IsNull(mainViewModel.ActiveDocumentView);

            mainViewModel.FileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});

            Assert.AreEqual(mainViewModel.DocumentViews.Single(), mainViewModel.ActiveDocumentView);
            Assert.AreEqual(1, mainViewModel.DocumentViews.Count);

            mainViewModel.FileService.CloseSolutionCommand.Execute(null);

            Assert.IsFalse(mainViewModel.DocumentViews.Any());
            Assert.IsNull(mainViewModel.ActiveDocumentView);
        }

        [TestMethod]
        public void PropertiesWithNotification()
        {
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();

            object startView = new object();
            AssertHelper.PropertyChangedEvent(mainViewModel, x => x.StartView, () => mainViewModel.StartView = startView);
            Assert.AreEqual(startView, mainViewModel.StartView);

            ICommand exitCommand = new DelegateCommand(() => { });
            AssertHelper.PropertyChangedEvent(mainViewModel, x => x.ExitCommand, () => mainViewModel.ExitCommand = exitCommand);
            Assert.AreEqual(exitCommand, mainViewModel.ExitCommand);
        }

        [TestMethod]
        public void SelectLanguageTest()
        {
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            Assert.IsNull(mainViewModel.NewLanguage);

            mainViewModel.ChineseCommand.Execute(null);
            Assert.AreEqual("zh-CN", mainViewModel.NewLanguage.Name);

            mainViewModel.EnglishCommand.Execute(null);
            Assert.AreEqual("en-US", mainViewModel.NewLanguage.Name);
        }

        [TestMethod]
        public void UpdateShellServiceDocumentNameTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            IFileService fileService = Container.GetExportedValue<IFileService>();
            IShellService shellService = Container.GetExportedValue<IShellService>();
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();

            Assert.AreEqual(String.Empty, shellService.SolutionName);

            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            Assert.AreEqual("NewSolution", shellService.SolutionName);

            //  Close solution.
            fileService.CloseSolutionCommand.Execute(null);
            Assert.AreEqual(String.Empty, shellService.SolutionName);

            //  Open the solution.
            fileDialogService.Result = new FileDialogResult(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"),
                new FileType("FMStuido Solution Documents (*.fmsln)", ".fmsln"));
            fileService.OpenSolutionCommand.Execute(null);
            Assert.AreEqual("NewSolution", shellService.SolutionName);

            AssertHelper.PropertyChangedEvent(shellService, x => x.SolutionName,
                () => fileService.SolutionDoc.AliasName = "TestSolution");
            Assert.AreEqual("TestSolution", shellService.SolutionName);
        }
    }
}
