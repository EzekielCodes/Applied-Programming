# Opgave 1: Realtime geluidsbewerking met FFT

### Functies

##### AudioController.cs / ```CalculateSampleRate()```

- Deze functie  berkent de lengte van de complexe array door de TotalSeconds * samplerate
  De waarde de we hieruit krijgen doen we eerst tot de macht van 2 want FFT werkt met waarde van de macht van 2.


##### AudioController.cs / ```BandStop(Complex[] complex, int indexHigh, int indexLow)```

- Deze funtie berekent de BandStop door alle index tussen de indexLow en indexHigh op 0 te zetten
  De inverse index zetten we ook op 0 


##### AudioController.cs / ```BandPass(Complex[] complex, int indexHigh, int indexLow)```

- Deze functie berekent de BandPass door alle index tussen de indexLow en indexHigh die laager zijn dan 0 op die op 0 te zetten.
  we doen het zelfde voor het inverse indexen


##### AudioController.cs / ``` ConvertandFilter() ```
- in deze functie gebeurt roepen we onze bandpass en bandstop maar eerst wordt er gechecked welke toepassing onze gebruiker
  nodig heeft en hiermee kunnen we een no filter , bandstop of bandpass toepassing gebruiken.