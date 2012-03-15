using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using BigEgg.Framework.Applications;
using BigEgg.Framework.Applications.Services;
using BillList.Applications.Documents;
using FMStudio.Application.Documents;
using FMStudio.Application.Properties;
using FMStudio.Application.Services;
using FMStudio.Application.ViewModels;
using FMStudio.Application.Views;

namespace FMStudio.Application.Controllers
{
    /// <summary>
    /// Responsible for the file related commands.
    /// </summary>
    [Export]
    internal class FileController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly IFileDialogService fileDialogService;
        private readonly IShellService shellService;
        private readonly FileService fileService;
        private readonly List<IDocumentType> documentTypes;
        private readonly RecentFileList recentSolutionList;
        //private readonly DelegateCommand newCommand;
        //private readonly DelegateCommand openCommand;
        //private readonly DelegateCommand closeCommand;
        //private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand newSolutionCommand;
        private readonly DelegateCommand openSolutionCommand;
        private readonly DelegateCommand closeSolutionCommand;
        private readonly DelegateCommand saveSolutionCommand;
        private IDocument lastActiveDocument;


        [ImportingConstructor]
        public FileController(CompositionContainer container, IMessageService messageService, IFileDialogService fileDialogService,
            IShellService shellService, FileService fileService)
        {
            this.container = container;
            this.messageService = messageService;
            this.fileDialogService = fileDialogService;
            this.shellService = shellService;
            this.fileService = fileService;
            this.documentTypes = new List<IDocumentType>();
            //this.newCommand = new DelegateCommand(NewCommand);
            //this.openCommand = new DelegateCommand(OpenCommand);
            //this.closeCommand = new DelegateCommand(CloseCommand, CanCloseCommand);
            //this.saveCommand = new DelegateCommand(SaveCommand, CanSaveCommand);
            this.newSolutionCommand = new DelegateCommand(NewSolutionCommand);
            this.openSolutionCommand = new DelegateCommand(OpenSolutionCommand);
            this.closeSolutionCommand = new DelegateCommand(CloseSolutionCommand, CanCloseSolutionCommand);
            this.saveSolutionCommand = new DelegateCommand(SaveSolutionCommand, CanSaveSolutionCommand);

            //this.fileService.NewCommand = newCommand;
            //this.fileService.OpenCommand = openCommand;
            //this.fileService.CloseCommand = closeCommand;
            //this.fileService.SaveCommand = saveCommand;
            this.fileService.NewSolutionCommand = newSolutionCommand;
            this.fileService.OpenSolutionCommand = openSolutionCommand;
            this.fileService.CloseSolutionCommand = closeSolutionCommand;
            this.fileService.SaveSolutionCommand = saveSolutionCommand;

            this.recentSolutionList = Settings.Default.RecentSolutionList;
            if (this.recentSolutionList == null) { this.recentSolutionList = new RecentFileList(); }
            this.fileService.RecentFileList = recentSolutionList;

            //AddWeakEventListener(fileService, FileServicePropertyChanged);
        }


        private ReadOnlyObservableCollection<IDocument> Documents { get { return fileService.Documents; } }

        private IDocument ActiveDocument
        {
            get { return fileService.ActiveDocument; }
            set { fileService.ActiveDocument = value; }
        }

        private SolutionDocument SolutionDoc 
        { 
            get { return fileService.SolutionDoc; }
            set { fileService.SolutionDoc = value; }
        }


        public void Initialize()
        {
            //documentTypes.Add(new SolutionDocumentType());
        }

        internal void Register(DocumentType documentType)
        {
            documentTypes.Add(documentType);
        }

        public void Shutdown()
        {
            Settings.Default.RecentSolutionList = recentSolutionList;
        }


        public IDocument NewSolution(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName)) { throw new ArgumentException("The argument fullFileName must not be null or empty."); }
            return NewSolutionCore(fullFileName);
        }

        public IDocument OpenSolution(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName)) { throw new ArgumentException("The argument fullFileName must not be null or empty."); }
            return OpenSolutionCore(fullFileName);
        }

        private void NewSolutionCommand(object commandParameter)
        {
            string fullFileName = commandParameter as string;
            if (!string.IsNullOrEmpty(fullFileName))
            {
                NewSolution(fullFileName);
            }
            else
            {
                NewSolution();
            }
        }

        private void OpenSolutionCommand(object commandParameter)
        {
            string fullFileName = commandParameter as string;
            if (!string.IsNullOrEmpty(fullFileName))
            {
                OpenSolution(fullFileName);
            }
            else
            {
                OpenSolution();
            }
        }

        private bool CanCloseSolutionCommand() { return SolutionDoc != null; }

        private void CloseSolutionCommand() { CloseSolution(); }

        private bool CanSaveSolutionCommand() { return SolutionDoc != null && SolutionDoc.Modified; }

        private void SaveSolutionCommand() { SaveSolution(); }


        internal SolutionDocument NewSolution()
        {
            // Show the new solutiion view to the user
            IDialogView newSolutionView = container.GetExportedValue<IDialogView>();
            NewSolutionViewModel newSolutionViewModel = 
                new NewSolutionViewModel(newSolutionView, "NewSolution", 
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ApplicationInfo.ProductName));
            bool? dialogResult = newSolutionViewModel.ShowDialog(shellService.ShellView);

            if (dialogResult == true)
            {
                if (!CanSolutionClose()) { return SolutionDoc; }

                SolutionDocumentType documentType = new SolutionDocumentType();
                return NewSolutionCore(Path.Combine(newSolutionViewModel.Location, newSolutionViewModel.SolutionName,
                                 newSolutionViewModel.SolutionName, documentType.FileExtension));
            }
            else
                return null;
        }

        private IDocument OpenSolution()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            FileType fileType = new FileType(documentType.Description, documentType.FileExtension);
            FileDialogResult result = fileDialogService.ShowOpenFileDialog(shellService.ShellView, fileType);
            if (result.IsValid)
            {
                if (!CanSolutionClose()) { return SolutionDoc; }

                return OpenSolutionCore(result.FileName);
            }
            return null;
        }

        private void SaveSolution()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            SaveCore(documentType, SolutionDoc);
        }

        private bool CloseSolution()
        {
            if (!CanSolutionClose()) { return false; }

            SolutionDoc = null;
            while (Documents.Any())
            {
                fileService.RemoveDocument(Documents.First());
            }
            return true;
        }

        private bool CanSolutionClose()
        {
            List<IDocument> modifiedDocuments = Documents.Where(d => d.Modified).ToList();
            if (SolutionDoc.Modified)
                modifiedDocuments.Add(SolutionDoc);
            if (!modifiedDocuments.Any()) { return true; }

            // Show the save changes view to the user
            IDialogView saveChangesView = container.GetExportedValue<IDialogView>();
            SaveChangesViewModel saveChangesViewModel = new SaveChangesViewModel(saveChangesView, modifiedDocuments);
            bool? dialogResult = saveChangesViewModel.ShowDialog(shellService.ShellView);

            if (dialogResult == true)
            {
                foreach (IDocument document in modifiedDocuments)
                {
                    SaveDocument(document);
                }
            }

            return dialogResult != null;
        }


        private SolutionDocument NewSolutionCore(string fullFilePath, FileType fileType = null)
        {
            // Check if solution already exists
            if (Directory.Exists(Path.GetDirectoryName(fullFilePath)))
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, Resources.SolutionAlreadyExisted, Path.GetFileNameWithoutExtension(fullFilePath)));

            SolutionDocumentType documentType = new SolutionDocumentType();
            SolutionDocument document = documentType.New(fullFilePath) as SolutionDocument;
            fileService.SolutionDoc = document;

            recentSolutionList.AddFile(fullFilePath);
            return document;
        }

        private SolutionDocument OpenSolutionCore(string fullFilePath)
        {
            SolutionDocumentType documentType = new SolutionDocumentType();

            SolutionDocument document = null;
            try
            {
                document = documentType.Open(fullFilePath) as SolutionDocument;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                messageService.ShowError(shellService.ShellView, 
                    string.Format(CultureInfo.CurrentCulture, 
                    Resources.CannotOpenSolution, Path.GetFileNameWithoutExtension(fullFilePath)));
                return null;
            }
            if (document != null)
            {
                SolutionDoc = document as SolutionDocument;
                recentSolutionList.AddFile(document.FullFilePath);
            }
            return document;
        }

        private void SaveCore(IDocumentType documentType, IDocument document)
        {
            try
            {
                documentType.Save(document, document.FullFilePath);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                messageService.ShowError(shellService.ShellView, 
                    string.Format(CultureInfo.CurrentCulture, Resources.CannotSaveFile, document.FullFilePath));
            }
        }

        private void SaveDocument(IDocument document)
        {
            if (document is SolutionDocument)
            {
                SolutionDocumentType documentType = new SolutionDocumentType();
                SaveCore(documentType, document);
            }
            else
            {
                IEnumerable<IDocumentType> saveTypes = documentTypes.Where(d => d.CanSave(document));
                IDocumentType documentType = saveTypes.First(d => d.FileExtension == Path.GetExtension(document.FullFilePath));
                SaveCore(documentType, document);
            }
        }
    }
}
