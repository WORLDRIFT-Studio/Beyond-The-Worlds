// -----------------------------------------------------------------------
// <copyright file="Hero.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.bases.resources;
using Godot;

namespace BeyondTheWorlds.entities.bases.types;

public partial class Hero : Entity
{
    [Export]
    public HeroData? EntityInformation { get; set; }
}
