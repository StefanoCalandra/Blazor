# Suite di applicazioni Blazor

Questo repository fornisce ora un singolo punto di accesso per tutte le applicazioni di esempio incluse. Apri la soluzione `BlazorSuite.sln` in Visual Studio oppure con gli strumenti `dotnet` per lavorare con qualsiasi progetto senza cambiare repository.

## Progetti inclusi

| Progetto | Percorso | Descrizione |
| --- | --- | --- |
| **BlazorWeatherApp** | `BlazorWeatherApp/BlazorWeatherApp.csproj` | Cruscotto meteorologico che dimostra il recupero dati e componenti riutilizzabili. |
| **BulkyBookWeb** | `Bulky/BulkyWeb/BulkyBookWeb.csproj` | Storefront ASP.NET Core MVC supportato da librerie di classe per dati, modelli e utilità. |
| **BulkyBook.DataAccess** | `Bulky/Bulky.DataAccess/BulkyBook.DataAccess.csproj` | Livello dati Entity Framework Core utilizzato dall'applicazione web BulkyBook. |
| **BulkyBook.Models** | `Bulky/Bulky.Models/BulkyBook.Models.csproj` | Classi di modello condivise per l'ecosistema BulkyBook. |
| **BulkyBook.Utility** | `Bulky/Bulky.Utility/BulkyBook.Utility.csproj` | Helper e costanti di supporto per BulkyBook. |
| **PmsApi** | `PmsApi/PmsApi.csproj` | Web API per l'esempio di sistema di gestione progetti. |
| **YumBlazor** | `YumBlazor/YumBlazor.csproj` | Demo di gestione ricette costruita con Blazor. |
| **LearnBlazor** | `LearnBlazor/LearnBlazor.csproj` | Raccolta di componenti e helper Blazor orientati all'apprendimento. |
| **TodoList** | `TodoList/TodoList.csproj` | Applicazione di gestione attività che mostra operazioni CRUD. |

## Idee di miglioramento

Cerchi modi per estendere gli esempi o mantenere ogni progetto focalizzato su uno scenario specifico? Consulta la [Roadmap di miglioramento della Blazor Suite](docs/project-improvement-roadmap.md) per idee curate di miglioramento e suggerimenti di differenziazione per tutti i progetti.

## Novità recenti

- **BlazorWeatherApp** ora memorizza in cache i risultati, convalida la configurazione all'avvio e può popolare il cruscotto usando la posizione corrente del browser per evidenziare un'integrazione API resiliente.
- **LearnBlazor** ha trasformato la pagina meteo in un laboratorio interattivo che dimostra caricamenti puntuali, aggiornamenti in streaming e cicli di aggiornamento annullabili in background.
- **YumBlazor** ha sostituito la schermata meteo segnaposto con uno spazio di pianificazione pasti che reagisce alla dispensa e alle preferenze di attenzione.
- **TodoList** salva le attività nell'archiviazione locale protetta, traccia le scadenze e supporta filtri basati su tag per diventare un compagno più ricco ai campioni di backend.

## Per iniziare

1. Ripristina le dipendenze e compila il progetto desiderato:
   ```bash
   dotnet build BlazorSuite.sln
   ```
   La compilazione della soluzione genera ogni esempio utilizzando gli stessi file di progetto esistenti prima della consolidazione, quindi funzionalità e riferimenti rimangono invariati.
2. Esegui un esempio specifico invocando `dotnet run` dalla cartella del progetto, ad esempio:
   ```bash
   cd BlazorWeatherApp
   dotnet run
   ```

Ogni progetto mantiene la propria struttura e configurazione originale, quindi la documentazione e i tutorial esistenti restano validi.
