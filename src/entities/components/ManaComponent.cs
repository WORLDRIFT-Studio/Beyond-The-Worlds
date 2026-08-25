using System;
using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/magic_wand.svg")]
public partial class ManaComponent : BaseComponent
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
