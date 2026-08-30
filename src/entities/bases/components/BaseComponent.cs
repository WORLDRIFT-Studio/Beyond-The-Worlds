using Godot;

namespace BeyondTheWorlds.enemies.bases;

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

        var parent = GetParent();

        if (GetTree().EditedSceneRoot == this)
            return warnings.ToArray();

        if (parent is not ComponentContainer && GetTree().EditedSceneRoot != this)
            warnings.Add("Component must be a child of ComponentContainer.");

        return warnings.ToArray();
    }
}
