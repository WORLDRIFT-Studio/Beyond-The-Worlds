// -----------------------------------------------------------------------
// <copyright file="HealthComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.Interfaces;
using Godot;
using BaseComponent = BeyondTheWorlds.entities.bases.components.BaseComponent;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/heart.svg")]
public partial class HealthComponent : BaseComponent, IDamageable, IHealable
{
    [Signal]
    public delegate void HealthChangedEventHandler(double currentHealth, double maxHealth);

    [Signal]
    public delegate void DeathEventHandler();

    private double _currentHealth;

    private double _maxHealth;

    [Export(PropertyHint.Range, "1, 1000, 1, prefer_slider, or_greater, suffix:HP")]
    public double MaxHealth
    {
        get => _maxHealth;
        private set
        {
            double percentage = _currentHealth / _maxHealth;
            _maxHealth = value;
            _currentHealth += _currentHealth * percentage;
            if (Engine.IsEditorHint())
                return;
            EmitSignalHealthChanged(_currentHealth, _maxHealth);
        }
    }

    public double Health
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = Math.Clamp(value, 0, MaxHealth);
            if (!Engine.IsEditorHint())
                return;
            EmitSignalHealthChanged(_currentHealth, _maxHealth);
            if (!IsDead)
                return;
            EmitSignalDeath();
        }
    }

    public bool IsFullHealed => _currentHealth >= MaxHealth;
    public bool IsDead => _currentHealth <= 0;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
    }

    /// <summary>
    ///     Deal damage to object by given amount.
    /// </summary>
    /// <param name="damage">amount to take</param>
    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
            return;
        Health -= damage;
    }

    /// <summary>
    ///     Heal object by given amount.
    /// </summary>
    /// <param name="health">amount to heal</param>
    public void Heal(int health)
    {
        if (IsFullHealed || health <= 0)
            return;
        Health += health;
    }

    /// <summary>
    ///     Heal object by a given percentage of maximal health
    /// </summary>
    /// <param name="percent">max health percentage to heal</param>
    public void HealByPercent(double percent)
    {
        if (IsFullHealed || percent <= 0)
            return;
        Health += (int)(_maxHealth * percent);
    }
}
