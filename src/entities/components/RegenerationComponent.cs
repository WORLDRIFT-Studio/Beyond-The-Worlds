// -----------------------------------------------------------------------
// <copyright file="RegenerationComponent.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.enemies.bases;
using Godot;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/medkit.svg")]
public partial class RegenerationComponent : BaseComponent
{
    private double _clock;
    private HealthComponent? _healthComponent;
    private double _regenerationPercent;
    private double _regenerationRate;

    /// <summary>
    ///     Healing interval. Cant be negative.
    /// </summary>
    [Export(PropertyHint.Range, "0, 100, 0.1, or_greater, prefer_slider, suffix:s")]
    public double RegenerationRate
    {
        get => _regenerationRate;
        set
        {
            if (value <= 0)
                return;
            _regenerationRate = value;
        }
    }

    /// <summary>
    ///     Health percentage to heal. Cant be negative.
    /// </summary>
    [Export(PropertyHint.Range, "0, 100, 0.1, or_greater, prefer_slider, suffix:HP")]
    public double RegenerationPercent
    {
        get => _regenerationPercent;
        set
        {
            if (value <= 0)
                return;
            _regenerationPercent = value;
        }
    }

    /// <summary>
    ///     Reference to HealthComponent for healing
    /// </summary>
    [Export]
    public HealthComponent? HealthComponent
    {
        get => _healthComponent;
        private set
        {
            _healthComponent = value;
            UpdateConfigurationWarnings();
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;
        _clock += delta;
        if (!(_clock >= _regenerationRate))
            return;
        HealthComponent?.HealByPercent(_regenerationRate);
        _clock -= _regenerationRate;
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (HealthComponent == null)
            warnings.Add("Missing HealthComponent. Add missing components.");

        return [.. warnings, .. base._GetConfigurationWarnings()];
    }
}
