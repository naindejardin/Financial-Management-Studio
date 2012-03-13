using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using BillList.Applications.Documents;
using FMStudio.Application.Documents;

namespace FMStudio.Application.Services
{
    [Export]
    internal class FileService : DataModel, IFileService
    {
        private readonly ObservableCollection<IDocument> documents;
        private readonly ReadOnlyObservableCollection<IDocument> readOnlyDocuments;
        private IDocument activeDocument;
        private SolutionDocument solutionDoc;
        private RecentFileList recentFileList;
        //private ICommand newCommand;
        //private ICommand openCommand;
        //private ICommand closeCommand;
        //private ICommand saveCommand;
        //private ICommand saveAsCommand;
        private ICommand newSolutionCommand;
        private ICommand openSolutionCommand;
        private ICommand closeSolutionCommand;
        private ICommand saveSolutionCommand;


        [ImportingConstructor]
        public FileService()
        {
            this.documents = new ObservableCollection<IDocument>();
            this.readOnlyDocuments = new ReadOnlyObservableCollection<IDocument>(documents);
        }


        public ReadOnlyObservableCollection<IDocument> Documents { get { return this.readOnlyDocuments; } }

        public IDocument ActiveDocument
        {
            get { return this.activeDocument; }
            set
            {
                if (this.activeDocument != value)
                {
                    if (value != null && !this.documents.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Documents collection.");
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
                    this.solutionDoc = value; 
                    RaisePropertyChanged("SolutionDoc");
                }
            }
        }


        public RecentFileList RecentFileList
        {
            get { return this.recentFileList; }
            set
            {
                if (this.recentFileList != value)
                {
                    this.recentFileList = value;
                    RaisePropertyChanged("RecentFileList");
                }
            }
        }

        //public ICommand NewCommand
        //{
        //    get { return this.newCommand; }
        //    set
        //    {
        //        if (this.newCommand != value)
        //        {
        //            this.newCommand = value;
        //            RaisePropertyChanged("NewCommand");
        //        }
        //    }
        //}

        //public ICommand OpenCommand
        //{
        //    get { return this.openCommand; }
        //    set
        //    {
        //        if (this.openCommand != value)
        //        {
        //            this.openCommand = value;
        //            RaisePropertyChanged("OpenCommand");
        //        }
        //    }
        //}

        //public ICommand CloseCommand
        //{
        //    get { return this.closeCommand; }
        //    set
        //    {
        //        if (this.closeCommand != value)
        //        {
        //            this.closeCommand = value;
        //            RaisePropertyChanged("CloseCommand");
        //        }
        //    }
        //}

        //public ICommand SaveCommand
        //{
        //    get { return this.saveCommand; }
        //    set
        //    {
        //        if (this.saveCommand != value)
        //        {
        //            this.saveCommand = value;
        //            RaisePropertyChanged("SaveCommand");
        //        }
        //    }
        //}


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

        public ICommand SaveSolutionCommand
        {
            get { return this.saveSolutionCommand; }
            set
            {
                if (this.saveSolutionCommand != value)
                {
                    this.saveSolutionCommand = value;
                    RaisePropertyChanged("SaveSolutionCommand");
                }
            }
        }


        public void AddDocument(IDocument document)
        {
            SolutionDoc.AddDocument(document);
            documents.Add(document);
        }

        public void RemoveDocument(IDocument document)
        {
            SolutionDoc.RemoveDocument(document);
            documents.Remove(document);
        }
    }
}
