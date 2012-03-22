﻿using System.ComponentModel;

namespace FMStudio.Application.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object ShellView { get; }

        string SolutionName { get; set; }
    }
}
