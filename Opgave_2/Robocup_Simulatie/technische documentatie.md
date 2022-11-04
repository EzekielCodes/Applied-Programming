# Opgave 2: Robot Simulation

### Functies


##### MainviewModel.cs / ```Initpresentation```

- In deze functie wordt de vaste shapes aangemaakt. Hier wordt de bal, terrain, Goal Post en rand aangemaakt.

##### MainviewModel.cs / ```Camerachoice())```

- Deze functie wordt verbonden met de De changeviewCommand button om een keuze te maken tussen een topview camera(perspectieve camera) of de default camera.



##### MainviewModel.cs / ```UpdateWorldDisplay()```

- Deze functie update om de 10 milliseconden de positie van de ball en spelers van beide teams.


##### GameLogic.cs / ```CreatePlayers(int aantal)```

- Deze functie verwacht een integer van het aantal spelers (max 3) , Hierin wordt een een even aantal spelers aangemaakt voor beide teams op random posities met de ``` GenerateRandomPoint(int i, bool state)`` functie -> deze functie generate een random Point op de veld een de bool state variable bepaalt aan welke kant van de veld dat de spelers aangemaakt moeten worden. 

##### gamelogic.cs

``` StartMoveAsync()```
-  Deze funtie wordt opgeroepen wanneer er op de strt knop wordt geklikt.
   In deze functie word een cancellationToken aangemaakt, hierna wordt de functie ```ExecSimulatieLoop()``` opgeroepen

    ``` ExecSimulatieLoop()```
    -   Dit is de functie voor de game simulation Loop hier wordt alles gecheckt zoals collision, movement en doelpunten gemaakt.

         - In SimulatieLoop wordt een ```async Task UpdatePosition(Ball,TimeSpan)``` die de positie van de spelers constant update.
         - In simulatieLoop wordt een ``` async Task MoveObject(ball)``` die constant de positie van de ball update.
         - In deze functie wordt de ```async Task CollisionWithBall(TImespan)``` word er constant gecheckt als de spelers het ball hebben geraakt door de afstand tussen de 2 objecten te     berekenen(player.position - ball.position).
         - Er is ook een functie ```collisionWithWall`` word hier constant gechecktals een ball of speler het rand heeft geraakt.
         - ```ColiisionWithPlayers``` checkt avoor botsing tussen de spelers door de afstand tussen de radius van 2 spelers te controleren
         - In de simulatieLoop wordt er ook constant gekeken als de ball en spelers de rand hebben geraakt. Hiervoor heb ik een functie ``` async Task CollisionWithWall``` gebruikt
         Hier wordt constant gekeken als de ball.X en ball.Z position in de range van de veld nog leggen anders zitten ze tegen de rand.



