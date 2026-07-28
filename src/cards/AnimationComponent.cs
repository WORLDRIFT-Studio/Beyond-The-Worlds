using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace BeyondTheWorlds.cards;

[Tool]
[GlobalClass]
public partial class AnimationComponent : Node
{
    private Node _targetCard;

    [Export]
    private Node TargetCard
    {
        get => _targetCard;
        set
        {
            _targetCard = value;
            _GetConfigurationWarnings();
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<String> warnings = [];
        if (TargetCard != null) 
            return base._GetConfigurationWarnings();
        
        if (TargetCard == null) 
            warnings.Add("Komponent wymaga podpięcia następującego węzła: 'Target Card'");

        return warnings.ToArray();
    }

    public async void CardEntry(Vector2 checkpointPos, Vector2 targetPos, float targetRotation, double duration = .25d)
    {
        Tween tween = CreateTween()
            .BindNode(TargetCard)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine);

        tween.TweenProperty(TargetCard, "position", checkpointPos, duration);
        tween.TweenProperty(TargetCard, "scale", new Vector2(1, 1), duration);
        tween.TweenInterval(0.2);
        tween.TweenProperty(TargetCard, "position", targetPos, duration);
        tween.TweenProperty(TargetCard, "rotation_degrees", targetRotation, duration);
    }

    public void CardLeave()
    {
        throw new NotImplementedException();
    }

    public void CardHover()
    {
        throw new NotImplementedException();
    }
}