using System.Collections.Generic;
using BillList.Applications.Documents;
using FMStudio.Application.Views;

namespace FMStudio.Application.ViewModels
{
    public class SaveChangesViewModel : DialogViewModel<IDialogView>
    {
        private readonly IEnumerable<IDocument> documents;


        public SaveChangesViewModel(IDialogView view, IEnumerable<IDocument> documents)
            : base(view)
        {
            this.documents = documents;
        }


        public IEnumerable<IDocument> Documents { get { return documents; } }
    }
}
