using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Test.Views.Dialogs;
using FMStudio.Applications.ViewModels.Dialogs;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.Test.Views
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
