using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using BigEgg.Framework.Applications;
using BigEgg.Framework.Applications.Services;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Properties;
using FMStudio.Applications.Services;
using FMStudio.Applications.ViewModels.Dialogs;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.Controllers
{
    /// <summary>
    /// Responsible for the file related commands.
    /// </summary>
    [Export]
    internal class FileController : Controller
    {
        #region Members
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly IFileDialogService fileDialogService;
        private readonly IShellService shellService;
        private readonly FileService fileService;
        private readonly List<IDocumentType> documentTypes;
        private readonly RecentFileList recentSolutionList;
        private readonly DelegateCommand newDocumentCommand;
        private readonly DelegateCommand closeDocumentCommand;
        private readonly DelegateCommand saveDocumentCommand;
        private readonly DelegateCommand saveAllDocumentCommand;
        private readonly DelegateCommand newSolutionCommand;
        private readonly DelegateCommand openSolutionCommand; 
        private readonly DelegateCommand closeSolutionCommand;
        private readonly DelegateCommand showSolutionCommand;
        private IDocument lastActiveDocument;
        #endregion

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

            this.newDocumentCommand = new DelegateCommand(NewDocumentCommand, CanNewDocumentCommand);
            this.closeDocumentCommand = new DelegateCommand(CloseDocumentCommand, CanCloseDocumentCommand);
            this.saveDocumentCommand = new DelegateCommand(SaveDocumentCommand, CanSaveDocumentCommand);
            this.saveAllDocumentCommand = new DelegateCommand(SaveAllDocumentCommand, CanSaveAllDocumentCommand);
            this.newSolutionCommand = new DelegateCommand(NewSolutionCommand);
            this.openSolutionCommand = new DelegateCommand(OpenSolutionCommand);
            this.closeSolutionCommand = new DelegateCommand(CloseSolutionCommand, CanCloseSolutionCommand);
            this.showSolutionCommand = new DelegateCommand(ShowSolutionCommand, CanShowSolutionCommand);

            this.fileService.NewDocumentCommand = this.newDocumentCommand;
            this.fileService.CloseDocumentCommand = this.closeDocumentCommand;
            this.fileService.SaveDocumentCommand = this.saveDocumentCommand;
            this.fileService.SaveAllDocumentCommand = this.saveAllDocumentCommand;
            this.fileService.NewSolutionCommand = this.newSolutionCommand;
            this.fileService.OpenSolutionCommand = this.openSolutionCommand;
            this.fileService.CloseSolutionCommand = this.closeSolutionCommand;
            this.fileService.ShowSolutionCommand = this.showSolutionCommand;

            this.recentSolutionList = Settings.Default.RecentSolutionList;
            if (this.recentSolutionList == null) { this.recentSolutionList = new RecentFileList(); }
            this.fileService.RecentSolutionList = recentSolutionList;

            AddWeakEventListener(fileService, FileServicePropertyChanged);
        }

        #region Properties
        private ReadOnlyObservableCollection<IDocument> OpenedDocuments { get { return fileService.OpenedDocuments; } }

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
        #endregion

        #region Public Methods
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

        public bool CloseSolution()
        {
            if (!CanDocumentsClose(OpenedDocuments)) { return false; }

            SolutionDoc = null;
            ActiveDocument = null;
            while (OpenedDocuments.Any())
            {
                fileService.RemoveDocument(OpenedDocuments.First());
            }
            return true;
        }
        #endregion

        #region Command Implement
        #region Command Line Methods
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
        #endregion

        #region Command Methods
        private bool CanDocumentsClose(IEnumerable<IDocument> documentsToClose)
        {
            List<IDocument> modifiedDocuments = documentsToClose.Where(d => d.Modified).ToList();
            if (!modifiedDocuments.Any()) { return true; }

            // Show the save changes view to the user
            ISaveChangesDialogView saveChangesView = container.GetExportedValue<ISaveChangesDialogView>();
            SaveChangesDialogViewModel saveChangesViewModel = new SaveChangesDialogViewModel(saveChangesView, modifiedDocuments);
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

        private bool CanNewDocumentCommand() { return SolutionDoc != null; }

        private void NewDocumentCommand() { NewDocument(); }

        private bool CanCloseDocumentCommand() { return ActiveDocument != null; }

        private void CloseDocumentCommand() { CloseDocument(ActiveDocument); }

        private bool CanSaveDocumentCommand() { return ActiveDocument != null && ActiveDocument.Modified; }

        private void SaveDocumentCommand() { SaveDocument(ActiveDocument); }

        private bool CanSaveAllDocumentCommand() { return OpenedDocuments.Where(d => d.Modified).Any(); }

        private void SaveAllDocumentCommand() { SaveAllDocument(); }
        
        private void NewSolutionCommand(object commandParameter)
        {
            IList<string> fileInfo = commandParameter as IList<string>;

            if ((fileInfo != null) && (fileInfo.Count == 2)
                && (!string.IsNullOrEmpty(fileInfo[0])) 
                && (!string.IsNullOrEmpty(fileInfo[1])))
            {
                SolutionDocumentType documentType = new SolutionDocumentType();
                string fullFileName = Path.Combine(fileInfo[0], fileInfo[1],
                                 fileInfo[1] + documentType.FileExtension);
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

        private bool CanShowSolutionCommand() { return SolutionDoc != null; }

        private void ShowSolutionCommand() { ShowSolution(); }
        #endregion

        #region Command Implement Mehods
        private IDocument NewDocument()
        {
            // Show the new document view to the user
            INewDocumentDialogView newDocumentView = container.GetExportedValue<INewDocumentDialogView>();
            NewDocumentDialogViewModel newDocumentViewModel = 
                new NewDocumentDialogViewModel(
                    newDocumentView, documentTypes.Where(d=> d.CanNew()).Select(d => d));

            bool? dialogResult = newDocumentViewModel.ShowDialog(shellService.ShellView);

            if (dialogResult == true)
            {
                return NewCore(Path.Combine(Path.GetDirectoryName(SolutionDoc.FullFilePath), 
                    newDocumentViewModel.FileName + newDocumentViewModel.SelectDocumentType.FileExtension));
            }
            else
                return null;
        }

        private void SaveDocument(IDocument document)
        {
            IDocumentType documentType;

            if (document is SolutionDocument)
            {
                documentType = new SolutionDocumentType();
            }
            else
            {
                IEnumerable<IDocumentType> saveTypes = documentTypes.Where(d => d.CanSave(document));
                documentType = saveTypes.First(d => d.FileExtension == Path.GetExtension(document.FullFilePath));
            }
            SaveCore(documentType, document);
        }

        private void SaveAllDocument()
        {
            List<IDocument> modifiedDocuments = OpenedDocuments.Where(d => d.Modified).ToList();
            if (!modifiedDocuments.Any()) { return; }

            // Show the save changes view to the user
            foreach (IDocument document in modifiedDocuments)
            {
                SaveDocument(document);
            }
        }

        private bool CloseDocument(IDocument document)
        {
            if (!CanDocumentsClose(new IDocument[] { document })) { return false; }

            if (ActiveDocument == document)
            {
                ActiveDocument = null;
            }
            fileService.RemoveDocument(document);
            return true;
        }

        internal SolutionDocument NewSolution()
        {
            // Show the new solutiion view to the user
            INewSolutionDialogView newSolutionView = container.GetExportedValue<INewSolutionDialogView>();
            if (!Directory.Exists(Settings.Default.DefaultNewSolutionLocation))
            {
                Settings.Default.DefaultNewSolutionLocation
                    = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                        ApplicationInfo.ProductName);
                Directory.CreateDirectory(Settings.Default.DefaultNewSolutionLocation);
            }

            NewSolutionDialogViewModel newSolutionViewModel =
                new NewSolutionDialogViewModel(newSolutionView);
            bool? dialogResult = newSolutionViewModel.ShowDialog(shellService.ShellView);

            if (dialogResult == true)
            {
                if (!CloseSolution()) { return SolutionDoc; }

                SolutionDocumentType documentType = new SolutionDocumentType();
                return NewSolutionCore(Path.Combine(newSolutionViewModel.Location, newSolutionViewModel.SolutionName,
                                 newSolutionViewModel.SolutionName + documentType.FileExtension));
            }
            else
                return null;
        }

        internal IDocument OpenSolution()
        {
            SolutionDocumentType documentType = new SolutionDocumentType();
            FileType fileType = new FileType(documentType.Description, documentType.FileExtension);
            FileDialogResult result = fileDialogService.ShowOpenFileDialog(shellService.ShellView, fileType);

            if (result.IsValid)
            {
                return OpenSolutionCore(result.FileName);
            }
            return null;
        }

        private void ShowSolution()
        {
            List<IDocument> solutionDocuments = OpenedDocuments.Where(d => d is SolutionDocument).ToList();
            if (solutionDocuments.Any())
            {
                return;
            }
            else
            {
                this.fileService.AddDocument(this.fileService.SolutionDoc);
            }

            this.fileService.ActiveDocument = this.fileService.SolutionDoc;
        }
        #endregion

        #region Command Core Methods
        private SolutionDocument NewSolutionCore(string fullFilePath)
        {
            if (String.IsNullOrWhiteSpace(Path.GetDirectoryName(fullFilePath)))
            {
                this.messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, Resources.NewSolutionPathInvalid, fullFilePath));
                return null;
            }

            // Check if solution already exists
            if (Directory.Exists(Path.GetDirectoryName(fullFilePath)))
            {
                this.messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, Resources.SolutionAlreadyExisted, Path.GetFileNameWithoutExtension(fullFilePath)));
                return null;
            }

            SolutionDocumentType documentType = new SolutionDocumentType();
            if (fullFilePath
                .Substring(fullFilePath.Length - documentType.FileExtension.Length)
                .CompareTo(documentType.FileExtension) != 0)
            {
                fullFilePath = fullFilePath + documentType.FileExtension;
            }

            SolutionDocument document = documentType.New(fullFilePath) as SolutionDocument;

            this.recentSolutionList.AddFile(fullFilePath);
            fileService.AddDocument(document);
            return this.fileService.SolutionDoc;
        }

        private SolutionDocument OpenSolutionCore(string fullFilePath)
        {
            if ((SolutionDoc != null) && (SolutionDoc.FullFilePath == fullFilePath))
            {
                this.messageService.ShowMessage(shellService.ShellView,
                    string.Format(CultureInfo.CurrentCulture,
                    Resources.SolutionAlreadyOpened, Path.GetFileNameWithoutExtension(fullFilePath)));
                return null;
            }

            if (!CloseSolution()) { return SolutionDoc; }

            SolutionDocumentType documentType = new SolutionDocumentType();
            SolutionDocument document = null;
            try
            {
                document = documentType.Open(fullFilePath) as SolutionDocument;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                this.messageService.ShowError(shellService.ShellView, 
                    string.Format(CultureInfo.CurrentCulture, 
                    Resources.CannotOpenSolution, Path.GetFileNameWithoutExtension(fullFilePath)));
                return null;
            }
            if (document != null)
            {
                fileService.AddDocument(document);
                this.recentSolutionList.AddFile(document.FullFilePath);
            }
            return document;
        }

        private IDocument NewCore(string fullFilePath)
        {
            IDocument document = null;
            try
            {
                IDocumentType documentType = this.documentTypes.First(d => d.FileExtension == Path.GetExtension(fullFilePath));
                if (documentType == null)
                {
                    throw new ArgumentException("documentType is not an item of the DocumentTypes collection.");
                }
                document = documentType.New(fullFilePath);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                this.messageService.ShowError(shellService.ShellView,
                    string.Format(CultureInfo.CurrentCulture,
                    Resources.CannotNewSolution, Path.GetFileNameWithoutExtension(fullFilePath)));
                return null;
            }
            this.fileService.AddDocument(document);
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
        #endregion
        #endregion

        #region Private Methods
        private void FileServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveDocument")
            {
                if (lastActiveDocument != null) { RemoveWeakEventListener(lastActiveDocument, ActiveDocumentPropertyChanged); }

                lastActiveDocument = fileService.ActiveDocument;

                if (lastActiveDocument != null) { AddWeakEventListener(lastActiveDocument, ActiveDocumentPropertyChanged); }

                UpdateCommands();
            }
            else if (e.PropertyName == "SolutionDoc")
            {
                UpdateCommands();
            }
        }

        private void ActiveDocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Modified")
            {
                UpdateCommands();
            }
        }

        private void UpdateCommands()
        {
            newDocumentCommand.RaiseCanExecuteChanged();
            closeDocumentCommand.RaiseCanExecuteChanged();
            saveDocumentCommand.RaiseCanExecuteChanged();
            saveAllDocumentCommand.RaiseCanExecuteChanged();
            closeSolutionCommand.RaiseCanExecuteChanged();
            showSolutionCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
