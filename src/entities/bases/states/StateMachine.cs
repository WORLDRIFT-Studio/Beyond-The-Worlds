using System.Collections.Generic;
using System.Linq;
using Godot;

namespace BeyondTheWorlds.enemies.bases;

[Tool]
[GlobalClass, Icon("res://addons/at-icons/node/cog.svg")]
public partial class  StateMachine: Node
{
    private Dictionary<string, State> _states = new Dictionary<string, State>();
    private Node _parent;
    
    [Export]
    private State _defaultState;
    private State _currentState;
    
    public override void _Ready()
    {
        _parent = GetParent();
        UpdateStates();
        ChildEnteredTree += _ => UpdateStates();
        ChildExitingTree += _ => UpdateStates();
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState.PhysicsUpdate(delta);
    }

    public override void _Process(double delta)
    {
        _currentState.Update(delta);
    }

    private void UpdateStates()
    {
        var children = GetChildren().OfType<State>();
        foreach (var state in children)
        {
            state.StateMachine = this;
            state.Parent = _parent;
            _states[state.Name] = state;
        }
    }
    
    public void ChangeState(string newState)
    {
        if (!_states.ContainsKey(newState)) return;
        _currentState.Exit();
        _currentState = _states[newState];
        _currentState.Enter();
    }
}