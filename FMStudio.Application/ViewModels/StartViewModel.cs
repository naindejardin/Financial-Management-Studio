using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Views;
using System.ComponentModel.Composition;
using FMStudio.Applications.Services;

namespace FMStudio.Applications.ViewModels
{
    [Export]
    public class StartViewModel : ViewModel<IStartView>
    {
        private readonly IFileService fileService;


        [ImportingConstructor]
        public StartViewModel(IStartView view, IFileService fileService)
            : base(view)
        {
            this.fileService = fileService;
        }


        public IFileService FileService { get { return this.fileService; } }
    }
}
