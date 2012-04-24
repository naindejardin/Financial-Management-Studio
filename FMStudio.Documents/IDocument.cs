using System.ComponentModel;

namespace FMStudio.Documents
{
    public interface IDocument : INotifyPropertyChanged
    {
        IDocumentType DocumentType { get; }

        string FullFilePath { get; set; }

        bool Modified { get; set; }
    }
}
