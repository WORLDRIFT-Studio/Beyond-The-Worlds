using System;
using BeyondTheWorlds.entities.models;
using Godot;

public partial class SpawnSystem : Node3D
{
    [Export]
    public float SpawnY = 0.5f;

    [Export]
    public Node3D? Target;

    [Export(PropertyHint.Range, "0, 20, 1")]
    public float Timer = 5.0f;

    private Vector3 _spawnPoint;
    private Timer? _spawnTimer;

    [Export]
    public PackedScene? EnemyScene { get; set; }

    [Export]
    public MeshInstance3D? SpawnArea { get; set; }

    public override void _Ready()
    {
        _spawnTimer = GetNode<Timer>("SpawnTimer");

        _spawnTimer.WaitTime = Timer;
        _spawnTimer.Start();
        _spawnTimer.Timeout += SpawnEnemy;
    }

    private void SpawnEnemy()
    {
        if (EnemyScene == null || SpawnArea == null)
            return;

        var meshPosition = SpawnArea.GlobalPosition;

        var meshSize = SpawnArea.GetAabb();
        var globalScale = SpawnArea.GlobalBasis.Scale;

        var halfWidthX = meshSize.Size.X * globalScale.X / 2f;
        var halfDepthZ = meshSize.Size.Z * globalScale.Z / 2f;

        var minZ = meshPosition.Z - halfDepthZ;
        var maxZ = meshPosition.Z + halfDepthZ;

        var minX = meshPosition.X - halfWidthX;
        var maxX = meshPosition.X + halfWidthX;

        var randomX = new RandomNumberGenerator().RandfRange(minX, maxX);
        var randomZ = new RandomNumberGenerator().RandfRange(minZ, maxZ);

        float direction = new RandomNumberGenerator().RandiRange(1, 4);

        _spawnPoint = direction switch
        {
            1 => new Vector3(minX, SpawnY, randomZ),
            2 => new Vector3(maxX, SpawnY, randomZ),
            3 => new Vector3(randomX, SpawnY, minZ),
            4 => new Vector3(randomX, SpawnY, maxZ),
            _ => _spawnPoint,
        };

        var enemy = EnemyScene.Instantiate<Enemy>();

        AddChild(enemy);
        enemy.Target = Target;
        enemy.GlobalPosition = _spawnPoint;
    }
}
