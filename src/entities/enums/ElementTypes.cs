// -----------------------------------------------------------------------
// <copyright file="ElementTypes.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BeyondTheWorlds.entities.enums;

[Flags]
public enum ElementTypes
{
    None = 0,
    Fire = 1 << 0,
    Water = 1 << 1,
    Earth = 1 << 2,
    Air = 1 << 3,
    Light = 1 << 4,
    Dark = 1 << 5,
}
