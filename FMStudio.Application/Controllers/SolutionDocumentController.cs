using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Services;
using FMStudio.Applications.ViewModels;
using FMStudio.Applications.Views;

namespace FMStudio.Applications.Controllers
{
    /// <summary>
    /// Responsible to synchronize Solution Documents with SolutionViewModels.
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
