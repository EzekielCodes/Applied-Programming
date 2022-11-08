
## Feedback Applied Programming

### Opgave 2: Robocup Simulatie

#### Algemeen

#### Architectuur (10%)


***Modulair, meerlagenmodel***

- [x] *Meerlagenmodel via mappen of klassebibliotheken*
- [x] *Dependency injection*
- [x] *Gebruik  MVVM Design pattern*

* Bij je `GameLogic` gebruik je geen DI om je `IGamePhysics` instantie door te geven.
* De 'oude' `DataAccessLayer` en zijn interface worden niet gebruikt. Waarom staan die dan in je solution? 


***'Separation of concern'***

- [x] *Domein-logica beperkt tot logische laag*
- [x] *Logische laag onafhankelijk van presentatielaag*
- [x] *'infrastructuur' code in datalaag*


#### Programmeerstijl, Kwaliteit van de code (5%)



***Naamgeving***

- [x] *Naamgeving volgens C# conventie*
- [x] *Zinvolle, duidelijke namen*

***Korte methodes***

- [ ] *maximale lengte ~20 lijnen*

* Splits te lange methodes op in deelmethodes!

***Programmeerstijl***

- [x] *Layout code*
- [x] *Correct gebruik commentaar*
- [x] *Algemene programmeerstijl*

* Je volgt niet overal de regels van clean code en C# conventies.
* Je gebruikt 'async' soms verkeerd.

#### User interface, functionaliteit, UX (15%) 

***Ergonomie***

- [x] *Layout UI*
- [x] *Goede UX*

* De UX is nog niet OK doordat je simulatie zelf nog niet helemala in orde is.

***functionaliteit***

- [x] *Selectie aantal spelers*
- [x] *Start, Stop, Pauze, reset, Unblock*
- [x] *Score, timer, indicatie goal & einde wedstrijd* 

* Er is geen 'Unblock' mogelijkheid.

#### Goede werking – Visualisatie (20%)

***juiste technieken gebruikt***

- [x] *correct weergeven terrein*
- [x] *correct weergeven spelers & bal (via Transform)*
- [x] *controlleerbare & 'Top'-camera*
- [x] *Goede werking*

#### Goede werking –– Fysische simulatie , gedrag van de robots (30%)

***juiste technieken gebruikt***

- [x] *Correcte fysische simulatie bal (positie, richting, vertraging) *
- [x] *Correcte fysische simulatie speler (positie, richting, versnelling)*
- [ ] *Collision detection (walls, ball, players)*
- [ ] *Collison resolution*
- [ ] *Goal detection*

***Juiste werking***

- [x] *Goede werking render- & gameloop*

***Snelheid, efficiëntie, concurrency***

- [ ] *Zinvol gebruik concurrency*
- [x] *Efficiënte (vector)berekeningen*

* Je gebruikt hier async methodes op een foute manier.

#### Installeerbare package voor distributie (5%)


- [x] *Installable package beschikbaar in repo*
- [x] *public key beschikbaar in repo (in geval van self-signed certificaat)*

* Je hebt nog geen grafische assets toegevoegd.

#### Correct gebruik GIT (5%)

- [x] *Gebruik 'atomaire' commits*
- [x] *zinvolle commit messages*


#### Rapportering (10%)

- [x] *Structuur*
- [ ] *Volledigheid*
- [ ] *Technische diepgang*
- [x] *Professionele stijl*

* Technische diepgang is beperkt: uitleg over de fysische simualtie, vectorberekening, collisiondetection en -verwerking ontbreken bijna volledig.