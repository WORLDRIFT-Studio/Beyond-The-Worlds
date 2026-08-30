// -----------------------------------------------------------------------
// <copyright file="SpawnSystem.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.entities.models;
using Godot;
using Timer = Godot.Timer;

namespace BeyondTheWorlds.entities;

public partial class SpawnSystem : Node3D
{
    private Vector3 _spawnPoint;
    private Timer? _spawnTimer;

    [Export]
    public float SpawnY { get; set; } = 0.5f;

    [Export]
    public Node3D? Target { get; set; }

    [Export(PropertyHint.Range, "0, 20, 1")]
    public float Timer { get; set; } = 5.0f;

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

        Vector3 meshPosition = SpawnArea.GlobalPosition;

        Aabb meshSize = SpawnArea.GetAabb();
        Vector3 globalScale = SpawnArea.GlobalBasis.Scale;

        float halfWidthX = meshSize.Size.X * globalScale.X / 2f;
        float halfDepthZ = meshSize.Size.Z * globalScale.Z / 2f;

        float minZ = meshPosition.Z - halfDepthZ;
        float maxZ = meshPosition.Z + halfDepthZ;

        float minX = meshPosition.X - halfWidthX;
        float maxX = meshPosition.X + halfWidthX;

        float randomX = new RandomNumberGenerator().RandfRange(minX, maxX);
        float randomZ = new RandomNumberGenerator().RandfRange(minZ, maxZ);

        float direction = new RandomNumberGenerator().RandiRange(1, 4);

        _spawnPoint = direction switch
        {
            1 => new Vector3(minX, SpawnY, randomZ),
            2 => new Vector3(maxX, SpawnY, randomZ),
            3 => new Vector3(randomX, SpawnY, minZ),
            4 => new Vector3(randomX, SpawnY, maxZ),
            _ => _spawnPoint,
        };

        Enemy enemy = EnemyScene.Instantiate<Enemy>();

        AddChild(enemy);
        enemy.Target = Target;
        enemy.GlobalPosition = _spawnPoint;
    }
}
