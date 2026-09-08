// -----------------------------------------------------------------------
// <copyright file="AttackComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.common.debug_console;
using BeyondTheWorlds.entities.bases.resources;
using BeyondTheWorlds.entities.bases.types;
using BeyondTheWorlds.Interfaces;
using Godot;
using BaseComponent = BeyondTheWorlds.entities.bases.components.BaseComponent;

namespace BeyondTheWorlds.entities.components;

[GlobalClass]
[Icon("res://addons/at-icons/node3d/cutlass.svg")]
[Tool]
public partial class AttackComponent : BaseComponent
{
    [Export]
    private AttackData? _attackData;

    private ManaComponent? _manaComponent;

    [Export]
    private TargetingComponent? _targetingComponent;

    [Export]
    private ManaComponent? ManaComponent
    {
        get => _manaComponent;
        set
        {
            _manaComponent = value;
            UpdateConfigurationWarnings();
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (_attackData is MagicAttackData && _manaComponent is null)
            warnings.Add("Magical attack need a mana component for properly work");

        return [.. warnings];
    }

    private async void TryExecuteAttack()
    {
        if (_targetingComponent is null)
        {
            DebugConsole.Log(
                "ERROR",
                "Entities",
                $"TargetingComponent is null at '{Parent?.Name ?? "unknown"}'."
            );
            return;
        }

        if (_targetingComponent.Target is null)
            return;

        switch (_attackData)
        {
            case MeleeAttackData meleeAttackData:
                ExecuteAttack(meleeAttackData, _targetingComponent.Target);
                break;
            case MagicAttackData magicAttackData:
                await ExecuteAttack(magicAttackData, _targetingComponent.Target)
                    .ConfigureAwait(true);
                break;
            case RangedAttackData rangedAttackData:
                await ExecuteAttack(rangedAttackData, _targetingComponent.Target)
                    .ConfigureAwait(true);
                break;
            default:
                DebugConsole.Log(
                    "ERROR",
                    "Enemies",
                    $"Enemy '{Parent?.Name ?? "unknown"}' tryied to attack using unknown AttackData"
                );
                break;
        }
    }

    private void ExecuteAttack(MeleeAttackData attackData, Entity target)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(attackData.Damage);
    }

    private async Task ExecuteAttack(RangedAttackData attackData, Entity target)
    {
        if (attackData.ProjectileData?.ProjectileScene is null)
            return;

        // ReSharper disable once EmptyForStatement caused by possibility of higher projectile ammount
        for (int i = 0; i < attackData.ProjectileData.ProjectileAmountPerAttack; i++)
        {
            Projectile projectile =
                attackData.ProjectileData.ProjectileScene.Instantiate<Projectile>();
            projectile.Initialize(attackData.ProjectileData);
            projectile.GlobalPosition = GlobalPosition;
            projectile.LookAt(target.GlobalPosition);

            GetTree().GetCurrentScene().AddChild(projectile);
            await ToSignal(
                GetTree().CreateTimer(attackData.ProjectileData.Delay),
                SceneTreeTimer.SignalName.Timeout
            );
        }
    }

    private async Task ExecuteAttack(MagicAttackData attackData, Entity target)
    {
        if (attackData.ProjectileData?.ProjectileScene is null)
            return;

        if (_manaComponent?.CurrentMana < attackData.ManaCost)
            return;

        // ReSharper disable once EmptyForStatement caused by possibility of higher projectile ammount
        for (int i = 0; i < attackData.ProjectileData.ProjectileAmountPerAttack; i++)
        {
            Projectile projectile =
                attackData.ProjectileData.ProjectileScene.Instantiate<Projectile>();
            projectile.Initialize(attackData.ProjectileData);
            projectile.GlobalPosition = GlobalPosition;
            projectile.LookAt(target.GlobalPosition);

            GetTree().GetCurrentScene().AddChild(projectile);
            await ToSignal(
                GetTree().CreateTimer(attackData.ProjectileData.Delay),
                SceneTreeTimer.SignalName.Timeout
            );
        }
    }
}
