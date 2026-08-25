using System;
using BeyondTheWorlds.enemies.bases;
using BeyondTheWorlds.Interfaces;
using Godot;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/heart.svg")]
public partial class HealthComponent : BaseComponent, IDamageable, IHealable
{
    private double _currentHealth;

    private double _maxHealth;

    [Export(PropertyHint.Range, "1, 1000, 1, prefer_slider, or_greater, suffix:HP")]
    public double MaxHealth
    {
        get => _maxHealth;
        private set
        {
            var percentage = _currentHealth / _maxHealth;
            _maxHealth = value;
            _currentHealth += _currentHealth * percentage;
            if (Engine.IsEditorHint())
                return;
            EmitSignalHealthChanged(_currentHealth, _maxHealth);
        }
    }

    public double CurrentHealth
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
        CurrentHealth -= damage;
    }

    /// <summary>
    ///     Heal object by given amount.
    /// </summary>
    /// <param name="health">amount to heal</param>
    public void Heal(int health)
    {
        if (IsFullHealed || health <= 0)
            return;
        CurrentHealth += health;
    }

    /// <summary>
    ///     Heal object by a given percentage of maximal health
    /// </summary>
    /// <param name="percent">max health percentage to heal</param>
    public void HealByPercent(double percent)
    {
        if (IsFullHealed || percent <= 0)
            return;
        CurrentHealth += (int)(_maxHealth * percent);
    }

    [Signal]
    public delegate void HealthChangedEventHandler(double currentHealth, double maxHealth);

    [Signal]
    public delegate void DeathEventHandler();
}
