using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.entities.bases.states;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/cog.svg")]
public partial class StateMachine : Node
{
    private readonly Dictionary<string, State> _states = new(StringComparer.Ordinal);
    private State? _currentState;

    [Export]
    private State? _defaultState;

    private Node? _parent;

    public override void _Ready()
    {
        _parent = GetParent();
        UpdateStates();
        ChildEnteredTree += _ => UpdateStates();
        ChildExitingTree += _ => UpdateStates();
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState?.PhysicsUpdate(delta);
    }

    public override void _Process(double delta)
    {
        _currentState?.Update(delta);
    }

    public void ChangeState(string newState)
    {
        if (!_states.ContainsKey(newState))
            return;
        _currentState?.Exit();
        _currentState = _states[newState];
        _currentState.Enter();
    }

    private void UpdateStates()
    {
        IEnumerable<State> children = GetChildren().OfType<State>();
        foreach (State state in children)
        {
            state.StateMachine = this;
            state.Parent = _parent;
            _states[state.Name] = state;
        }
    }
}
