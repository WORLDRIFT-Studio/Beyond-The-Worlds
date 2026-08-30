using BeyondTheWorlds.autoloads;
using Godot;

namespace BeyondTheWorlds.entities.models;

public partial class Enemy : CharacterBody3D
{
    private NavigationAgent3D? _navAgent;

    [Export]
    public Node3D? Target { get; set; }

    public override async void _Ready()
    {
        _navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

        _navAgent.VelocityComputed += OnVelocityComputed;
        _navAgent.TargetReached += OnTargetReached;

        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        UpdateTargetPosition();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_navAgent == null || _navAgent.IsNavigationFinished())
            return;

        var nextPosition = _navAgent.GetNextPathPosition();
        var direction = (nextPosition - GlobalPosition).Normalized();

        _navAgent.SetVelocity(direction * Speed);

        if (direction.Length() > 0)
        {
            var lookTarget = new Vector3(nextPosition.X, GlobalPosition.Y, nextPosition.Z);
            if (GlobalPosition.DistanceTo(lookTarget) > 0.01)
                LookAt(lookTarget, Vector3.Up);
        }
    }

    private const float Speed = 3.0f;

    private void OnTargetReached()
    {
        GD.Print("Target reached!");
        Events.EmitPlayerDamaged(1);
        QueueFree();
    }

    private void OnVelocityComputed(Vector3 safeVelocity)
    {
        Velocity = Velocity.MoveToward(safeVelocity, 0.25f);
        MoveAndSlide();
    }

    private void UpdateTargetPosition()
    {
        if (Target != null && IsInstanceValid(Target) && _navAgent != null)
            _navAgent.TargetPosition = Target.GlobalPosition;
    }
}
