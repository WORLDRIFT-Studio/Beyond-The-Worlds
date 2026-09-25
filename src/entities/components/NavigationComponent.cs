// -----------------------------------------------------------------------
// <copyright file="NavigationComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.common.debug_console;
using BeyondTheWorlds.entities.bases.components;
using Godot;
using Entity = BeyondTheWorlds.entities.bases.types.Entity;

namespace BeyondTheWorlds.entities.components;

[GlobalClass]
[Icon("res://addons/at-icons/node3d/road.svg")]
[Tool]
public partial class NavigationComponent : BaseComponent
{
    [Signal]
    public delegate void NavigationChangedEventHandler();

    private NavigationAgent3D? _navigationAgent3D;
    private TargetingComponent? _targetingComponent;

    [Export]
    private TargetingComponent? TargetingComponent
    {
        get => _targetingComponent;
        set
        {
            _targetingComponent = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    private NavigationAgent3D? NavigationAgent3D
    {
        get => _navigationAgent3D;
        set
        {
            _navigationAgent3D = value;
            UpdateConfigurationWarnings();
        }
    }

    public Vector3? DefaultTargetPosition { get; set; }

    public Vector3? NextWaypoint => NavigationAgent3D?.GetNextPathPosition();

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
            return;

        if (TargetingComponent != null)
        {
            TargetingComponent.TargetChanged += TargetingComponentOnTargetChanged;
            if (NavigationAgent3D != null && TargetingComponent.Target != null)
                NavigationAgent3D.TargetPosition = TargetingComponent.Target.GlobalPosition;
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];
        if (NavigationAgent3D is null)
            warnings.Add(
                "NavigationComponent require a NavigationAgent3D for pathfinding. Add a NavigationAgent3D as a child of this node."
            );

        return [.. base._GetConfigurationWarnings(), .. warnings];
    }

    private void TargetingComponentOnTargetChanged(Entity newTarget)
    {
        SetNavigation(newTarget.GlobalPosition);
    }

    public void SetNavigation(Vector3 position)
    {
        NavigationAgent3D?.SetTargetPosition(position);
    }

    public Vector3? GetNextWaypoint()
    {
        if (NavigationAgent3D is null)
        {
            DebugConsole.Log(
                DebugLevel.Error,
                "NavigationComponent",
                $"Navigation agent missing at {Parent?.Name ?? "Unknown"}"
            );
            return null;
        }

        Vector3? nav = NavigationAgent3D.GetNextPathPosition();
        return nav;
    }
}
