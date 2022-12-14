using Globals.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using ScottPlot;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//

namespace PresentationLayer.ViewModels;
public class MainViewModel : ObservableObject, IDisposable
{

    private IAudioController? _controller;

    private bool _playing = false;
    private bool _sourceSelected = false;
    private bool _disposedValue;
    private PeriodicTimer? _timer;
    private string _selectedDevice = String.Empty;

    private string _title = "WpfApp (MVVM)";

    public List<string> Devices => _controller?.Devices ?? new List<string>();
    public string SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            _selectedDevice = value;
            _controller?.SetDevice(value);
        }
    }
    public string AudioFilePath { get; set; } = "<select an audiofile>";
    public String AudioLength => _controller == null ? string.Empty : $"{_controller.AudioLength:hh\\:mm\\:ss}";
    public String AudioPosition => _controller == null ? string.Empty : $"{_controller.AudioPosition:hh\\:mm\\:ss}";
    public float Volume
    {
        get => _controller?.Volume ?? 0;
        set
        {
            if (_controller != null) _controller.Volume = value;
        }
    }

    public int MinFrequency
    {
        get => _controller?.MinFrequency ?? 0;
        set
        {
            if (_controller != null) _controller.MinFrequency = value;
        }
    }
    public int MaxFrequency
    {
        get => _controller?.MaxFrequency ?? 0;
        set
        {
            if (_controller != null) _controller.MaxFrequency = value;
        }
    }

    public int SelectedFilter
    {
        get => _controller?.SelectedFilter ?? 0;
        set
        {
            if (_controller != null) _controller.SelectedFilter = value;
        }
    }

    public bool Minisenabled { get; set; }

    public bool Maxisenabled { get; set; }

    public bool Filterisenabled { get; set; }
    public string RecordButtonCaption => _controller!.IsRecording ? "Stop Recording" : "Start Recording";

    
    public IAsyncRelayCommand OpenFileCommand { get; }
    public IRelayCommand PlayCommand { get; }
    public IRelayCommand PauseCommand { get; }
   

    public MainViewModel(IAudioController? controller)
    {
        EnableFilters();
        _controller = controller;
        OpenFileCommand = new AsyncRelayCommand(OpenFile);
        PlayCommand = new RelayCommand(PlaySource, () => _sourceSelected && !_playing);
        PauseCommand = new RelayCommand(StopSource, () => _playing);
       
        SelectedDevice = Devices[0];

    }

    private async Task OpenFile()
    {
        _sourceSelected = false;
        StopSource();
        _timer?.Dispose();
        string? path = GetSourcePath();
        try
        {
            if (!string.IsNullOrEmpty(path))
            {
                DisableFilters();
                await Task.Run(() => _controller?.SetSource(path)) ;
                _sourceSelected = true;
                AudioFilePath = System.IO.Path.GetFileName(path);
                OnPropertyChanged(nameof(AudioFilePath));
                OnPropertyChanged(nameof(AudioLength));
                
                PlaySource();
                
            }
        }
        catch (ArgumentException e)
        {
            _controller?.Stop();
            _playing = false;
            _sourceSelected = false;
            UpdateUiCommandsState();
            AudioFilePath = e.Message;
            OnPropertyChanged(nameof(AudioFilePath));
        }
    }

    private static string GetSourcePath()
    {
        OpenFileDialog? dialog = new()
        {
            Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*",
        };
        return dialog.ShowDialog() == true ? dialog.FileName : string.Empty;
    }

    private void UpdateUiCommandsState()
    {
        PlayCommand.NotifyCanExecuteChanged();
        DisableFilters();
        PauseCommand.NotifyCanExecuteChanged();
       
        OnPropertyChanged(nameof(RecordButtonCaption));
    }

    private async void PlaySource()
    {
       
        _controller?.Start();
        _playing = true;
        UpdateUiCommandsState();
        _timer = new(TimeSpan.FromSeconds(1));
        while (_playing && await _timer.WaitForNextTickAsync())
        {
            OnPropertyChanged(nameof(AudioPosition));
          
            if ((_controller != null) && (_controller.AudioPosition >= _controller.AudioLength)) StopSource();
        }
        EnableFilters();
    }

    private void StopSource()
    {
        _timer?.Dispose();
        _controller?.Stop();
        _playing = false;
        //EnableFilters();
        UpdateUiCommandsState();
    }

   

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _controller?.Dispose();
            }
            _controller = null;
            _disposedValue = true;
        }
    }
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void DisableFilters()
    {
        Minisenabled = false;
        Maxisenabled = false;
        Filterisenabled = false;
        OnPropertyChanged(nameof(Maxisenabled));
        OnPropertyChanged(nameof(Minisenabled));
        OnPropertyChanged(nameof(Filterisenabled));
    }

    public void EnableFilters()
    {
        Minisenabled = true;
        Maxisenabled = true;
        Filterisenabled = true;
        OnPropertyChanged(nameof(Maxisenabled));
        OnPropertyChanged(nameof(Minisenabled));
        OnPropertyChanged(nameof(Filterisenabled));
    }

  
    public string Title
    {
        get => _title;
        private set => SetProperty<string>(ref _title, value);
    }}
