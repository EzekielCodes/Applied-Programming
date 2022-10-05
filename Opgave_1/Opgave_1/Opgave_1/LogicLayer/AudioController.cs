using AudioTools.Implementation;
using AudioTools.Interfaces;
using Globals.Interfaces;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;


namespace LogicLayer;
public class AudioController : IAudioController
{
    private readonly IAudioFileReaderFactory _audioFileReaderFactory;
    private readonly IAudioPlayerFactory _audioPlayerFactory;
    private readonly IMp3FileWriterFactory _mp3FileWriterFactory;

    // max delay is calculated based an a maximum samplerate of 48000Hz and the constant MaxEchoDelay property. 
    private readonly IDelayLine<AudioSampleFrame> _delayLine;
    private IAudioFileReader? _reader;
    private IAudioPlayer? _player;
    private IMp3FileWriter? _recorder;
    private string _currentDevice;
    private bool _playing = false;
    public bool IsRecording { get; private set; } = false;
    private TimeSpan _delay = TimeSpan.FromMilliseconds(0);
    private float _volume = 50f;
    private int _minFreq;
    private int _maxFreq;
    private int _indexHigh;
    private int _indexLow;

    private int _selectedIndex;
    private bool _disposedValue;
    private double _frequencyResolutie;
    private Complex[] _complexArrayLeft;
    private Complex[] _complexArrayRight;
    private int _range = 0;
    public List<string> Devices => (new List<string> { "Default" }).Concat(AudioSystem.OutputDeviceCapabilities.Select(c => c.ProductName)).ToList();

    public TimeSpan AudioLength => _reader?.TimeLength ?? new TimeSpan();
    public TimeSpan AudioPosition => _reader?.TimePosition ?? new TimeSpan();

    public float Volume
    {
        get => (int)(_volume);
        set
        {
            _volume = value < 0f ? 0f : value > 100f ? 100f : value;
            if (_player == null) return;
            _player.Volume = _volume / 100f;
        }
    }

    public int MinFrequency 
    { 
        get => _minFreq;
        set
        {
            if(_maxFreq != value && value < _maxFreq)
            {
                _minFreq = value;
            }
        }
    }
    public int MaxFrequency
    {
        get => _maxFreq;
        set
        {
            if (_minFreq != value && value > _minFreq)
            {
                _maxFreq = value;
            }
        }}
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => _selectedIndex = value;
    }

    public  TimeSpan MaxEchoDelay => TimeSpan.FromSeconds(1);

    public TimeSpan EchoDelay
    {
        get => TimeSpan.FromMilliseconds(_delayLine.Delay * 1000 / (_reader?.SampleRate ?? 44100));
        set
        {
            _delay = (value <= MaxEchoDelay) ? value : MaxEchoDelay;
            _delayLine.Delay = TimeSpanToFrames(_delay);
        }
    }

    

    public AudioController(IAudioFileReaderFactory audioFileReaderFactory, IAudioPlayerFactory audioPlayerFactory, IMp3FileWriterFactory mdlFileWriterFactory, IDelaylineFactory delayLineFactory)
    {
        _currentDevice = Devices[0];
        _audioFileReaderFactory = audioFileReaderFactory;
        _audioPlayerFactory = audioPlayerFactory;
        _mp3FileWriterFactory = mdlFileWriterFactory;
        _delayLine = delayLineFactory.Create<AudioSampleFrame>((int)(MaxEchoDelay.TotalSeconds * 48000));
    }

    private int TimeSpanToFrames(TimeSpan interval)
    {
        return (int)interval.TotalMilliseconds * (_reader?.SampleRate ?? 0) / 1000;
    }

    public void SetSource(string path)
    {
        StopRecording();
        _playing = false;
        _reader = _audioFileReaderFactory.Create(path);
        CalculateSampleRate();
        ReadSamples();
        Calculateblok();
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        _player = _audioPlayerFactory.Create(_currentDevice, _reader!.SampleRate);
        _player.Volume = _volume;
        _delayLine.Clear();
        _player.SampleFramesNeeded += Player_OnSampleFramesNeeded;

    }

    public void SetDevice(string device)
    {
        _currentDevice = device;
        if (_reader != null)
        {
            var oldplayer = _player;
            oldplayer!.SampleFramesNeeded -= Player_OnSampleFramesNeeded;
            CreatePlayer();
            oldplayer?.Dispose();
        }
        if (_playing) _player?.Start();
    }

    
    /// <summary>
    /// Hier vraagt de player een bepaalde aantal frame
    /// </summary>
    /// <param name="frameCount"></param>
    private void Player_OnSampleFramesNeeded(int frameCount)
    {
        
        for (int i = 0; i < frameCount; i++)
        {
            var sampleFrame = CalculateNextFrame();
            _range++;
            if (IsRecording) _recorder!.WriteSampleFrame(sampleFrame);
            _player?.WriteSampleFrame(sampleFrame);

        }
    }

    /// <summary>
    /// Hier wordt de 2 complexe array opgevuld met samples
    /// De methode ReadSampleFrame geeft een sampleframe terug
    /// </summary>
    private  void ReadSamples()
    {
        for (int i = 0; i < _complexArrayLeft.Length; i++)
        {

            var x = _reader.ReadSampleFrame();
            _complexArrayLeft[i] = x.Left;
            _complexArrayRight[i] = x.Right;
        }
    }

    
    /// <summary>
    /// Hier wordt de volgende sample frame berekend
    /// Een audioSampleframe wordt terug gegegven aan de player
    /// </summary>
    /// <returns></returns>
    private AudioSampleFrame CalculateNextFrame()
    { 
        return new AudioSampleFrame((float)_complexArrayLeft[_range].Real, (float)_complexArrayRight[_range].Real);
    }

    public void Start()
    {
        if (_player == null) return;
       
        _player.Start();
       
        _playing = true;
    }

    public void StartRecording()
    {
        const string filePath = "fragment.mp3";
        if (!_playing) return;
        _recorder = _mp3FileWriterFactory.Create(filePath, _reader!.SampleRate);
        IsRecording = true;
    }

    public void StopRecording()
    {
        if (!IsRecording) return;
        _recorder!.Close();
        _recorder = null;
        IsRecording = false;
    }

    public void Stop()
    {
        _player?.Stop();
        StopRecording();
        _playing = false;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Stop();
                _reader?.Dispose();
                _player?.Dispose();
            }
            _reader = null;
            _player = null;
            _disposedValue = true;
        }}

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Hier wordt de sampleRate berekent
    /// + de complexarrays worden hier initialiseert
    /// </summary>
    public void CalculateSampleRate()
    {
        double TotalsampleRate = _reader.TimeLength.TotalSeconds * _reader.SampleRate;
        int sampleRate = (int)TotalsampleRate;
        int ExpectedSize = GetValuePow(sampleRate, 1);

        _complexArrayLeft = new Complex[ExpectedSize];
        _complexArrayRight = new Complex[ExpectedSize];
    }

    /// <summary>
    /// Hier wordt gechekt welke Filter gekozen is en eventueel de filter
    /// funtie uitvoeren
    /// </summary>
    public void Calculateblok()
    {
        if (_selectedIndex == 1)
        {
            CalculateFreqResolutie();
            FFTransform(_complexArrayLeft);
            FFTransform(_complexArrayRight);
            _indexHigh = Searchindex(_maxFreq, _frequencyResolutie);
            _indexLow = GetLowIndex(_frequencyResolutie, _minFreq);
            BandPass(_complexArrayLeft, _indexHigh, _indexLow);
            BandPass(_complexArrayRight, _indexHigh, _indexLow);
            IFFTransform(_complexArrayLeft);
            IFFTransform(_complexArrayRight);
        }
        else if(_selectedIndex == 2)
        {
            CalculateFreqResolutie();
            FFTransform(_complexArrayLeft);
            FFTransform(_complexArrayRight);
            int IndexHigh = Searchindex(_maxFreq, _frequencyResolutie);
            int indexLow = GetLowIndex(_frequencyResolutie, _minFreq);
            BandStop(_complexArrayLeft, IndexHigh, indexLow);
            BandStop(_complexArrayRight, IndexHigh, indexLow);
            IFFTransform(_complexArrayLeft);
            IFFTransform(_complexArrayRight);
        }}

    /// <summary>
    /// Hier wordt de FrequencyResolutie berekent
    /// </summary>
    public void CalculateFreqResolutie()
    {
        int samplerate = _reader.SampleRate;
        int n = BerekenN(samplerate);

        _frequencyResolutie = (double)samplerate / _complexArrayLeft.Length;
        int bloksize = (int)Math.Ceiling(_frequencyResolutie);
    }

    /// <summary>
    /// 5 = 5hz veresite nauwkeurigheid
    /// </summary>
    /// <param name="samplerate"></param>
    /// <returns></returns>
    public int BerekenN(int samplerate)
    {
        int mod = samplerate / 5;
        int n = GetValuePow(mod, 1);
        return n;
    }

    /// <summary>
    /// berekenen moet in de macht van 2^x zijn(FFT laat alleen macht to 2 toe..)
    /// </summary>
    /// <param name="sampleRate"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public int GetValuePow(int sampleRate, int x)
    {  
        int pow = (int)Math.Pow(2, x);

        if (pow > sampleRate)
        {
            return pow;
        }
        x++;
        return GetValuePow(sampleRate, x);
    }

    /// <summary>
    /// hier wordt FFT gedaan
    /// </summary>
    /// <param name="complexarray"></param>
    public static void FFTransform(Complex[] complexarray) => Fourier.Forward(complexarray);

    /// <summary>
    /// Hier wordt de index van het BindHigh berekent
    /// </summary>
    /// <param name="filterHz"></param>
    /// <param name="opgeslist"></param>
    /// <returns></returns>
    public static int Searchindex(int filterHz, double opgeslist)
    {
        int index = 0;
        index = (int)Math.Floor(filterHz / opgeslist);
        
        return index;
    }

    /// <summary>
    /// Hier wordt de index van het bindLow berekent
    /// </summary>
    /// <param name="waarde"></param>
    /// <param name="low"></param>
    /// <returns></returns>
    public static int GetLowIndex(double waarde, int low)
    {
        double value = waarde;
        int indexLow = 0;
        while (waarde < low)
        {
            indexLow++;
            waarde += value;
        }

        return indexLow;
    }

    /// <summary>
    /// BandStop frequency binnen een bepaalde range wegfilteren
    /// </summary>
    /// <param name="complex"></param>
    /// <param name="indexHigh"></param>
    /// <param name="indexLow"></param>
    public static void BandStop(Complex[] complex, int indexHigh, int indexLow)
    {

        for (int i = indexLow ; i < indexHigh + 1; i++)
        {
                complex[i] = new Complex(0, 0);
        }

        int indexHighInverse = complex.Length - indexHigh;
        int indexLowInverse = complex.Length - indexLow;
        for (int i = indexHighInverse; i < indexLowInverse; i++)
        {
            complex[i] = new Complex(0, 0);
        }
    }

    /// <summary>
    /// BandPass frequency binnen een bepaalde range laten
    /// </summary>
    /// <param name="complex"></param>
    /// <param name="indexHigh"></param>
    /// <param name="indexLow"></param>
    public static void BandPass(Complex[] complex, int indexHigh, int indexLow)
    {
        Complex[] ComplexKopie = new Complex[complex.Length];
        for (int i = 0; i < complex.Length; i++)
            ComplexKopie[i] = complex[i];

        for (int i = 0; i < complex.Length; i++)
            complex[i] = new Complex(0, 0);

        for (int i = indexLow; i < indexHigh + 1; i++)
        {
            if (ComplexKopie[i].Real > 0)
                complex[i] = ComplexKopie[i];
        }

        int indexHighInverse = complex.Length - indexHigh;
        int indexLowInverse = complex.Length - indexLow;
        for (int i = indexHighInverse; i < indexLowInverse; i++)
        {
            if (ComplexKopie[i].Real > 0)
                complex[i] = ComplexKopie[i]; 
        }}

    
    /// <summary>
    /// Hier wordt een inverse FFT gedaan
    /// </summary>
    /// <param name="complex"></param>
    public static void IFFTransform(Complex[] complex)
    {
        Fourier.Inverse(complex);
    }}