// -----------------------------------------------------------------------
// <copyright file="ChaseState.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.common.debug_console;
using BeyondTheWorlds.entities.bases.states;
using BeyondTheWorlds.entities.components;
using Godot;

namespace BeyondTheWorlds.entities.states;

[Tool]
[GlobalClass]
public partial class ChaseState : State
{
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];
        if (Navigation is null) warnings.Add("Navigation is null");
        if (Targeting is null) warnings.Add("Targeting is null");
        if (Movement is null) warnings.Add("Movement is null");

        return [.. base._GetConfigurationWarnings(), .. warnings];
    }

    #region Life Cycle


    public override void PhysicsUpdate(double delta)
    {
        if (Movement is null || Targeting is null)
        {
            DebugConsole.Log(DebugLevel.Debug, "ChaseComponent", "StateVerification failed");
            return;
        }

        Vector3? waypoint = Navigation?.GetNextWaypoint();

        if (Targeting.Target is null || waypoint is null) return;
        Movement.MoveInDirection(waypoint.Value.DirectionTo(Targeting.Target.GlobalPosition));
    }

    #endregion

    #region Parameters

    private MovementComponent? _movement;
    private NavigationComponent? _navigation;
    private TargetingComponent? _targeting;

    [Export]
    private TargetingComponent? Targeting
    {
        get => _targeting;
        set
        {
            _targeting = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    private MovementComponent? Movement
    {
        get => _movement;
        set
        {
            _movement = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    private NavigationComponent? Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            UpdateConfigurationWarnings();
        }
    }

    #endregion
}
