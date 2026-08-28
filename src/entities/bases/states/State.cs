using System;
using System.Collections.Generic;
using BeyondTheWorlds.entities.bases.states;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/cog.svg")]
public abstract partial class State : Node
{
    public abstract StateMachine? StateMachine { get; set; }
    public abstract Node? Parent { get; set; }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (GetParent() is not entities.bases.states.StateMachine)
            warnings.Add("State must be a children of StateMachine");

        return warnings.ToArray();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update(double delta) { }

    public virtual void PhysicsUpdate(double delta) { }
}
