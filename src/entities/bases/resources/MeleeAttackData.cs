using System;
using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[GlobalClass]
[Icon("res://addons/at-icons/node3d/baseball_bat.svg")]
public partial class MeleeAttackData(int damage, double cooldown, ElementTypes attackElement)
    : AttackData(damage, cooldown, attackElement)
{
    [Export]
    public double KnockbackForce { get; set; } = 0.5;
}
