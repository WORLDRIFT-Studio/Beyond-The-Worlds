// -----------------------------------------------------------------------
// <copyright file="Entity.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;
using ComponentContainer = BeyondTheWorlds.entities.bases.components.ComponentContainer;

namespace BeyondTheWorlds.entities.bases.types;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/fingerprint.svg")]
public abstract partial class Entity : Node3D
{
    private ComponentContainer? _componentContainer;

    [Export]
    public ComponentContainer? ComponentContainer
    {
        get => _componentContainer;
        private set
        {
            _componentContainer = value;
            UpdateConfigurationWarnings();
        }
    }

    public override void _Ready()
    {
        ComponentContainer = GetNodeOrNull<ComponentContainer>("ComponentContainer");
        ChildEnteredTree += _ => UpdateConfigurationWarnings();
        ChildExitingTree += _ => UpdateConfigurationWarnings();
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (ComponentContainer == null)
            warnings.Add("Missing ComponentContainer.");

        return warnings.ToArray();
    }

    public T? GetComponent<T>()
        where T : class
    {
        return ComponentContainer?.GetComponent<T>();
    }

    public IList<T>? GetComponents<T>()
        where T : class
    {
        return ComponentContainer?.GetComponents<T>();
    }
}
