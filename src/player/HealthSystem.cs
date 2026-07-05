using BeyondTheWorlds.autoloads;
using Godot;

namespace BeyondTheWorlds.player;

public partial class HealthSystem : Node
{
	public short MaxHealth { get; private set; } = 50;
	
	/// <summary>
	/// Zmienna CurrentHealth typu short (int16 bit) przechowuje aktualny stan zdrowia gracza
	/// i pilnuje, aby nie wyjść poza maksymalny zakres (MaxHealth) poprzez set i get. W przypadku
	/// gdy HP spadnie do 0, emituje stosowny sygnał, to samo w przypadku zmianny wartości.
	/// </summary>
	private short _currentHealth;
	public short CurrentHealth
	{
		get => _currentHealth;
		private set
		{
			_currentHealth = (short) Mathf.Clamp(value, 0, MaxHealth);
			Events.EmitPlayerHealthChanged(_currentHealth, MaxHealth);
			if (_currentHealth == 0) Events.EmitPlayerDied();
		}
	}

	
	public override void _Ready()
	{
		_currentHealth = MaxHealth;
		
		Events.Instance.PlayerHealed += OnPlayerHealed;
		Events.Instance.PlayerTakedDamage += OnPlayerTakedDamage;
	}
	
	
	private void OnPlayerHealed(short value) => CurrentHealth += value;
	private void OnPlayerTakedDamage(short value) =>  CurrentHealth -= value;

	private void OnNewGameStarted()
	{
		_currentHealth = 50;
		MaxHealth = 50;
	}
}