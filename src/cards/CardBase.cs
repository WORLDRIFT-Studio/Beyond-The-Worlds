using Godot;
using System;
using BeyondTheWorlds.common.debug_console;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class CardBase : Node2D
{
	private string _cardName;
	private Texture2D _cardTexture;
	private int _cardCost;
	private string _cardType;
	private string _cardClass;
	private string _cardDesc;
	public CardData CardInfo { get; private set; }

	[ExportGroup("Nodes")]
	[Export] private Label NodeCardName { get; set; }
	[Export] private Label NodeCardCost { get; set; }
	[Export] private TextureRect NodeCardTexture { get; set; }
	[Export] private RichTextLabel NodeCardDesc { get; set; }
	
	/// <summary>
	/// Inicjalizuje dane karty na podstawie wejścia
	/// </summary>
	/// <param name="cardInfo">Dane bazowe karty, trzymane w <see cref="CardData"/></param>
	public void Initialize(CardData cardInfo)
	{
		CardInfo = cardInfo;
		_cardName = cardInfo.CardName;
		_cardTexture = cardInfo.CardTexture;
		_cardCost = cardInfo.CardCost;
		_cardType = cardInfo.CardType;
		_cardClass = cardInfo.CardClass;
		_cardDesc = cardInfo.CardDescription;

		NodeCardName.Text = _cardName;
		NodeCardDesc.Text = _cardDesc;
		NodeCardCost.Text = $"{_cardCost}";
		NodeCardTexture.Texture = _cardTexture;
	}
}
