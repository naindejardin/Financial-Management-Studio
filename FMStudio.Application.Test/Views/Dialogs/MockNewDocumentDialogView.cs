﻿using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Test.Views.Dialogs;
using FMStudio.Applications.ViewModels.Dialogs;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Applications.Test.Views
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

