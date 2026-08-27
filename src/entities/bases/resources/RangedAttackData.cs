using System;
using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[GlobalClass]
[Icon("res://addons/at-icons/node3d/bow_and_arrow.svg")]
public partial class RangedAttackData(
    int damage,
    double cooldown,
    ElementTypes attackElement,
    PackedScene? projectileScene
) : AttackData(damage, cooldown, attackElement)
{
    [Export]
    public PackedScene? ProjectileScene { get; set; } = projectileScene;
}
