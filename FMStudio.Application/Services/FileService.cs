using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using FMStudio.Documents;

namespace FMStudio.Applications.Services
{
    [Export(typeof(IFileService)), Export]
    internal class FileService : DataModel, IFileService
    {
        #region Members
        private readonly ObservableCollection<IDocument> openedDocuments;
        private readonly ReadOnlyObservableCollection<IDocument> readOnlyOpenedDocuments;
        private IDocument activeDocument;
        private SolutionDocument solutionDoc;
        private RecentFileList recentSolutionList;
        private ICommand newDocumentCommand;
        private ICommand closeDocumentCommand;
        private ICommand saveDocumentCommand;
        private ICommand saveAllDocumentCommand;
        private ICommand newSolutionCommand;
        private ICommand openSolutionCommand;
        private ICommand closeSolutionCommand;
        private ICommand showSolutionCommand;
        #endregion

        [ImportingConstructor]
        public FileService()
        {
            this.openedDocuments = new ObservableCollection<IDocument>();
            this.readOnlyOpenedDocuments = new ReadOnlyObservableCollection<IDocument>(this.openedDocuments);
            SolutionName = String.Empty;
        }

        #region Properties
        public ReadOnlyObservableCollection<IDocument> OpenedDocuments { get { return this.readOnlyOpenedDocuments; } }

        public IDocument ActiveDocument
        {
            get { return this.activeDocument; }
            set
            {
                if (this.activeDocument != value)
                {
                    if (value != null && !this.openedDocuments.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Opened Documents collection.");
                    }
                    this.activeDocument = value;
                    RaisePropertyChanged("ActiveDocument");
                }
            }
        }

        public SolutionDocument SolutionDoc
        {
            get { return this.solutionDoc; }
            set 
            { 
                if (this.solutionDoc != value)
                {
                    if (value != null)
                    {
                        SolutionName = value.AliasName;
                        this.solutionDoc = value;
                        AddWeakEventListener(SolutionDoc, SolutionDocPropertyChanged);
                    }
                    else
                    {
                        SolutionName = String.Empty;
                        RemoveWeakEventListener(SolutionDoc, SolutionDocPropertyChanged);
                        this.solutionDoc = value;
                    }
                    RaisePropertyChanged("SolutionDoc");
                    RaisePropertyChanged("SolutionName");
                }
            }
        }

        public string SolutionName { get; private set; }

        public RecentFileList RecentSolutionList
        {
            get { return this.recentSolutionList; }
            set
            {
                if (this.recentSolutionList != value)
                {
                    this.recentSolutionList = value;
                    RaisePropertyChanged("RecentSolutionList");
                }
            }
        }

        public ICommand NewDocumentCommand
        {
            get { return this.newDocumentCommand; }
            set
            {
                if (this.newDocumentCommand != value)
                {
                    this.newDocumentCommand = value;
                    RaisePropertyChanged("NewDocumentCommand");
                }
            }
        }

        public ICommand CloseDocumentCommand
        {
            get { return this.closeDocumentCommand; }
            set
            {
                if (this.closeDocumentCommand != value)
                {
                    this.closeDocumentCommand = value;
                    RaisePropertyChanged("CloseDocumentCommand");
                }
            }
        }

        public ICommand SaveDocumentCommand
        {
            get { return this.saveDocumentCommand; }
            set
            {
                if (this.saveDocumentCommand != value)
                {
                    this.saveDocumentCommand = value;
                    RaisePropertyChanged("SaveDocumentCommand");
                }
            }
        }
        
        public ICommand SaveAllDocumentCommand
        {
            get { return this.saveAllDocumentCommand; }
            set
            {
                if (this.saveAllDocumentCommand != value)
                {
                    this.saveAllDocumentCommand = value;
                    RaisePropertyChanged("SaveAllDocumentCommand");
                }
            }
        }

        public ICommand NewSolutionCommand
        {
            get { return this.newSolutionCommand; }
            set
            {
                if (this.newSolutionCommand != value)
                {
                    this.newSolutionCommand = value;
                    RaisePropertyChanged("NewSolutionCommand");
                }
            }
        }

        public ICommand OpenSolutionCommand
        {
            get { return this.openSolutionCommand; }
            set
            {
                if (this.openSolutionCommand != value)
                {
                    this.openSolutionCommand = value;
                    RaisePropertyChanged("OpenSolutionCommand");
                }
            }
        }

        public ICommand CloseSolutionCommand
        {
            get { return this.closeSolutionCommand; }
            set
            {
                if (this.closeSolutionCommand != value)
                {
                    this.closeSolutionCommand = value;
                    RaisePropertyChanged("CloseSolutionCommand");
                }
            }
        }

        public ICommand ShowSolutionCommand
        {
            get { return this.showSolutionCommand; }
            set
            {
                if (this.showSolutionCommand != value)
                {
                    this.showSolutionCommand = value;
                    RaisePropertyChanged("ShowSolutionCommand");
                }
            }
        }
        #endregion

        #region Public Methods
        public void AddDocument(IDocument document)
        {
            if (document is SolutionDocument)
            {
                if (SolutionDoc == null)
                {
                    SolutionDoc = document as SolutionDocument;
                }
                else
                {
                    if (SolutionDoc != document as SolutionDocument)
                    {
                        throw new ArgumentException("Already have a solution opened");
                    }
                }
            }
            this.openedDocuments.Add(document);
            ActiveDocument = document;
        }

        public void RemoveDocument(IDocument document)
        {
            openedDocuments.Remove(document);
            ActiveDocument = null;
        }
        #endregion

        #region Private Methods
        private void SolutionDocPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AliasName")
            {
                if (SolutionDoc != null)
                {
                    SolutionName = SolutionDoc.AliasName;
                    RaisePropertyChanged("SolutionName");
                }
                else
                {
                    SolutionName = String.Empty;
                    RaisePropertyChanged("SolutionName");
                }
            }
        }
        #endregion
    }
}
