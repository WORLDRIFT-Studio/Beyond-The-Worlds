// -----------------------------------------------------------------------
// <copyright file="BuildingData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

public partial class BuildingData : Resource
{
    [Export(PropertyHint.PlaceholderText, "Enter building name")]
    public string? Name { get; set; }

    [Export(PropertyHint.MultilineText, "Enter building description")]
    public string? Description { get; set; }
}
