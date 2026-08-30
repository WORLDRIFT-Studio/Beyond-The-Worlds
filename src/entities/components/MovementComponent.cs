using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.entities.components;

public partial class MovementComponent : BaseComponent
{
    [Export(PropertyHint.Range, "0, 100, 0.25, or_greater, prefer_slider")]
    public double MaxSpeed { get; set; }

    [Export(PropertyHint.Range, "0, 100, 0.25, or_greater, prefer_slider")]
    public double MaxAcceleration { get; set; }
}
