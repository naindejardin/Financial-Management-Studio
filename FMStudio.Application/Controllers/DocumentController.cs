﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Services;

namespace FMStudio.Applications.Controllers
{
    /// <summary>
    /// Responsible to synchronize the Documents with the UI Elements that represent these Documents.
    /// </summary>
    internal abstract class DocumentController : Controller
    {
        private readonly IFileService fileService;
        
        
        protected DocumentController(IFileService fileService)
        {
            if (fileService == null) { throw new ArgumentNullException("fileService"); }
            
            this.fileService = fileService;
            AddWeakEventListener(fileService, FileServicePropertyChanged);
            AddWeakEventListener(fileService.OpenedDocuments, DocumentsCollectionChanged);
        }


        protected abstract void OnDocumentAdded(IDocument document);

        protected abstract void OnDocumentRemoved(IDocument document);

        protected abstract void OnActiveDocumentChanged(IDocument activeDocument);

        private void FileServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveDocument") { OnActiveDocumentChanged(fileService.ActiveDocument); }
        }

        private void DocumentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnDocumentAdded(e.NewItems.Cast<Document>().Single());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnDocumentRemoved(e.OldItems.Cast<Document>().Single());
                    break;
                default:
                    throw new NotSupportedException("This kind of documents collection change is not supported.");
            }
        }
    }
}
