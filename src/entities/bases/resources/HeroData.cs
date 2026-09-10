// -----------------------------------------------------------------------
// <copyright file="HeroData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

public partial class HeroData : Resource
{
    [Export(PropertyHint.PlaceholderText, "Enter a name for the hero.")]
    public string? Name { get; set; }

    [Export(PropertyHint.MultilineText, "Enter a description for the hero.")]
    public string? Description { get; set; }
}
