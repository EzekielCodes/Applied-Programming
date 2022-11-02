# Opgave 2: Robot Simulation

### Functies


##### MainviewModel.cs / ```Initpresentation```

- In deze functie wordt de vaste shapes aangemaakt. Hier wordt de bal, terrain, Goal Post en rand aangemaakt.

##### MainviewModel.cs / ```Camerachoice())```

- Deze functie wordt verbonden met de De changeviewCommand button om een keuze te maken tussen een topview camera(perspectieve camera) of de default camera.

##### World.cs / ```CreatePlayers(int aantal)```

- Deze functie verwacht een integer van het aantal spelers (max 3) , Hierin wordt een een even aantal spelers aangemaakt voor beide teams op random posities met de ``` GenerateRandomPoint(int i, bool state)`` functie -> deze functie generate een random Point op de veld een de bool state variable bepaalt aan welke kant van de veld dat de spelers aangemaakt moeten worden. 


##### MainviewModel.cs / ```UpdateWorldDisplay()```

- Deze functie update om de 10 milliseconden de positie van de ball en spelers van beide teams.


