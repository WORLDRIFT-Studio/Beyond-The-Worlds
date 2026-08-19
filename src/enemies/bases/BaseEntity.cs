using System.Collections.Generic;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass]
public partial class Entity : Node3D
{
    [Export]
    public ComponentContainer ComponentContainer { get; private set; }

    public override void _Ready()
    {
        ComponentContainer = GetNode<ComponentContainer>("ComponentContainer");
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];
        if (ComponentContainer == null)
            warnings.Add("Missing ComponentContainer.");
        return warnings.ToArray();
    }

    public T GetComponent<T>()
        where T : class
    {
        return ComponentContainer.GetComponent<T>();
    }

    public List<T> GetComponents<T>()
        where T : class
    {
        return ComponentContainer.GetComponents<T>();
    }
}
