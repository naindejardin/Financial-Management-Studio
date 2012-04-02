using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Test.Views.Dialogs;
using FMStudio.Applications.ViewModels.Dialogs;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.Test.Views
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
