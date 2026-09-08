// -----------------------------------------------------------------------
// <copyright file="MagicAttackData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/magic_wand.svg")]
public partial class MagicAttackData : AttackData
{
    [Export]
    public ProjectileData? ProjectileData { get; set; }

    [Export(PropertyHint.Range, "1, 10, 1, prefer_slider, or_greater, suffix:mana")]
    public int ManaCost { get; set; } = 5;
}
