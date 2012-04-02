using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Application.Test.Views.Dialogs;
using FMStudio.Application.ViewModels.Dialogs;
using FMStudio.Application.Views.Dialogs;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(ISaveChangesDialogView)), Export]
    public class MockSaveChangesDialogView : MockDialogViewBase, ISaveChangesDialogView
    {
        public Action<MockSaveChangesDialogView> ShowDialogAction { get; set; }
        public SaveChangesDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<SaveChangesDialogViewModel>(this); } }


        protected override void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
