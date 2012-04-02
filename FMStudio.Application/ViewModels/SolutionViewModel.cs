using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigEgg.Framework.Applications;
using FMStudio.Applications.Views;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Services;

namespace FMStudio.Applications.ViewModels
{
    public class SolutionViewModel : ViewModel<ISolutionView>
    {
        private readonly SolutionDocument document;
        private bool isVisible;


        public SolutionViewModel(ISolutionView view, SolutionDocument document)
            : base(view)
        {
            this.document = document;
        }


        public SolutionDocument Document { get { return document; } }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged("IsVisible");
                }
            }
        }
    }
}
