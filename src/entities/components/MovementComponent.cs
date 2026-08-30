// -----------------------------------------------------------------------
// <copyright file="MovementComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.entities.components;

public partial class MovementComponent : BaseComponent
{
    [Export(PropertyHint.Range, "0, 100, 0.25, or_greater, prefer_slider")]
    public double MaxSpeed { get; set; }

    [Export(PropertyHint.Range, "0, 100, 0.25, or_greater, prefer_slider")]
    public double MaxAcceleration { get; set; }
}
