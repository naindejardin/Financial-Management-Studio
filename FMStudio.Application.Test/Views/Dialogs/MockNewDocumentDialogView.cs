using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Application.Test.Views.Dialogs;
using FMStudio.Application.ViewModels.Dialogs;
using FMStudio.Application.Views.Dialogs;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(INewDocumentDialogView)), Export]
    public class MockNewDocumentDialogView : MockDialogViewBase, INewDocumentDialogView
    {
        public Action<MockNewDocumentDialogView> ShowDialogAction { get; set; }
        public NewDocumentDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<NewDocumentDialogViewModel>(this); } }


        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}

