using Godot;

public partial class SpawnSystem : Node3D 
{
	[Export] public PackedScene EnemyScene {get;set;}
	[Export] public MeshInstance3D SpawnArea {get;set;}

	[Export] public float SpawnY = 0.5f;

	[Export] public Node3D Target;

	[Export(PropertyHint.Range, "0, 20, 1")] public float timer = 5.0f;
	private Timer SpawnTimer;

	private Vector3 SpawnPoint;

	public override void _Ready() 
	{
		SpawnTimer = GetNode<Timer>("SpawnTimer");

		SpawnTimer.WaitTime = timer;
		SpawnTimer.Start();
		SpawnTimer.Timeout += SpawnEnemy;
	}

	private void SpawnEnemy() 
	{
		if (EnemyScene == null || SpawnArea == null) return;

		Vector3 meshPosition = SpawnArea.GlobalPosition;

		Aabb meshSize = SpawnArea.GetAabb();
		Vector3 globalScale = SpawnArea.GlobalBasis.Scale;

		float halfWidthX = (meshSize.Size.X * globalScale.X) / 2f;
		float halfDepthZ = (meshSize.Size.Z * globalScale.Z) / 2f;

		float MinZ = meshPosition.Z - halfDepthZ;
		float MaxZ = meshPosition.Z + halfDepthZ;

		float MinX = meshPosition.X - halfWidthX;
		float MaxX = meshPosition.X + halfWidthX;

		float RandomX = new RandomNumberGenerator().RandfRange(MinX, MaxX);
		float RandomZ = new RandomNumberGenerator().RandfRange(MinZ, MaxZ);

		float Direction = new RandomNumberGenerator().RandiRange(1,4);

		SpawnPoint = Direction switch
		{
			1 => new Vector3(MinX, SpawnY, RandomZ),
			2 => new Vector3(MaxX, SpawnY, RandomZ),
			3 => new Vector3(RandomX, SpawnY, MinZ),
			4 => new Vector3(RandomX, SpawnY, MaxZ),
			_ => SpawnPoint
		};

		Enemy enemy = EnemyScene.Instantiate<Enemy>();

		AddChild(enemy);
		enemy.Target = Target;
		enemy.GlobalPosition = SpawnPoint;
	}
}
