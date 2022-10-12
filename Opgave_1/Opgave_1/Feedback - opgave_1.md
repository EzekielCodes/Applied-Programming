
## Feedback Applied Programming

### Opgave 1: Realtime geluidsbewerking met FFT

#### Algemeen

#### Architectuur (10%)


***Modulair, meerlagenmodel***
- [x] *Meerlagenmodel via mappen of klassebibliotheken*
- [x] *Dependency injection*
- [x] *Gebruik  MVVM Design pattern*

* De 'oude' `Logic` en zijn interface (en ook `IData`) worden niet gebruikt. Waarom staan die dan in je solution? 

***'Separation of concern'***

- [x] *Domein-logica beperkt tot logische laag*
- [x] *Logische laag onafhankelijk van presentatielaag*
- [x] *'infrastructuur' code in datalaag*


#### Programmeerstijl, Kwaliteit van de code (5%)

***Naamgeving***

- [x] *Naamgeving volgens C# conventie*
- [x] *Zinvolle, duidelijke namen*

* gebruik overal zinvolle namen: `SelectedIndex` geeft niet duidelijk aan wat er precies geselecteerd is.

***Korte methodes***

- [x] *maximale lengte ~20 lijnen*

***Programmeerstijl***

- [x] *Layout code*
- [x] *Correct gebruik commentaar*
- [x] *Algemene stijl*

* Je hebt uit de demo-code nog het echo-mechanisme voor een deel overgehouden (UI en delayline). Dat is slordig en ze werken niet: verwijder die dan ook uit je code.
* Je hebt in de `AudioController` nog stukken voor het wegschrijven van een bestand (die je niet gebruikt). Verwijder die.
* `SearchIndex` en `GetLowIndex` (in `AudioController`) doen eigenlijk hetzelfde, maar op een andere manier. Je gebruikt daarvoor beter één methode twee keer.

#### User interface, functionaliteit, UX (15%) 

***Ergonomie***

- [x] *Layout UI*
- [x] *Goede UX*

* De 'echo' slider is niet functioneel (en overbodig): verwijder die.
* UI is eerder elementair.
* 'Min' frequentie kan pas ingesteld worden als de bovenste al ingegeven is.
* * De bediening van de filterparameters kan ook nog aangepast worden tijdens het afspelen (en dan heeft die geen effect meer bij batchverwerking).
* Waarom gebruik je een `RenderTransform` voor de positionering van de 'Max Frequentie' textbox (samen met een `Margin`)? 
* Na het selecteren van een bestand, geef je best een indicatie aan de gebruiker dat de verwerking bezig is.

***functionaliteit***

- [x] *Selectie type filter*
- [x] *instelling afsnijfrequenties*
- [x] *selectie audiobestand & bediening player* 

#### Goede werking – Inlezen afspelen – FFT – filtering (minstens batchverwerking) (20%)

***juiste technieken gebruikt***

- [x] *correct inlezen en afspelen samples*
- [x] *correcte toepassing FFT*
- [x] *juiste implementatie filter*

***Juiste werking***

- [x] *Goede werking*

* Als je tijdens het afspelen van een audiofragment een nieuw bestand selecteert en afspeelt, start dat niet bij het begin...

#### Goede werking – Segmentering  – Windowing – filtering (realtime verwerking)  (25%)

--> Nog niet beoordeeld

***juiste technieken gebruikt***

- [ ] *correct inlezen en afspelen samples met segmentatie*
- [ ] *correcte toepassing Windowing*
- [ ] *Correct gebruik overlap & combinatie segmenten*
- [ ] *juiste implementatie filter*

***Juiste werking***

- [ ] *Goede werking*

#### Installeerbare package voor distributie (10%)

- [x] *Installable package beschikbaar in repo*
- [x] *public key beschikbaar in repo (in geval van self-signed certificaat)*

* Je hebt het certificaat met de private key (*.pfx) in je repository opgenomen omdfat je (nog) geen '.gitignore' bestand hebt. Dat is zeker niet de bedoeling!!! 

#### Correct gebruik GIT (5%)

- [x] *Gebruik 'atomaire' commits*
- [x] *zinvolle commit messages*

* Je hebt nog geen '.gitignore' bestand zodat je heel wat 'rommel' in je repository krijgt...

#### Rapportering (10%)

- [x] *Structuur*
- [x] *Volledigheid*
- [x] *Technische diepgang*
- [ ] *Professionele stijl*

* Een kort overzicht van de architectuur van je programma (wat gebeurt in welke module) zou een goede aanvulling zijn.
* Lees je eigen tekst na: de zinsconstructie klopt niet altijd. 