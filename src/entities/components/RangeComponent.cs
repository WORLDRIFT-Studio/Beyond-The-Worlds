// -----------------------------------------------------------------------
// <copyright file="RangeComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.enemies.bases;
using Godot;
using Entity = BeyondTheWorlds.entities.bases.types.Entity;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/target.svg")]
public partial class RangeComponent : BaseComponent
{
    [Signal]
    public delegate void EntityEnteredAreaEventHandler(Entity entity);

    [Signal]
    public delegate void EntityExitedAreaEventHandler(Entity target);

    private Area3D? _detectionArea;
    private CollisionShape3D? _detectionHandler;
    private CsgSphere3D? _detectionSphere;

    private float _radius;

    [Export(PropertyHint.Range, "1, 10, or_greater, prefer_slider, suffix:m")]
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value > 0 ? value : _radius;
            RedrawArea();
        }
    }

    /// <summary>
    ///     Return all entities in range, if no entities in range, return null.
    /// </summary>
    public IList<Entity>? EntitiesInRange =>
        _detectionArea?.GetOverlappingBodies().Cast<Entity>().ToList();

    public override void _Ready()
    {
        if (_detectionArea is null || _detectionHandler is null || _detectionSphere is null)
        {
            UpdateConfigurationWarnings();
            return;
        }

        _detectionArea = GetNode<Area3D>("%DetectionArea");
        _detectionHandler = GetNode<CollisionShape3D>("%DetectionHandler");
        _detectionSphere = GetNode<CsgSphere3D>("%DetectionSphere");

        if (Engine.IsEditorHint() || _detectionArea == null)
            return;

        _detectionArea.BodyEntered += body =>
        {
            if (body is Entity entity)
                EmitSignalEntityEnteredArea(entity);
        };

        _detectionArea.BodyExited += body =>
        {
            if (body is Entity entity)
                EmitSignalEntityExitedArea(entity);
        };
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (GetTree().GetCurrentScene().GetOwner() == this)
            return [];

        if (_detectionArea is null || _detectionHandler is null || _detectionSphere is null)
            warnings.Add(
                "This node must be instanced as a scene. Delete it, and add it from scenes."
            );

        return [.. warnings, .. base._GetConfigurationWarnings()];
    }

    public bool IsEntityInRange(Entity target)
    {
        return _detectionArea != null && _detectionArea.OverlapsBody(target);
    }

    private void RedrawArea()
    {
        if (_detectionHandler?.Shape is SphereShape3D shape)
            shape.Radius = Radius;

        if (_detectionSphere != null)
            _detectionSphere.Radius = Radius;
    }
}
