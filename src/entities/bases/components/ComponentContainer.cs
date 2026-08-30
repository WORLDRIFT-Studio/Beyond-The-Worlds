using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/archive.svg")]
public partial class ComponentContainer : Node3D
{
    public Godot.Collections.Dictionary<string, BaseComponent> Components { get; set; } = new();

    public override void _Ready()
    {
        UpdateComponents();
    }

    public T? GetComponent<T>()
        where T : class
    {
        return Components.Values.OfType<T>().FirstOrDefault();
    }

    public IList<T> GetComponents<T>()
        where T : class
    {
        return Components.Values.OfType<T>().ToList();
    }

    private void UpdateComponents()
    {
        IEnumerable<BaseComponent> children = GetChildren().OfType<BaseComponent>();
        foreach (BaseComponent child in children)
        {
            child.Parent = GetOwner<Node3D>();
            Components[child.Name] = child;
        }
    }
}
