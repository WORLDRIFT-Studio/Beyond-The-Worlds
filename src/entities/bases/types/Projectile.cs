// -----------------------------------------------------------------------
// <copyright file="Projectile.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.bases.resources;
using Godot;

namespace BeyondTheWorlds.entities.bases.types;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/arrow_projectile.svg")]
public partial class Projectile : StaticBody3D
{
    private double _lifespan;
    private double _lifespanTimer;
    private double _speed;

    public override void _PhysicsProcess(double delta)
    {
        _lifespanTimer += delta;
        GlobalPosition = GlobalPosition with { X = (float)(GlobalPosition.X + _speed * delta) };
        if (_lifespanTimer >= _lifespan)
            QueueFree();
    }

    public void Initialize(ProjectileData projectileData)
    {
        _speed = projectileData.Speed;
        _lifespan = projectileData.Lifespan;
    }
}
