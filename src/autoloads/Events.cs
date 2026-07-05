using Godot;

namespace BeyondTheWorlds.autoloads;

/// <summary>
/// W celu dodanie nowego Eventu należy zadeklarować go poprzez [Signal] w public delegate void, gdzie nazwa powinna być
/// zakończona na 'EventHandler'. Nastepnie nalezy stowrzyć metode do emitowania sygnału poprzez użycie Instance.
/// Nazwa nowej metody powinna zaczynać się od 'Emmit' a nastepnie zaweirac nazwe Eventu do którego należy (bez końcówki
/// 'EventHandler').
/// </summary>
public partial class Events : Node
{
    public static Events Instance { get; private set; }
    
    [Signal]
    public delegate void PlayerTakedDamageEventHandler(short damage);
    [Signal]
    public delegate void PlayerHealedEventHandler(short heal);
    [Signal]
    public delegate void PlayerDiedEventHandler();
    [Signal]
    public delegate void PlayerHealthChangedEventHandler(short current, short max);
    [Signal]
    public delegate void NewGameStartedEventHandler();

    public override void _Ready()
    {
        Instance = this;
    }

    public static void EmitPlayerDamaged(short damage)
    {
        Instance.EmitSignalPlayerTakedDamage(damage);
    }

    public static void EmitPLayerHealed(short heal)
    {
        Instance.EmitSignalPlayerHealed(heal);
    }
    
    public static void EmitPlayerDied()
    {
        Instance.EmitSignalPlayerDied();
    }

    public static void EmitPlayerHealthChanged(short current, short max)
    {
        Instance.EmitSignalPlayerHealthChanged(current, max);
    }

    public static void EmitNewGameStarted()
    {
        Instance.EmitSignalNewGameStarted();
    }
}
