// -----------------------------------------------------------------------'''//
// <copyright file="TargetingComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.common.debug_console;
using BeyondTheWorlds.entities.bases.types;
using BeyondTheWorlds.entities.enums;
using Godot;
using BaseComponent = BeyondTheWorlds.entities.bases.components.BaseComponent;
using Entity = BeyondTheWorlds.entities.bases.types.Entity;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/bullseye.svg")]
public partial class TargetingComponent : BaseComponent
{
    [Signal]
    public delegate void TargetChangedEventHandler(Entity newTarget);

    private RangeComponent? _range;

    [Export(PropertyHint.Range, "0.01, 1, 0.01, prefer_slider, suffix:s")]
    private double _refreshTime = .2d;

    private Entity? _target;

    [Export(PropertyHint.Flags)] private EntityFlags _targetTypes = EntityFlags.Enemy;

    private double _timer;

    [Export]
    private RangeComponent? Range
    {
        get => _range;
        set
        {
            _range = value;
            UpdateConfigurationWarnings();
        }
    }

    public Entity? Target
    {
        get => _target;
        private set
        {
            if (value == _target) return;
            _target = value;
            EmitSignalTargetChanged(_target);
        }
    }

    public bool HaveTarget => Target is not null;

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;
        _timer += delta;
        if (_timer < _refreshTime) return;

        UpdateTarget();
        _timer -= _refreshTime;
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (_range is null) warnings.Add("Missing range component. Add it in inspector.");

        return [.. warnings, .. base._GetConfigurationWarnings()];
    }

    public void RefreshTarget()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        if (Parent is null)
        {
            DebugConsole.Log(DebugLevel.Error, "TargetingComponent", $"Parrent is non-exist");
            return;
        }

        IList<Entity>? entities = _range?.GetEntitiesInRange;
        Target = entities
            ?.Where(IsValidType)
            .MinBy(e => e.GlobalPosition.DistanceSquaredTo(Parent.GlobalPosition));
    }

    private bool IsValidType(Entity? entity)
    {
        if (entity == Parent) return false;

        switch (entity)
        {
            case null:
                return false;

            case Enemy when _targetTypes.HasFlag(EntityFlags.Enemy):
            case Hero when _targetTypes.HasFlag(EntityFlags.Hero):
            case Building when _targetTypes.HasFlag(EntityFlags.Building):
                return true;

            default:
                return false;
        }
    }
}
