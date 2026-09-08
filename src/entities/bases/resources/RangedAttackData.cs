// -----------------------------------------------------------------------
// <copyright file="RangedAttackData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/bow_and_arrow.svg")]
public partial class RangedAttackData : AttackData
{
    [Export]
    public ProjectileData? ProjectileData { get; set; }
}
