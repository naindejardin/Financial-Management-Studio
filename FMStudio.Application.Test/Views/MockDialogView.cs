using System;
using System.ComponentModel.Composition;
using BigEgg.Framework.Applications;
using FMStudio.Application.ViewModels;
using FMStudio.Application.Views;

namespace FMStudio.Application.Test.Views
{
    [Export(typeof(IDialogView)), Export]
    public class MockDialogView : MockView, IDialogView
    {
        public Action<MockDialogView> ShowDialogAction { get; set; }
        public bool IsVisible { get; private set; }
        public object Owner { get; private set; }
        public NewSolutionDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<NewSolutionDialogViewModel>(this); } }


        public void ShowDialog(object owner)
        {
            Owner = owner;
            IsVisible = true;
            if (ShowDialogAction != null) { ShowDialogAction(this); }
            IsVisible = false;
            Owner = null;
        }

        public void Close()
        {
            IsVisible = false;
            Owner = null;
        }
    }
}
