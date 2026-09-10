// -----------------------------------------------------------------------
// <copyright file="Building.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.bases.resources;
using Godot;

namespace BeyondTheWorlds.entities.bases.types;

public partial class Building : Entity
{
    [Export]
    public BuildingData? EntityInformation { get; set; }
}
