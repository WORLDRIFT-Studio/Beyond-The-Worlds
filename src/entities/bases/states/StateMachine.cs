// -----------------------------------------------------------------------
// <copyright file="StateMachine.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.entities.bases.states;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node/cog.svg")]
public partial class StateMachine : Node
{
    private State? _currentState;
    private State? _defaultState;

    private Node? _parent;

    private readonly Dictionary<string, State> _states = new(StringComparer.Ordinal);

    [Export]
    private State? DefaultState
    {
        get => _defaultState;
        set
        {
            _defaultState = value;
            UpdateConfigurationWarnings();
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];
        if (_defaultState == null)
            warnings.Add(
                "Default state is null. This node for work need a default state. Add it in inscpector."
            );
        return [.. warnings];
    }

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
