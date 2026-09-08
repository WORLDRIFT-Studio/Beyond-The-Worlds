// -----------------------------------------------------------------------
// <copyright file="AttackData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.enums;
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
    public double AttackCooldown { get; set; } = 1.0d;

    [Export(PropertyHint.Flags, "Fire, Water, Earth, Air, Light, Dark")]
    public ElementTypes AttackElement { get; set; } = ElementTypes.None;
}
