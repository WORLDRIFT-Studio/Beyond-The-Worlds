// -----------------------------------------------------------------------
// <copyright file="MeleeAttackData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/baseball_bat.svg")]
public partial class MeleeAttackData : AttackData
{
    [Export(PropertyHint.Range, "0.00, 1, 0.05, prefer_slider")]
    public double KnockbackForce { get; set; } = 0.5d;
}
