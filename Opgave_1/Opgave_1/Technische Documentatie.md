# Opgave 1: Realtime geluidsbewerking met FFT

### Functies

##### AudioController.cs / ```CalculateSampleRate()```

- Deze functie berekent de lengte van de complexe array door de TotalSeconds * samplerate
  De waarde de we hieruit krijgen doen we eerst tot de macht van 2 want FFT werkt met waarde van de macht van 2.
  De ```GetValuePow ``` functie zorgt ervoor dat de arrayLength macht van 2 is.


##### AuddioController.cs /```ReadSamples()```

- Deze functie vult de 2 complexe array op met samples die uit de reader funtie ReadSampleFrame komt, De samples bevat een linker en rechter kanaal


##### AudioController.cs / ``` ConvertandFilter() ```
- 
    De ``` CalculateFreqResolutie() ``` functie berekent de frequentie resolutie door de sampleRate te delen door de lengte van de array.
- In deze functie gebeurt roepen we onze bandpass en bandstop maar eerst wordt er gecheckt welke toepassing onze gebruiker (via GUI)
  nodig heeft en hiermee kunnen we een no filter, bandstop of bandpass toepassing gebruiken.

  **De mogelijke filters zijn:**
    1. No filter
    2. Bandpass
    3. Bandstop

  + Bij no filter wordt geen berekening gedaan op de complex arrays, de oorspronkelijke waarden worden gewoon afgespeeld.
  + Bij BandPass wordt eerst een FFT transform gedaan, daarna woorden de arrays gefilterd door alle indexen op 0 te zetten behalve die in de index range.
  + Bij BandStop wordt eerst een FFT transform gedaan, daarna woorden de arrays gefilterd door de waarden van de index range op 0 te zetten.


  - ``` FFTransform() ```
    - Deze functie doet de FFT Transform, Hiervoor gebruiken we de MathNet.Numerics.IntegralTransforms package.

  ```BandStop(Complex[] complex, int indexHigh, int indexLow)```

  - Deze functie berekent de BandStop door alle indexen tussen de indexLow en indexHigh op 0 te zetten
    De inverse van deze indexen worden ook op 0 gezet. 


  ```BandPass(Complex[] complex, int indexHigh, int indexLow)```

  - Deze functie berekent de BandPass door alle indexen op 0 te zetten behalve die in de index range
    We doen hetzelfde voor het inverse indexen.


  ``` IFFTransform() ```
  - Deze functie doet de IFFT Transform, Hiervoor gebruiken we de MathNet.Numerics.IntegralTransforms package.



