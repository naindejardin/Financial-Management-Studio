using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using FMStudio.Application.Services;
using FMStudio.Application.ViewModels;
using BillList.Applications.Documents;
using BigEgg.Framework.Applications;
using System.ComponentModel;
using FMStudio.Application.Documents;
using FMStudio.Application.Views;

namespace FMStudio.Application.Controllers
{
    /// <summary>
    /// Responsible to synchronize RTF Documents with RichTextViewModels.
    /// </summary>
    [Export]
    internal class SolutionDocumentController : DocumentController
    {
        private readonly CompositionContainer container;
        private readonly IFileService fileService;
        private readonly IShellService shellService;
        private readonly MainViewModel mainViewModel;
        private SolutionViewModel solutionViewModel;
        
        [ImportingConstructor]
        public SolutionDocumentController(CompositionContainer container, IFileService fileService, IShellService shellService,
            MainViewModel mainViewModel) 
            : base(fileService)
        {
            this.container = container;
            this.fileService = fileService;
            this.shellService = shellService;
            this.mainViewModel = mainViewModel;
            this.solutionViewModel = null;
            AddWeakEventListener(mainViewModel, MainViewModelPropertyChanged);
        }


        protected override void OnDocumentAdded(IDocument document)
        {
            if ((document is SolutionDocument) && (this.solutionViewModel == null))
            {
                ISolutionView solutionView = container.GetExportedValue<ISolutionView>();
                this.solutionViewModel = new SolutionViewModel(solutionView, document as SolutionDocument);
                this.mainViewModel.DocumentViews.Add(solutionViewModel.View);
            }
        }

        protected override void OnDocumentRemoved(IDocument document)
        {
            if ((document is SolutionDocument) && (this.solutionViewModel != null))
            {
                this.mainViewModel.DocumentViews.Remove(this.solutionViewModel.View);
                this.solutionViewModel = null;
            }
        }

        protected override void OnActiveDocumentChanged(IDocument activeDocument)
        {
            if (activeDocument == null)
            {
                mainViewModel.ActiveDocumentView = null;
            }
            else
            {
                if ((activeDocument is SolutionDocument) && (this.solutionViewModel != null))
                {
                    mainViewModel.ActiveDocumentView = this.solutionViewModel.View;
                }
            }
        }


        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveDocumentView")
            {
                if (mainViewModel.ActiveDocumentView is ISolutionView)
                {
                    SolutionViewModel solutionViewModel = ViewHelper
                        .GetViewModel<SolutionViewModel>(mainViewModel.ActiveDocumentView as ISolutionView);
                    if (solutionViewModel != null)
                    {
                        fileService.ActiveDocument = solutionViewModel.Document;
                    }
                }
            }
        }
    }
}
