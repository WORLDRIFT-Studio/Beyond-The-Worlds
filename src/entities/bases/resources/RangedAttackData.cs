using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/bow_and_arrow.svg")]
public partial class RangedAttackData : AttackData
{
    [Export]
    public PackedScene? ProjectileScene { get; set; }
}
