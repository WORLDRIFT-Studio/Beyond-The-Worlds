// -----------------------------------------------------------------------
// <copyright file="BaseComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/icons.svg")]
public abstract partial class BaseComponent : Node3D
{
    public Node3D? Parent { get; set; }

    public override void _EnterTree()
    {
        UpdateConfigurationWarnings();
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        Node parent = GetParent();

        if (GetTree().EditedSceneRoot == this)
            return [.. warnings];

        if (parent is not ComponentContainer && GetTree().EditedSceneRoot != this)
            warnings.Add("Component must be a child of ComponentContainer.");

        return [.. warnings];
    }
}
