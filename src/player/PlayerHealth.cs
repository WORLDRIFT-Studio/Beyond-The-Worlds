using System;
using BeyondTheWorlds.autoloads;
using Godot;

namespace BeyondTheWorlds.player;

public partial class PlayerHealth : Node
{
    /// <summary>
    ///     Zmienna CurrentHealth typu short (int16 bit) przechowuje aktualny stan zdrowia gracza
    ///     i pilnuje, aby nie wyjść poza maksymalny zakres (MaxHealth) poprzez set i get. W przypadku
    ///     gdy HP spadnie do 0, emituje stosowny sygnał, to samo w przypadku zmianny wartości.
    /// </summary>
    private short _currentHealth;

    public short MaxHealth { get; private set; } = 50;

    public short CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = (short)Mathf.Clamp(value, 0, MaxHealth);
            GD.Print($"HealthSystem: New HP value is {_currentHealth}/{MaxHealth}");
            Events.EmitPlayerHealthChanged(_currentHealth, MaxHealth);

            if (_currentHealth == 0)
            {
                GD.Print("HealthSystem: Player died!");
                Events.EmitPlayerDied();
            }
        }
    }

    public override void _Ready()
    {
        _currentHealth = MaxHealth;

        if (Events.Instance != null)
        {
            Events.Instance.PlayerHealed += OnPlayerHealed;
            Events.Instance.PlayerTakedDamage += OnPlayerTakedDamage;
        }
    }

    private void OnPlayerHealed(short value)
    {
        GD.Print($"HealthSystem: Healed {value} HP");
        CurrentHealth += value;
    }

    private void OnPlayerTakedDamage(short value)
    {
        GD.Print($"HealthSystem: Taked {value} Damage");
        CurrentHealth -= value;
    }

    private void OnNewGameStarted()
    {
        GD.Print("HealthSystem: HP reseted");
        _currentHealth = 50;
        MaxHealth = 50;
    }
}
