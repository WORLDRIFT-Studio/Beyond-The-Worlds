using System.Collections.Generic;
using System.Linq;
using BeyondTheWorlds.common.debug_console;
using Godot;

namespace BeyondTheWorlds.cards;

/// <summary>
/// Zarządza ręką gracza (by nie walił konia)
/// </summary>
[GlobalClass]
public partial class HandManager : Node
{
	[Signal]
	public delegate void CardsAmmountChangedEventHandler();
	
	#region Variables
	
	/// <summary>
	/// Krzywa ręki
	/// </summary>
	[ExportCategory("Cards Variables")]
	[ExportGroup("Variables")]
	[Export] private Curve CardsArc { get; set; }
	/// <summary>
	/// Odstęp bazowy między kartami. Jeśli szerokoś
	/// </summary>
	[Export(PropertyHint.Range, "0, 200, 1, prefer_slider")]
	private short CardSpacing { get; set; } = 150;
	/// <summary>
	/// Ostrość łuku
	/// </summary>
	[Export(PropertyHint.Range, "0, 200, 1, prefer_slider")]
	private short ArcStrength { get; set; } = 100;
	[Export(PropertyHint.Range, "0, 2000, 10, prefer_slider")]
	private float BaseY { get; set; } = 1200;
	///Maksymalny obrótw karty	
	[Export(PropertyHint.Range, "0, 90, 1, prefer_slider")]
	private int MaxRotation { get; set; } = 20;
	
	[ExportGroup("Nodes")]
	[Export] private PackedScene CardBaseTscn { get; set; }

	public List<Node2D> CardOnHand { get; private set; } = [];
	private float CenterX { get; set; } = 960;
	private float MaxWidth { get; set; } = 1000f;
	
	#endregion
	
	
	public override  void _Ready()
	{
		CardsAmmountChanged += ArangeCards;
		// LoadPlayerDeck();
	}
	
	/// <summary>
	/// Ustala i ustawia pozycje kart
	/// </summary>
	public void ArangeCards()
	{
		CardOnHand = new List<Node2D>(GetChildren().OfType<Node2D>());
		int cardsCount = CardOnHand.Count;
		if (cardsCount == 0) return;
		
		int currentWidth = cardsCount * CardSpacing;
		float currentSpacing = currentWidth > MaxWidth ? MaxWidth/cardsCount : CardSpacing;
		float xOffset = (cardsCount - 1) * currentSpacing / 2;

		for (int i = 0; i < cardsCount; i++)
		{
			float xPos = CenterX + i * currentSpacing - xOffset ;
			float weight = cardsCount > 1 ? 2f * i / (cardsCount - 1) - 1 : 0f;
			float yPos = BaseY + CardsArc.Sample(weight) * -ArcStrength;
			CardOnHand[i].Position = new Vector2(xPos, yPos);
			CardOnHand[i].RotationDegrees = weight * MaxRotation;
			// TODO Zrobić dynamiczne pochylenie kart
			// TODO Zrobić animacje kart
		}
	}

	/// <summary>
	/// Zwraca liczbę kart na ręce
	/// </summary>
	public int GetCardsCount()
	{
		return GetChildren().OfType<CardBase>().Count();
	}

	// private void LoadPlayerDeck()
	// {
	// 	CardData cardData = GD.Load<CardData>("src/cards/cards_bases/test_card.tres");
	// 	var scena = CardBaseTscn.Instantiate<CardBase>();
	// 	scena.Initialize(cardData);
	// 	AddChild(scena);
	// 	ArangeCards();
	// }
}

