using Godot;

namespace BeyondTheWorlds.autoloads;

public partial class Events : Node
{
    public static Events Instance { get; private set; }
    
    [Signal]
    public delegate void PlayerDamagedEventHandler(short damage);
    [Signal]
    public delegate void PlayerHealedEventHandler(short heal);
    [Signal]
    public delegate void PlayerDiedEventHandler();
    [Signal]
    public delegate void PlayerHealthChangedEventHandler(short current, short max);


    public override void _Ready()
    {
        Instance = this;
    }

    public static void EmitPlayerDamaged(short damage)
    {
        Instance.EmitSignalPlayerDamaged(damage);
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
}
