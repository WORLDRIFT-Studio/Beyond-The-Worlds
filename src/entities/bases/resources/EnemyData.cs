// -----------------------------------------------------------------------
// <copyright file="EnemyData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.enums;
using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/angry_face.svg")]
public partial class EnemyData : Resource
{
    [Export(PropertyHint.PlaceholderText, "Eg. Rift Shifter")]
    public string Name { get; set; } = string.Empty;

    [Export(PropertyHint.MultilineText, "Lorem ipsum dolores sit amet")]
    public string Description { get; set; } = string.Empty;

    [Export]
    public EnemyTypes EnemyType { get; set; } = EnemyTypes.Normal;

    [Export]
    public MapRings Ring { get; set; } = MapRings.EthernalForest;

    [Export(PropertyHint.Range, "0, 100, 1, prefer_slider, or_greater")]
    public int DangerLevel { get; set; }
}
