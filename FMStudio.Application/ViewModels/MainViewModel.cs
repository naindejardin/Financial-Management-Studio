using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using BigEgg.Framework.Applications;
using BigEgg.Framework.Applications.Services;
using FMStudio.Application.Properties;
using FMStudio.Application.Services;
using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
{
    [Export]
    public class MainViewModel : ViewModel<IMainView>
    {
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly IFileService fileService;
        private readonly ObservableCollection<object> documentViews;
        private readonly DelegateCommand englishCommand;
        private readonly DelegateCommand chineseCommand;
        private readonly DelegateCommand aboutCommand;
        private object startView;
        private ICommand exitCommand;
        private object activeDocumentView;
        private CultureInfo newLanguage;


        [ImportingConstructor]
        public MainViewModel(IMainView view, IMessageService messageService, IShellService shellService, IFileService fileService)
            : base(view)
        {
            this.messageService = messageService;
            this.shellService = shellService;
            this.fileService = fileService;
            this.documentViews = new ObservableCollection<object>();
            this.englishCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("en-US")));
            this.chineseCommand = new DelegateCommand(() => SelectLanguage(new CultureInfo("ch-CN")));
            this.aboutCommand = new DelegateCommand(ShowAboutMessage);

            AddWeakEventListener(documentViews, DocumentViewsCollectionChanged);
        }


        public IFileService FileService { get { return this.fileService; } }

        public object StartView
        {
            get { return this.startView; }
            set
            {
                if (this.startView != value)
                {
                    this.startView = value;
                    RaisePropertyChanged("StartView");
                }
            }
        }

        public ObservableCollection<object> DocumentViews { get { return this.documentViews; } }

        public object ActiveDocumentView
        {
            get { return this.activeDocumentView; }
            set
            {
                if (this.activeDocumentView != value)
                {
                    this.activeDocumentView = value;
                    RaisePropertyChanged("ActiveDocumentView");
                }
            }
        }

        public CultureInfo NewLanguage { get { return this.newLanguage; } }


        public ICommand ExitCommand
        {
            get { return exitCommand; }
            set
            {
                if (exitCommand != value)
                {
                    exitCommand = value;
                    RaisePropertyChanged("ExitCommand");
                }
            }
        }

        public ICommand EnglishCommand { get { return englishCommand; } }

        public ICommand ChineseCommand { get { return chineseCommand; } }

        public ICommand AboutCommand { get { return aboutCommand; } }


        private void SelectLanguage(CultureInfo uiCulture)
        {
            if (!uiCulture.Equals(CultureInfo.CurrentUICulture))
            {
                messageService.ShowMessage(shellService.ShellView, Resources.RestartApplication + "\n\n" +
                    Resources.ResourceManager.GetString("RestartApplication", uiCulture));
            }
            newLanguage = uiCulture;
        }

        private void ShowAboutMessage()
        {
            messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, Resources.AboutText,
                ApplicationInfo.ProductName, ApplicationInfo.Version));
        }

        private void DocumentViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!documentViews.Any())
            {
                ViewCore.ContentViewState = ContentViewState.StartViewVisible;
            }
            else
            {
                ViewCore.ContentViewState = ContentViewState.DocumentViewVisible;
            }
        }
    }
}
