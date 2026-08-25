using System.Collections.Generic;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node/cog.svg")]
public abstract partial class State : Node
{
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (GetParent() is not bases.StateMachine)
            warnings.Add("State must be a children of StateMachine");

        return warnings.ToArray();
    }

    public abstract StateMachine StateMachine { get; set; }
    public abstract Node Parent { get; set; }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update(double delta) { }

    public virtual void PhysicsUpdate(double delta) { }
}
