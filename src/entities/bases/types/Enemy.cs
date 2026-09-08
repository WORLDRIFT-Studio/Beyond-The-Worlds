// -----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.bases.resources;
using Godot;

namespace BeyondTheWorlds.entities.bases.types;

[GlobalClass]
[Icon("res://addons/at-icons/node3d/angry_face.svg")]
[Tool]
public partial class Enemy : Entity
{
    [Export]
    public EnemyData? EnemyData { get; set; }
}
