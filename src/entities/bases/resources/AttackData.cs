using System;
using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/cutlass.svg")]
public abstract partial class AttackData : Resource
{
    [Export(PropertyHint.Range, "0, 100, 0.01, prefer_slider, or_greater, suffix:DMG")]
    public int Damage { get; set; } = 5;

    [Export(PropertyHint.Range, "0.01, 10, 0.01, prefer_slider, or_greater, suffix:s")]
    public double Cooldown { get; set; } = 1.0;

    [Export(PropertyHint.Flags, "Fire, Water, Earth, Air, Light, Dark")]
    public ElementTypes AttackElement { get; set; } = ElementTypes.None;
}
