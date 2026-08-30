using BeyondTheWorlds.enemies.bases;
using Godot;
using Entity = BeyondTheWorlds.entities.bases.Entity;

namespace BeyondTheWorlds.entities.components;

[Tool]
[GlobalClass]
[Icon("res://addons/at-icons/node3d/bullseye.svg")]
public partial class TargetingComponent : BaseComponent
{
    [Export]
    private RangeComponent? _range;

    private double _timer;

    [Export(PropertyHint.Range, "0.01, 1, 0.01, prefer_slider, suffix:s")]
    private double RefreshTime { get; set; } = .2d;

    public Entity? Target { get; private set; }

    public bool HaveTarget => Target is not null;

    public override void _PhysicsProcess(double delta)
    {
        _timer += delta;
        if (_timer < RefreshTime)
            return;

        SetTarget();
        _timer -= RefreshTime;
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (_range is null)
            warnings.Add("Missing range component. Add it in inspector.");

        return [.. warnings, .. base._GetConfigurationWarnings()];
    }

    public void ClearTarget()
    {
        Target = null;
    }

    private void SetTarget()
    {
        if (Parent is null)
            return;

        Target = _range
            ?.EntitiesInRange?.OrderBy(e =>
                e.GlobalPosition.DistanceSquaredTo(Parent.GlobalPosition)
            )
            .FirstOrDefault();
    }
}
