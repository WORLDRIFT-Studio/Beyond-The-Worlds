using System;
using Godot;

namespace BeyondTheWorlds.entities.bases.resources;

[GlobalClass]
[Icon("res://addons/at-icons/node3d/magic_wand.svg")]
public partial class MagicAttackData(
    int damage,
    int manaCost,
    double cooldown,
    ElementTypes attackElement,
    PackedScene? projectileScene
) : AttackData(damage, cooldown, attackElement)
{
    [Export]
    public PackedScene? ProjectileScene { get; set; } = projectileScene;

    [Export(PropertyHint.Range, "1, 10, 1, prefer_slider, or_greater, suffix:mana")]
    public int ManaCost { get; set; } = manaCost;

    public MagicAttackData()
        : this(0, 0, 0, ElementTypes.None, null) { }
}
