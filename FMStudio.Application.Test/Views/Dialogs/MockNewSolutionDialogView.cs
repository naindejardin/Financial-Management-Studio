using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Application.Test.Views.Dialogs;
using FMStudio.Application.ViewModels.Dialogs;
using FMStudio.Application.Views.Dialogs;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(INewSolutionDialogView)), Export]
    public class MockNewSolutionDialogView : MockDialogViewBase, INewSolutionDialogView
    {
        public Action<MockNewSolutionDialogView> ShowDialogAction { get; set; }
        public NewSolutionDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<NewSolutionDialogViewModel>(this); } }


        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
