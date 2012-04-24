using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BigEgg.Framework.Applications.Services;
using BigEgg.Framework.UnitTesting;
using FMStudio.Applications.Controllers;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Services;
using FMStudio.Applications.Test.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMStudio.Applications.Test.Views;

namespace FMStudio.Applications.Test.Controllers
{
    [TestClass]
    public class FileControllerTest : TestClassBase
    {
        protected override void OnTestCleanup()
        {
            base.OnTestCleanup();
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "NewSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "NewSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution2"), true);
        }


        [TestMethod]
        public void NewSolutionTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            Assert.IsNull(fileService.SolutionDoc);

            //  New solution dialog canceled.
            bool showDialogCalled = false;
            MockNewSolutionDialogView saveChangesView = Container.GetExportedValue<MockNewSolutionDialogView>();
            saveChangesView.ShowDialogAction = (view) =>
            {
                showDialogCalled = true;
                view.Close();
            };
            fileService.NewSolutionCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            Assert.IsNull(fileService.SolutionDoc);

            //  New solution dialog click OK.
            showDialogCalled = false;
            saveChangesView.ShowDialogAction = (view) =>
            {
                showDialogCalled = true;
                view.ViewModel.Location = Environment.CurrentDirectory;
                view.ViewModel.SolutionName = "TestSolution";
                view.ViewModel.OKCommand.Execute(null);
            };
            fileService.NewSolutionCommand.Execute(null);
            Assert.IsTrue(showDialogCalled);
            IDocument document = fileService.SolutionDoc;
            Assert.IsNotNull(fileService.SolutionDoc);
            Assert.AreEqual(Path.Combine(
                Environment.CurrentDirectory, "TestSolution", "TestSolution.fmsln"), document.FullFilePath);
            Assert.AreEqual(document, fileService.SolutionDoc);
            Assert.IsTrue(fileService.OpenedDocuments.Any());
        }

        [TestMethod]
        public void NewSolutionViaCommandLineTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            Assert.IsNull(fileService.SolutionDoc);

            // NewSolution is called with a path and solution name which might be a command line parameter.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            IDocument document = fileService.SolutionDoc;
            Assert.IsNotNull(fileService.SolutionDoc);
            Assert.AreEqual(Path.Combine(
                Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"), document.FullFilePath);
            Assert.AreEqual(document, fileService.SolutionDoc);
            Assert.IsTrue(fileService.OpenedDocuments.Any());

            // Call NewSolution with a fileName that has already exist.
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
        }

        [TestMethod]
        public void NewSolutionExceptionsTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();

            AssertHelper.ExpectedException<ArgumentException>(() => fileController.NewSolution(null));
        }


        [TestMethod]
        public void CloseSolutionTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            IDocument document = fileService.SolutionDoc;
            Assert.IsNotNull(fileService.SolutionDoc);
            Assert.IsTrue(fileService.OpenedDocuments.Any());

            fileService.CloseSolutionCommand.Execute(null);
            Assert.IsNull(fileService.SolutionDoc);
            Assert.IsFalse(fileService.OpenedDocuments.Any());
        }


        [TestMethod]
        public void OpenSolutionTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            Assert.IsNull(fileService.SolutionDoc);

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            fileService.CloseSolutionCommand.Execute(null);


            fileDialogService.Result = new FileDialogResult(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"),
                new FileType("FMStuido Solution Documents (*.fmsln)", ".fmsln"));
            fileService.OpenSolutionCommand.Execute(null);

            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual("FMStuido Solution Documents (*.fmsln)", fileDialogService.FileTypes.Last().Description);
            Assert.AreEqual(".fmsln", fileDialogService.FileTypes.Last().FileExtension);

            IDocument document = fileService.SolutionDoc;
            Assert.AreEqual(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"),
                document.FullFilePath);
            Assert.AreEqual(document, fileService.SolutionDoc);

            // Open the same file again -> It's not opened again.
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();
            fileService.OpenSolutionCommand.Execute(null);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));

            // Now the user cancels the OpenFileDialog box
            fileDialogService.Result = new FileDialogResult();
            document = fileService.SolutionDoc;
            fileService.OpenSolutionCommand.Execute(null);
            Assert.AreEqual(document, fileService.SolutionDoc);
        }

        [TestMethod]
        public void OpenSolutionViaCommandLineTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            Assert.IsNull(fileService.SolutionDoc);

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            fileService.CloseSolutionCommand.Execute(null);


            // Open is called with a fileName which might be a command line parameter.
            fileService.OpenSolutionCommand.Execute(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"));
            IDocument document = fileService.SolutionDoc;
            Assert.AreEqual(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"),
                document.FullFilePath);
            Assert.AreEqual(document, fileService.SolutionDoc);

            // Call open with a fileName that has an invalid extension
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();
            fileService.OpenSolutionCommand.Execute(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.wrongextension"));
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));

            // Call open with a fileName that doesn't exist
            messageService.Clear();
            fileService.OpenSolutionCommand.Execute(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "2i0501fh-89f1-4197-a318-d5241135f4f6.fmsln"));
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
        }

        [TestMethod]
        public void OpenSolutionExceptionsTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            AssertHelper.ExpectedException<ArgumentException>(() => fileController.OpenSolution(null));
        }


        [TestMethod]
        public void SaveSolutionTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});

            SolutionDocument document = fileService.SolutionDoc;
            document.AliasName = "NewAlias";
            Assert.AreEqual("NewAlias", document.AliasName);
            fileService.ActiveDocument = document;

            fileService.SaveDocumentCommand.Execute(document);
            fileService.CloseSolutionCommand.Execute(null);
            fileService.OpenSolutionCommand.Execute(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"));
            Assert.AreEqual("NewAlias", fileService.SolutionDoc.AliasName);
        }

        [TestMethod]
        public void OpenSaveSolutionTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            Assert.IsFalse(fileService.SaveDocumentCommand.CanExecute(null));

            fileService.CloseSolutionCommand.Execute(null);

            fileDialogService.Result = new FileDialogResult(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"),
                new FileType("FMStuido Solution Documents (*.fmsln)", ".fmsln"));
            fileService.OpenSolutionCommand.Execute(null);
            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
            Assert.IsFalse(fileService.SaveDocumentCommand.CanExecute(null));

            //  Test when solution document is modified.
            SolutionDocument document = fileService.SolutionDoc;
            document.AliasName = "NewAlias";
            Assert.AreEqual("NewAlias", document.AliasName);
            fileService.ActiveDocument = document;

            fileService.SaveDocumentCommand.Execute(document);
            fileService.CloseSolutionCommand.Execute(null);
            fileService.OpenSolutionCommand.Execute(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"));
            Assert.AreEqual("NewAlias", fileService.SolutionDoc.AliasName);
        }


        [TestMethod]
        public void CloseSolutionDocumentAndShowTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            fileService.ActiveDocument = fileService.SolutionDoc;

            fileService.CloseDocumentCommand.Execute(null);
            Assert.IsNull(fileService.ActiveDocument);
            Assert.IsNotNull(fileService.SolutionDoc);
            Assert.IsFalse(fileService.OpenedDocuments.Any());

            //  Show solution document
            fileService.ShowSolutionCommand.Execute(null);
            Assert.IsTrue(fileService.OpenedDocuments.Any());
            Assert.AreEqual(fileService.ActiveDocument, fileService.SolutionDoc);
        }

        [TestMethod]
        public void SaveAllDocumentTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});

            SolutionDocument document = fileService.SolutionDoc;
            document.AliasName = "NewAlias";
            Assert.AreEqual("NewAlias", document.AliasName);

            fileService.SaveAllDocumentCommand.Execute(null);
            fileService.CloseSolutionCommand.Execute(null);
            fileService.OpenSolutionCommand.Execute(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.fmsln"));
            Assert.AreEqual("NewAlias", fileService.SolutionDoc.AliasName);
        }
    }
}
