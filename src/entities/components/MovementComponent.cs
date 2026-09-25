// -----------------------------------------------------------------------
// <copyright file="MovementComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.common.debug_console;
using Godot;
using BaseComponent = BeyondTheWorlds.entities.bases.components.BaseComponent;

namespace BeyondTheWorlds.entities.components;

[GlobalClass]
[Tool]
public partial class MovementComponent : BaseComponent
{
    private Vector3? _direction;
    private bool _enititiesTargeting = true;

    [Export(PropertyHint.Range, "0, 100, 0.25, or_greater, prefer_slider")]
    private double Speed { get; set; }

    [Export]
    private TargetingComponent? TargetingComponent { get; set; }

    [Export]
    private bool EntitiesTargeting
    {
        get => _enititiesTargeting;
        set
        {
            _enititiesTargeting = value;
            UpdateConfigurationWarnings();
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (TargetingComponent is null && !EntitiesTargeting)
            warnings.Add(
                """
                TargetingComponent missing. Movement coponenent will be able only move to Dimensional Rift.
                If is this planed, turn off option "Entites Targeting".
                """
            );

        return [.. base._GetConfigurationWarnings(), .. warnings];
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Parent is null)
        {
            DebugConsole.Log(DebugLevel.Error, "MovementComponent", "Parent reference non-exist!");
            return;
        }

        if (_direction == null)
            return;

        Parent.Velocity = _direction.Value * (float)Speed;
        Parent.MoveAndSlide();
    }

    public void MoveInDirection(Vector3? direction)
    {
        _direction = direction;
        if (Parent is null)
        {
            DebugConsole.Log(DebugLevel.Error, "MovementComponent", "Parent reference non-exist!");
            return;
        }

        if (_direction is null)
        {
            Parent.Velocity = Vector3.Zero;
            return;
        }

        Parent.SetRotation((Vector3)_direction);
    }
}
