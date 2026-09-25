// -----------------------------------------------------------------------
// <copyright file="BaseComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.bases.types;
using Godot;

namespace BeyondTheWorlds.entities.bases.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/icons.svg")]
public abstract partial class BaseComponent : Node3D
{
    public Entity? Parent { get; private set; }

    public override void _EnterTree()
    {
        UpdateConfigurationWarnings();
        Parent = FindEntityParent(this);
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        Node parent = GetParent();

        if (GetTree().EditedSceneRoot == this)
            return [.. warnings];

        if (
            parent is not (ComponentContainer or BaseComponent)
            && GetTree().EditedSceneRoot != this
        )
            warnings.Add("Component must be a child of ComponentContainer.");

        return [.. warnings];
    }

    private Entity? FindEntityParent(Node node)
    {
        Node? parent = node.GetParent();
        while (parent != null)
        {
            if (parent is Entity entity)
                return entity;
            parent = parent.GetParent();
        }

        return null;
    }
}
