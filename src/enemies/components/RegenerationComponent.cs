using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.enemies.components;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node3d/medkit.svg")]
public partial class RegenerationComponent : BaseComponent
{
    private double _regenerationRate;
    private double _regenerationPercent;
    private double _clock;

    /// <summary>
    /// Healing interval. Cant be negative.
    /// </summary>
    [Export(PropertyHint.Range, "0, 100, 0.1, or_greater, prefer_slider, suffix:s")]
    public double RegenerationRate
    {
        get => _regenerationRate;
        private set
        {
            if (value <= 0)
                return;
            _regenerationRate = value;
        }
    }

    /// <summary>
    /// Health percentage to heal. Cant be negative.
    /// </summary>
    [Export(PropertyHint.Range, "0, 100, 0.1, or_greater, prefer_slider, suffix:HP")]
    public double RegenerationPercent
    {
        get => _regenerationPercent;
        private set
        {
            if (value <= 0)
                return;
            _regenerationPercent = value;
        }
    }

    /// <summary>
    /// Reference to HealthComponent for healing
    /// </summary>
    [Export]
    public HealthComponent HealthComponent { get; private set; }

    public override void _Process(double delta)
    {
        _clock += delta;
        if (!(_clock >= _regenerationRate))
            return;
        HealthComponent.HealByPercent(_regenerationRate);
        _clock -= _regenerationRate;
    }
}
