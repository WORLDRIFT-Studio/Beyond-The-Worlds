using System;
using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Flags]
public enum ElementTypes
{
    None = 0,
    Fire = 1 << 0,
    Water = 1 << 1,
    Earth = 1 << 2,
    Air = 1 << 3,
    Light = 1 << 4,
    Dark = 1 << 5,
}

[GlobalClass]
[Icon("res://addons/at-icons/node3d/cutlass.svg")]
public abstract partial class AttackData(int damage, double cooldown, ElementTypes attackElement)
    : Resource
{
    [Export(PropertyHint.Range, "0, 100, 0.01, prefer_slider, or_greater, suffix:DMG")]
    public int Damage { get; set; } = damage;

    [Export(PropertyHint.Range, "0.01, 10, 0.01, prefer_slider, or_greater, suffix:s")]
    public double Cooldown { get; set; } = cooldown;

    [Export(PropertyHint.Flags, "Fire, Water, Earth, Air, Light, Dark")]
    public ElementTypes AttackElement { get; set; } = attackElement;
}
