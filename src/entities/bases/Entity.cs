using System.Collections.Generic;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node3d/fingerprint.svg")]
public partial class Entity : Node3D
{
    [Export]
    public ComponentContainer? ComponentContainer { get; private set; }

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
        where T : class => ComponentContainer?.GetComponent<T>();

    public List<T>? GetComponents<T>()
        where T : class => ComponentContainer?.GetComponents<T>();
}
