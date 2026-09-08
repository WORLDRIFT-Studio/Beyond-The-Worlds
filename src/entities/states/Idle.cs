// -----------------------------------------------------------------------
// <copyright file="Idle.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;
using State = BeyondTheWorlds.entities.bases.states.State;
using StateMachine = BeyondTheWorlds.entities.bases.states.StateMachine;

namespace BeyondTheWorlds.entities.states;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/hand.svg")]
public partial class Idle : State
{
    private Node? _parent;
    private StateMachine? _stateMachine;

    public override StateMachine? StateMachine
    {
        get => _stateMachine;
        set => _stateMachine = value;
    }

    public override Node? Parent
    {
        get => _parent;
        set => _parent = value;
    }
}
