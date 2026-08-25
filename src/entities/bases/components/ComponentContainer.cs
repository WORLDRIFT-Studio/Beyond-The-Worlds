using System.Collections.Generic;
using System.Linq;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node/archive.svg")]
public partial class ComponentContainer : Node3D
{
    public T? GetComponent<T>()
        where T : class
    {
        return Components.Values.OfType<T>().FirstOrDefault();
    }

    public List<T> GetComponents<T>()
        where T : class
    {
        return Components.Values.OfType<T>().ToList();
    }

    public Godot.Collections.Dictionary<string, BaseComponent> Components { get; set; } = new();

    public override void _Ready()
    {
        UpdateComponents();
    }

    private void UpdateComponents()
    {
        var children = GetChildren().OfType<BaseComponent>();
        foreach (var child in children)
        {
            child.Parent = GetOwner<Node3D>();
            Components[child.Name] = child;
        }
    }
}
