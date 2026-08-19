namespace BeyondTheWorlds.Interfaces;

public interface IDamageable
{
    bool IsDead { get; }
    double MaxHealth { get; }
    double CurrentHealth { get; }

    void TakeDamage(int damage);
}
