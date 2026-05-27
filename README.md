# Filmotéka

Desktopová aplikace pro správu filmů a recenzí. Vytvořeno v Avalonia UI s MVVM architekturou a PostgreSQL databází běžící v Dockeru.

---

## Funkce

- Přidání, úprava a smazání filmů
- Přidání, úprava a smazání recenzí u každého filmu
- Filtrování filmů podle statusu (Plánuji / Koukám / Dokoukáno / Zrušeno)
- Vyhledávání filmů podle názvu nebo režiséra

---

## Technologie

- C# / .NET
- Avalonia UI (MVVM)
- PostgreSQL (Docker)
- Npgsql
- CommunityToolkit.Mvvm
- DotNetEnv
- Microsoft.Extensions.DependencyInjection

---

## Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [JetBrains Rider](https://www.jetbrains.com/rider/) nebo Visual Studio

---

## Spuštění projektu

### 1. Klonování repozitáře
```bash
git clone https://github.com/TVOJE_JMENO/NAZEV_REPO.git
cd NAZEV_REPO
```

### 2. Vytvoření .env souboru
Zkopíruj `.env.example` a vyplň své údaje:
```bash
cp .env.example .env
```

Obsah `.env`:
HOST=localhost
PORT=5432
DATABASE=filmoteka
USERNAME=postgres
PASSWORD=tvoje_heslo

### 3. Spuštění databáze
```bash
docker compose up -d
```

Databáze se automaticky vytvoří a naplní základními daty (statusy filmů).

### 4. Spuštění aplikace
V Rideru klikni na **Run**, nebo z příkazové řádky:
```bash
dotnet run
```

---

## Struktura projektu
final_project/
├── Models/             # Datové třídy (Film, Review, FilmStatus)
├── Repositories/       # Rozhraní a implementace přístupu k DB
├── ViewModels/         # Logika UI (MVVM)
├── Views/              # AXAML soubory (UI)
├── Services.cs         # Registrace závislostí (DI)
├── App.axaml           # Vstupní bod aplikace
├── docker-compose.yaml
├── schema.sql          # Vytvoření tabulek
├── seed.sql            # Naplnění číselníku
├── .env.example
└── .gitignore

---

## Databázové schéma
film_status (id, name)
film (id, title, director, year, genre, status_id → film_status)
review (id, film_id → film, author, rating, content, created_at)
