namespace BeyondTheWorlds.Interfaces;

public interface IHealable
{
    public bool IsFullHealed { get; }

    void Heal(int health);
}
