﻿using System;
using System.IO;
using System.Linq;
using BigEgg.Framework.UnitTesting;
using BillList.Applications.Documents;
using FMStudio.Application.Controllers;
using FMStudio.Application.Services;
using FMStudio.Application.Test.Documents;
using FMStudio.Application.Test.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigEgg.Framework.Applications.Services;
using System.Collections.Generic;
using FMStudio.Application.Documents;

namespace FMStudio.Application.Test.Controllers
{
    [TestClass]
    public class FileControllerTest : TestClassBase
    {
        protected override void OnTestCleanup()
        {
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "NewSolution")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "NewSolution"), true);
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "TestSolution2")))
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "TestSolution2"), true);
        }

        [TestMethod]
        public void NewSolutionViaCommandLineTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            FileService fileService = Container.GetExportedValue<FileService>();

            Assert.IsNull(fileService.SolutionDoc);

            // NewSolution is called with a fileName which might be a command line parameter.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            IDocument document = fileService.SolutionDoc;
            Assert.AreEqual(Path.Combine(
                Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"), document.FullFilePath);
            Assert.AreEqual(document, fileService.SolutionDoc);

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
        public void OpenSolutionTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            FileController fileController = Container.GetExportedValue<FileController>();
            FileService fileService = Container.GetExportedValue<FileService>();

            Assert.IsNull(fileService.SolutionDoc);

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            fileService.SolutionDoc = null;


            fileDialogService.Result = new FileDialogResult(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"),
                new FileType("FMStuido Solution Documents (*.sln)", ".sln"));
            fileService.OpenSolutionCommand.Execute(null);

            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual("FMStuido Solution Documents (*.sln)", fileDialogService.FileTypes.Last().Description);
            Assert.AreEqual(".sln", fileDialogService.FileTypes.Last().FileExtension);

            IDocument document = fileService.SolutionDoc;
            Assert.AreEqual(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"),
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
            FileService fileService = Container.GetExportedValue<FileService>();

            Assert.IsNull(fileService.SolutionDoc);

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            fileService.SolutionDoc = null;


            // Open is called with a fileName which might be a command line parameter.
            fileService.OpenSolutionCommand.Execute(Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"));
            IDocument document = fileService.SolutionDoc;
            Assert.AreEqual(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"),
                document.FullFilePath);
            Assert.AreEqual(document, fileService.SolutionDoc);

            // Call open with a fileName that has an invalid extension
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();
            fileService.OpenSolutionCommand.Execute("Document.wrongextension");
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));

            // Call open with a fileName that doesn't exist
            messageService.Clear();
            fileService.OpenSolutionCommand.Execute("2i0501fh-89f1-4197-a318-d5241135f4f6.sln");
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
            FileService fileService = Container.GetExportedValue<FileService>();

            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            SolutionDocument document = fileService.SolutionDoc;

            document.AliasName = "NewAlias";
            Assert.AreEqual("NewAlias", document.AliasName);
            fileService.SaveSolutionCommand.Execute(null); 
            fileService.OpenSolutionCommand.Execute(Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"));
            Assert.AreEqual("NewAlias", fileService.SolutionDoc.AliasName);
        }


        [TestMethod]
        public void OpenSaveSolutionTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            FileService fileService = Container.GetExportedValue<FileService>();

            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            Assert.IsFalse(fileService.SaveSolutionCommand.CanExecute(null));

            fileService.SolutionDoc = null;
            fileDialogService.Result = new FileDialogResult();

            fileDialogService.Result = new FileDialogResult(
                Path.Combine(Environment.CurrentDirectory, "NewSolution", "NewSolution.sln"),
                new FileType("FMStuido Solution Documents (*.sln)", ".sln"));
            fileService.OpenSolutionCommand.Execute(null);
            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
            Assert.IsFalse(fileService.SaveSolutionCommand.CanExecute(null));

            //  NOTE: Not test when solution document is modified.
        }
    }
}
