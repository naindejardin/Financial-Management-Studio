using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigEgg.Framework.Applications;
using FMStudio.Application.Views;
using System.ComponentModel.Composition;
using FMStudio.Application.Services;

namespace FMStudio.Application.ViewModels
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
