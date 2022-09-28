﻿using AudioTools.Factories;
using AudioTools.Implementation;
using AudioTools.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Globals.Interfaces;
using System.Diagnostics;
using System.Windows.Controls;
//for complex numbers
using System.Numerics;

//
using MathNet.Numerics.IntegralTransforms;
using System.Windows.Markup;
using ScottPlot;

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
    private bool _disposedValue;
    private Complex[] _complexArrayLeft;
    private Complex[] _complexArrayRight;
    private int _aantalFrame = 0;
    private int _tempFrame = 0;
    private int _range = 0;

    public float[] arrayleft;
    public float[] arrayright;

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
        //Debug.WriteLine(_reader.TimeLength.TotalSeconds);
        double TotalsampleRate = _reader.TimeLength.TotalSeconds * _reader.SampleRate;
        int sampleRate = (int)TotalsampleRate;
        int ExpectedSize = GetValuePow(sampleRate,1);

        arrayleft = new float[ExpectedSize];
        arrayright= new float[ExpectedSize];

        _complexArrayLeft = new Complex[ExpectedSize];
        _complexArrayRight = new Complex[ExpectedSize];


        ReadSamples();


       /* for (int i = sampleRate; i < arrayleft.Length; i++)
        {
            arrayleft[i] = 0;
            arrayright[i] = 0;
        }*/
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        _player = _audioPlayerFactory.Create(_currentDevice, _reader!.SampleRate);
        _player.Volume = _volume;
        _delayLine.Clear();
        //_player.SampleFramesNeeded += Readplayerframes;
        
        _player.SampleFramesNeeded += Player_OnSampleFramesNeeded;
        //_reader.ReadSamples(arrayleft, arrayright);

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

    private void Player_OnSampleFramesNeeded(int frameCount)
    {

        
        for (int i = 0; i < frameCount; i++)
        {
            _aantalFrame = i;
            var sampleFrame = CalculateNextFrame();
            _range++;
            //Debug.WriteLine("range after for sample " + _range);
            if (IsRecording) _recorder!.WriteSampleFrame(sampleFrame);
            _player?.WriteSampleFrame(sampleFrame);

        }
        //Debug.WriteLine("Samples needed " + frameCount);
        //Debug.WriteLine("range after for loo " + _range);
       
    }

    private  void ReadSamples()
    {
        for (int i = 0; i < _complexArrayLeft.Length; i++)
        {

            var x = _reader.ReadSampleFrame();
            _complexArrayLeft[i] = x.Left;
            _complexArrayRight[i] = x.Right;
        }
       // Debug.WriteLine(_complexArrayLeft[2000].Real);
    }

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


    public void Fillcomplex()
    {
        Calculateblok();
    }

    //"Forward " fourier time => frequency
    public void Calculateblok()
    {
        /*
         * samplerate lezen van _reader
         * n = frequentieNauwkeurigheid 
         * vb samplerate = 44100hz /5 => 8820
         * 8820 (2^14) => 16384()
         */
        int samplerate = _reader.SampleRate;
        int n = BerekenN(samplerate);
        Debug.WriteLine(n);

        /*
         * Je splitst het originele signaal op in blokken van 16k monsters
         * Bepaling van de f-band die 50 Hz omvat: de f-resolutie na FFT is 44100/16384 = 2,69Hz. De eerste
         * waarde slaat dus op de f-band tussen 0 en 2,69 Hz. de tweede op de band van 2,69 tot 5,38Hz
         */
        double opsplitsenOrigineel = (double)samplerate / n;
        
        /*
         * f-band tussen 0 en 2,69 Hz ronden we op naar boven met Math.ceiling dus 3Hz per blok
         */
        int bloksize = (int)Math.Ceiling(opsplitsenOrigineel);

        Complex[][] complexBloxLeft = Fullcomplexblok(bloksize, n , _complexArrayLeft);

        Complex[][] complexBloxRight = Fullcomplexblok(bloksize, n, _complexArrayRight);
 }

      
     
    /*
     * eerst sample rate berekenen moet in de macht van 2^x zijn.
     * 5 = 5hz veresite nauwkeurigheid
     */
    public int BerekenN(int samplerate)
    {
        int mod = samplerate / 5;
        int n = GetValuePow(mod, 1);
        return n;
    }

    /*
     * eerst sample rate berekenen moet in de macht van 2^x zijn(FFT laat alleen macht to 2 toe..
     * hier wordt een controle gedaan indien samplerate nog lager is blijven we optellen tot
     */

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

    /*
     * aan de hand van het aantal blok kunnen we een multidimensionel array
     * aanmaken. kolom = aantal blok ... rij = lengte
     */
    public Complex[][] Fullcomplexblok(int aantalblok, int n, Complex[] complexarray)
    {
        
        Complex[][] blokcomplex = new Complex[aantalblok][];
        for (int i = 0; i < aantalblok; i++)
        {
            blokcomplex[i] = new Complex[n];
            for (int x = 0; x < n; x++)
                blokcomplex[i][x] = complexarray[x].Real;
            Fourier.Forward(blokcomplex[i]);
        }
        return blokcomplex;}}