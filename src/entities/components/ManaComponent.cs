// -----------------------------------------------------------------------
// <copyright file="ManaComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;
using BaseComponent = BeyondTheWorlds.entities.bases.components.BaseComponent;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/magic_wand.svg")]
public partial class ManaComponent : BaseComponent
{
    [Signal]
    public delegate void ManaChangedEventHandler(int currentMana, int maxMana);

    private int _currentMana;
    private int _maxMana;

    [Export]
    public int MaxMana
    {
        get => _maxMana;
        set
        {
            _maxMana = value;
            if (Engine.IsEditorHint())
                return;
            EmitSignalManaChanged(CurrentMana, _maxMana);
        }
    }

    public int CurrentMana
    {
        get => _currentMana;
        set
        {
            _currentMana = value;
            if (Engine.IsEditorHint())
                return;
            EmitSignalManaChanged(_currentMana, _maxMana);
        }
    }

    public bool IsFullMana => CurrentMana == _maxMana;
    public bool IsEmptyMana => CurrentMana <= 0;

    public override void _Ready()
    {
        CurrentMana = MaxMana;
    }

    public void TakeMana(int amount)
    {
        if (IsEmptyMana)
            return;
        CurrentMana -= amount;
    }

    public void AddMana(int amount)
    {
        if (IsFullMana)
            return;
        CurrentMana += amount;
    }
}
