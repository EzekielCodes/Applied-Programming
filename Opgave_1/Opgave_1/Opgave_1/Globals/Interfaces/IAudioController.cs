namespace Globals.Interfaces;

public interface IAudioController
{
    
    TimeSpan AudioLength { get; }
    TimeSpan AudioPosition { get; }
    public List<string> Devices { get; }
    bool IsRecording { get; }
    float Volume { get; set; }

    int MinFrequency { get; set; }

    int MaxFrequency { get; set; }

    bool Minisenabled { get; set; }

    bool Maxisenabled { get; set; }

    int SelectedFilter { get; set; }
    void Dispose();
    void SetDevice(string device);
    void SetSource(string path);
    void Start();
    
    void Stop();
    
}