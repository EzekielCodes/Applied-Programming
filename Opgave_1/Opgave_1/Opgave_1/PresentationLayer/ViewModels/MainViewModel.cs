using Globals.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;

namespace PresentationLayer.ViewModels;
public class MainViewModel : ObservableObject
{
    private readonly ILogic _logic;

    private string _title = "WpfApp (MVVM)";

    // binding properties

    public string Title
    {
        get => _title;
        private set => SetProperty<string>(ref _title, value);
    }

    public string Time => DateTime.Now.ToLongTimeString();

    public IRelayCommand UpdateCommand { get; }

    public MainViewModel(ILogic logic)
    {
        _logic = logic;
        UpdateCommand = new RelayCommand(NotifyTimeChanged);
    }

    private void NotifyTimeChanged()
    {
        OnPropertyChanged(nameof(Time));
    }
}
