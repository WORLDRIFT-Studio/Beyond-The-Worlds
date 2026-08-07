# Informacje dla programistów:
---

## Struktura plików:

Beyond The Worlds (Root)
├── 📁 assets/                      # Wszystkie surowe zasoby i pliki binarne gry
│   ├── 📁 audio/                   # Muzyka (.ogg/.mp3) oraz efekty dźwiękowe (.wav)
│   ├── 📁 fonts/                   # Czcionki (.ttf/.otf) używane w interfejsie
│   └── 📁 themes/                  # Pliki stylów UI Godota (.theme), kolory, styleboxy
│
└── 📁 src/                         # Architektura kodu i sceny (Logika gry)
    ├── 📁 autoloads/               # Singletons / Autoloads (np. Global.cs, AudioManager.cs)
    ├── 📁 common/                  # Skrypty/Sceny ogólnego przeznaczenia (np. Twój FreeCam)
    ├── 📁 enemies/                 # Logika, AI, grafika i sceny przeciwników
    ├── 📁 levels/                  # Sceny poziomów (.tscn), mapy, środowiska (Environment)
    ├── 📁 objects/                 # Przedmioty interaktywne, przeszkody, skrzynie, znajdźki
    ├── 📁 ui/                      # Ekrany menu, HUD, paski zdrowia, ekrany pauzy
    └── 📁 vfx/                     # Efekty cząsteczkowe (GPUParticles), shadery, wybuchy

W powyższy sposób grupujemy pliki według przezanczenia, izolując je we własnych folderach, ułatwiajć zarządzanie wymaganymi plikami.

---

# Konwencje Nazewnicze projektu (Godot 4 + C#)

Dokument opisuje standardy nazewnictwa kodu i plików, mające na celu zapewnienie czystości w repozytorium Git oraz pełną synergię z analizatorem kodu JetBrains Rider.

---

### 1. Kod C# (PascalCase & camelCase)

Wszystkie skrypty C# muszą być zgodne z oficjalnym stylem .NET oraz architekturą silnika Godot.

#### Klasy, Struktury i Interfejsy
* **Klasy i Struktury (`PascalCase`):** Każda klasa dziedzicząca po obiektach Godota musi mieć modyfikator `partial`.
    ```csharp
    public partial class FreeCam : CharacterBody3D { }
    ```
* **Interfejsy (`IPascalCase`):** Zawsze zaczynają się od wielkiej litery `I`.
    ```csharp
    public interface IDamageable { }
    ```

#### Metody i Właściwości (Properties)
* **Metody (`PascalCase`):** Dotyczy to zarówno metod własnych, jak i nadpisywanych z cyklu życia Godota.
    ```csharp
    public override void _Ready() { }
    public void CalculateVelocity() { }
    ```
* **Właściwości (`PascalCase`):** Używaj właściwości z akcesorami zamiast publicznych pól.
    ```csharp
    [Export] public float Speed { get; private set; } = 10f;
    ```

#### Pola i Zmienne
* **Pola prywatne / chronione (`_camelCase`):** Zawsze zaczynane od podkreślenia (underscore).
    ```csharp
    private Vector3 _targetVelocity;
    ```
* **Zmienne lokalne i argumenty (`camelCase`):** Pisane małą literą, kolejne słowa wielką.
    ```csharp
    float moveUpDown = 0.0f;
    ```

#### Obsługa Zdarzeń (Input Event Wyjątek)
W C# `event` to słowo zastrzeżone. Zamiast pisać `@event`, dobrą praktyką w projekcie jest stosowanie nazwy `inputEvent` lub `@evt`:
```csharp
public override void _UnhandledInput(InputEvent inputEvent)
{
    if (inputEvent is InputEventMouseMotion mouseMotion) { }
}

---

2. System Plików i Zasoby Godota (snake_case)

Godot jest uruchamiany na systemach operacyjnych różnie traktujących wielkość liter (Windows ignoruje wielkość liter, Linux/Wayland ją rozróżnia). Aby uniknąć błędów importu:

---

- Foldery (snake_case): Wszystkie foldery wewnątrz res:// piszemy małymi literami z podłogami.

  - Dobrze: res://src/common/free_cam/

  - Źle: res://src/Common/FreeCam/

- Sceny i Zasoby binarne (snake_case): Pliki .tscn, .tres, .png, .wav itp.

  - Przykład: free_cam.tscn, exploding_barrel.tres

-Skrypty C# (PascalCase): Nazwa pliku .cs musi być identyczna z nazwą klasy w środku.

  - Przykład: Plik FreeCam.cs zawiera public partial class FreeCam.

---


