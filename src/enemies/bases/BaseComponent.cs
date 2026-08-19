using System.Collections.Generic;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node/icons.svg")]
public abstract partial class BaseComponent : Node
{
    public override void _EnterTree()
    {
        UpdateConfigurationWarnings();
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (GetParent() is not ComponentContainer)
        {
            warnings.Add("Component must be a child of ComponentContainer");
        }

        return warnings.ToArray();
    }
}
