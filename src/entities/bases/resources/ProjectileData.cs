// -----------------------------------------------------------------------
// <copyright file="ProjectileData.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
public partial class ProjectileData : Resource
{
    [Export]
    public PackedScene? ProjectileScene { get; set; }

    [Export(PropertyHint.Range, "0.01, 100, 00.1, prefer_slider, or_greater, suffix:s")]
    public double Lifespan { get; set; } = 10.0d;

    [Export(PropertyHint.Range, "0.01, 100, 0.01, prefer_slider, or_greater, suffix:m/s")]
    public float Speed { get; set; } = 1.0f;

    [Export(PropertyHint.Range, "1, 10, 1, prefer_slider, or_greater, suffix:x")]
    public int ProjectileAmountPerAttack { get; set; } = 1;

    [Export(PropertyHint.Range, "0.01, 10, 0.01, prefer_slider, or_greater, suffix:s")]
    public double Delay { get; set; } = 0.5d;
}
