using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.enemies.states;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node/hand.svg")]
public partial class Idle : State
{
    private StateMachine _stateMachine;
    private Node _parent;

    public override StateMachine StateMachine
    {
        get => _stateMachine;
        set => _stateMachine = value;
    }

    public override Node Parent
    {
        get => _parent;
        set => _parent = value;
    }
}
