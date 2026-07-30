using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeyondTheWorlds.common.debug_console;
using Godot;

namespace BeyondTheWorlds.cards;

[Tool]
[GlobalClass]
public partial class AnimationComponent : Node
{
    [ExportGroup("Nodes")]
    [Export]
    private Control TargetCard
    {
        get => _targetCard;
        set
        {
            _targetCard = value;
            UpdateConfigurationWarnings();
        }
    }
    [Export]
    public Control CardHitBox
    {
        get => _cardHitBox;
        set
        {
            _cardHitBox = value;
            UpdateConfigurationWarnings();
        }
    }
    
    [ExportGroup("Parameters")] 
    [Export(PropertyHint.Range, "1, 2, 0.01, prefer_slider")]
    private Vector2 _hoverScale;
    [Export(PropertyHint.Range, "0, 5, 0.01, prefer_slider, or_greater")]
    private double _duration;
    [Export(PropertyHint.Range, "0, 500, 1, prefer_slider, or_greater")] 
    private int _hoverHeight;
        
    private Control _targetCard;
    private Control _cardHitBox;
    
    
    public override string[] _GetConfigurationWarnings()
    {
        List<String> warnings = [];
        
        if (TargetCard == null) 
            warnings.Add("Komponent wymaga podpięcia następującego węzła: 'Target Card'");

        if (CardHitBox == null)
            warnings.Add("Komponent wymaga podpięcie następującego węzła: 'Card HitBox'");

        return warnings.ToArray();
    }

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        CardHitBox.MouseEntered += CardHovered;
        CardHitBox.MouseExited += CardDehovered;
    }

    public override void _ExitTree()
    {
        if (Engine.IsEditorHint()) return;
        CardHitBox.MouseEntered -= CardHovered;
        CardHitBox.MouseExited -= CardDehovered;
    }

    public async Task CardEntry(Vector2 checkpointPos, Vector2 targetPos, float targetRotation, double duration = .25d)
    {
        Tween tween = CreateTween()
            .BindNode(TargetCard)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine)
            .SetParallel();

        tween.TweenProperty(TargetCard, "position", checkpointPos, duration);
        tween.TweenProperty(TargetCard, "scale", new Vector2(1, 1), duration);
        tween.Chain().TweenInterval(0.4);
        tween.TweenProperty(TargetCard, "position", targetPos, duration);
        tween.TweenProperty(TargetCard, "rotation_degrees", targetRotation, duration);

        await ToSignal(tween, Tween.SignalName.Finished);
    }
    
    public async Task CardLeave(Vector2 checkpointPos, Vector2 targetPos, double duration = .25d)
    {
        Tween tween = CreateTween()
            .BindNode(TargetCard)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine)
            .SetParallel();
        
        tween.TweenProperty(TargetCard, "position", checkpointPos, duration);
        tween.TweenProperty(TargetCard, "rotation_degrees", 0, duration);
        tween.Chain().TweenInterval(0.4);
        tween.TweenProperty(TargetCard, "position", targetPos, duration);
        tween.TweenProperty(TargetCard, "scale", new Vector2(0.2f, 0.2f), duration);

        await ToSignal(tween, Tween.SignalName.Finished);
    }

    private void CardHovered()
    {
        Vector2 position = TargetCard.GetGlobalPosition();
        position.Y -= _hoverHeight;
        
        Tween tween = CreateTween()
            .BindNode(TargetCard)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine)
            .SetParallel();

        tween.TweenProperty(TargetCard, "scale", _hoverScale , _duration);
        tween.TweenProperty(TargetCard, "position", position, _duration);
    }

    private void CardDehovered()
    {
        Vector2 position = TargetCard.GetGlobalPosition();
        position.Y += _hoverHeight;
        
        Tween tween = CreateTween()
            .BindNode(TargetCard)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Sine)
            .SetParallel();

        tween.TweenProperty(TargetCard, "scale", new Vector2(1, 1) , _duration);
        tween.TweenProperty(TargetCard, "position", position, _duration);
    }
}