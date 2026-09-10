// -----------------------------------------------------------------------
// <copyright file="EntityFlags.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BeyondTheWorlds.entities.enums;

[Flags]
public enum EntityFlags
{
    Enemy = 1,
    Hero = 1 << 1,
    Building = 1 << 2,
}
