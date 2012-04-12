﻿using System.ComponentModel.Composition;
using System.Windows;
using FMStudio.Applications.Views.Dialogs;

namespace FMStudio.Presentation.Views.Dialogs
{
    [Export(typeof(ISaveChangesDialogView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SaveChangesWindow : Window, ISaveChangesDialogView
    {
        public SaveChangesWindow()
        {
            InitializeComponent();
        }


        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }
    }
}