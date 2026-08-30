// -----------------------------------------------------------------------
// <copyright file="IDamageable.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BeyondTheWorlds.Interfaces;

public interface IDamageable
{
    bool IsDead { get; }
    double MaxHealth { get; }
    double CurrentHealth { get; }

    void TakeDamage(int damage);
}
