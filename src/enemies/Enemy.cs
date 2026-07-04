using Godot;

public partial class Enemy : CharacterBody3D 
{
	[Export] public Node3D Target {get;set;}
	private NavigationAgent3D NavAgent;
	const float SPEED = 3.0f;

	public override async void _Ready() 
	{
		NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		NavAgent.VelocityComputed += OnVelocityComputed;
		NavAgent.TargetReached += OnTargetReached;

		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		UpdateTargetPosition();
	}

	private void OnTargetReached() 
	{
		GD.Print("Target reached!");
		QueueFree();
	}

	private void OnVelocityComputed(Vector3 safe_velocity) 
	{
		Velocity = Velocity.MoveToward(safe_velocity, 0.25f);
		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta) 
	{
		if (NavAgent.IsNavigationFinished())
		{
			return;
		}

		Vector3 NextPosition = NavAgent.GetNextPathPosition();
		Vector3 Direction = (NextPosition - GlobalPosition).Normalized();

		NavAgent.SetVelocity(Direction * SPEED);

		if (Direction.Length() > 0)
		{
			Vector3 LookTarget = new Vector3(NextPosition.X, GlobalPosition.Y, NextPosition.Z);
			if (GlobalPosition.DistanceTo(LookTarget) > 0.01)
			{
				LookAt(LookTarget, Vector3.Up);
			}
		}
	}

	private void UpdateTargetPosition() 
	{
		if (Target != null && IsInstanceValid(Target))
		{
			NavAgent.TargetPosition = Target.GlobalPosition;
		}
	}
}
