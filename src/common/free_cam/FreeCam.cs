using Godot;
using System;

namespace BeyondTheWorlds.common.free_cam;
public partial class FreeCam:CharacterBody3D
{	
	/// <summary>
	/// Skrypt kontrolujący pracę free camery.
	/// Odpowiada za swobodne poruszanie się kamery w przestrzeni 3D za pomocą klawiatury (WASD + SHIFT/SPACE) oraz obrót kamery za pomocą myszy.
	/// </summary>
	
	#region MoveParameters

	[ExportCategory("Move Parameters")]
	
	[Export(PropertyHint.Range, "0.001, 1, 0.01, prefer_slider")]
	public float Sensitive { get; private set; } = 0.001f;
	
	[Export(PropertyHint.Range, "10, 100, 0.1, prefer_slider")]
	public float Speed { get; private set; } = 10f;
	
	[Export(PropertyHint.Range, "1, 10, 0.5, prefer_slider")]
	public float Gravity { get; private set; } = 0.5f;
	
	#endregion

	#region Nodes

	private Camera3D MainCamera3D => GetNode<Camera3D>("%Camera3D");

	#endregion

	#region Logic

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		float moveUpDown = 0.0f;

		if (Input.IsActionPressed("move_up")) 
			moveUpDown += Gravity;
		if (Input.IsActionPressed("move_down")) 
			moveUpDown -= Gravity;

		Vector3 dir = (MainCamera3D.Transform.Basis * new Vector3(inputDir.X, moveUpDown, inputDir.Y)).Normalized();

		Velocity = dir != Vector3.Zero
			? Velocity.MoveToward(dir * Speed,
				(float)(200 * delta))
			: Velocity.MoveToward(Vector3.Zero,
				(float)(200 * delta));

		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			
			Vector3 currentRotation = MainCamera3D.Rotation;
			currentRotation.Y -= mouseMotion.Relative.X * Sensitive;
			currentRotation.X -= mouseMotion.Relative.Y * Sensitive;
			GD.Print("Mouse Motion: " + currentRotation);
			currentRotation.X = Mathf.Clamp(currentRotation.X, -Mathf.Pi/2, Mathf.Pi/2);
			MainCamera3D.Rotation = currentRotation;
		}
	}

	#endregion
}
