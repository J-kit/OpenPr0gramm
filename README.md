# OpenPr0gramm
Eine quelloffene .NET-Implementierung für das pr0gramm.

## Installation

Via NuGet:
```
Install-Package OpenPr0gramm
```

**Achtung:** Nach dem Installieren wird eine `RefitStubs.cs` zu dem Projekt hinzugefügt. Diese wird immer beim Kompilieren neu generiert. **Du darfst sie nicht löschen**, sonst findet der Generator nicht mehr und wirft einen Compilerfehler. Ich weiß, du brauchst sie eigentlich nicht, aber aufgrund einer bescheuerten Designentscheidung von Refit (siehe [die Issue dazu](https://github.com/paulcbetts/refit/issues/120)) geht das nicht anders. Wenn du gerade eine freie Minute hast, schau doch bei der [Issue](https://github.com/paulcbetts/refit/issues/120) vorbei und weise den Maintainer darauf hin, dass man da was machen sollte.

## Verwendung
Die Library besteht aus 3 Schichten und ist an der JS-API der Webseite orientiert:

1. Refit-Interface-HTTP-Wrapper
2. Mapping der Rohdaten auf die Interface-Abstraktionen (`IPr0grammApiClient`)
3. Wrapping von abstrahierten Parametern auf die Rohdaten (`Pr0grammClient`)

Für den normalen Umgang sollte der 3. Layer reichen. Wenn du willst, kannst du aber auch die einzelnen Schichten austauschen. Die Library sollte auf allen Plattformen lauffähig sein, auf denen Refit und JSON.NET funktionieren.

Hier etwas Beispielcode:
```C#
var client = new Pr0grammClient();
var loginRes = await client.User.LogIn("user", "password");
if(!loginRes.Success)
{
	if(loginRes.Ban != null && loginRes.Ban.IsBanned)
	{
		Console.WriteLine($"Du bist bis {loginRes.Ban.Until} gebannt. Warum? \"{loginRes.Ban.Reason}\".");
	}
	else
	{
		Console.WriteLine("Das Passwort war wohl falsch oder so.");
	}
	return;
}

var frontItemRes = await client.Item.GetItems(ItemFlags.SFW, ItemStatus.Promoted);
Console.WriteLine("Posts:");
foreach(var item in frontItemRes.Items)
{
	Console.WriteLine($"{item.Id} von {item.User} ({item.Mark})");
}

CookieContainer container = client.GetCookie(); // Kann weggespeichert/serialisiert werden
// für spätere Verwendung (um sich nicht noch mal einloggen zu müssen)
var client2 = new Pr0grammClient(container); // Client mit Satz an Cookies initialisieren
```
Der Rest sollte selbsterklärend sein. Sämtliche Funktionalität befindet sich bei der `Pr0grammClient`-Klasse.

### Extras

OpenPr0gramm hat auch ein paar Zusatzklassen, die ein paar API-Funktionen wrappen.

#### SyncTimer
Der `SyncTimer` ist ein Timer, der den Sync-Befehl automatisch aufruft. Das ist z. B. praktisch um zu schauen, ob es neue Nachrichten gibt.
```C#
var timer = new SyncTimer(client); // oder einen IPr0grammUserService
timer.GotSync += (sender, e) => Console.WriteLine($"Es gibt {e.InboxCount} neue Nachrichten");
timer.Start(); // Syncs anfangen
// ...
timer.Stop(); // keine Syncs mehr machen
```
Die `lastId` behält der `SyncTimer` intern vor und updated sie entsprechend.


## Nutzungsbestimmungen/Lizenz
Zusätzlich zu den in der LICENSE-File angegebenen Bestimmungen gilt:
- **Keine kommerzielle Nutzung.**
- Hole dir *vorher* die Erlaubnis der Seitenbetreiber, deine Anwendung zu entwickeln.
Wenn du etwas vorhast, was nicht in Einklang mit den Nutzungsbestimmungen ist, kontaktiere mich (via pr0gramm/Email) und wir können drüber reden.

## Bugs
Es kann sein, dass bei der Serialisierung bestimmte Felder aufgrund von Typos oder Brainlags bei der Implementierung nicht richtig gemappt werden. Wenn dir sowas auffällt, kontaktire mich bitte, poste eine Issue oder fix es selbst und stelle einen Pull-Request.
