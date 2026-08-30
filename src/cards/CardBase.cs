// -----------------------------------------------------------------------
// <copyright file="CardBase.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Godot;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class CardBase : Node2D
{
    private string? _cardClass;
    private int _cardCost;
    private string? _cardDesc;
    private string? _cardName;
    private Texture2D? _cardTexture;
    private string? _cardType;
    public CardData? CardInfo { get; private set; }

    [ExportGroup("Nodes")]
    [Export]
    private Label? NodeCardName { get; set; }

    [Export]
    private Label? NodeCardCost { get; set; }

    [Export]
    private TextureRect? NodeCardTexture { get; set; }

    [Export]
    private RichTextLabel? NodeCardDesc { get; set; }

    /// <summary>
    ///     Inicjalizuje dane karty na podstawie wejścia
    /// </summary>
    /// <param name="cardInfo">Dane bazowe karty, trzymane w <see cref="CardData" /></param>
    public void Initialize(CardData cardInfo)
    {
        CardInfo = cardInfo;
        _cardName = cardInfo.CardName;
        _cardTexture = cardInfo.CardTexture;
        _cardCost = cardInfo.CardCost;
        _cardType = cardInfo.CardType;
        _cardClass = cardInfo.CardClass;
        _cardDesc = cardInfo.CardDescription;

        if (
            NodeCardName == null
            || NodeCardDesc == null
            || NodeCardCost == null
            || NodeCardTexture == null
            || NodeCardDesc == null
        )
            return;

        NodeCardName.Text = _cardName;
        NodeCardDesc.Text = _cardDesc;
        NodeCardCost.Text = $"{_cardCost}";
        NodeCardTexture.Texture = _cardTexture;
    }
}
