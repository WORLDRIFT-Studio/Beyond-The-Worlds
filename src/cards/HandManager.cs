using System.Collections.Generic;
using System.Linq;
using BeyondTheWorlds.common.debug_console;
using Godot;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class HandManager : Node
{
	[Signal]
	public delegate void CardsAmmountChangedEventHandler();
	
	#region Variables
	
	[ExportCategory("Cards Variables")]
	[ExportGroup("Variables")]
	
	[Export] 
	private Curve CardsArc { get; set; }
	
	[Export(PropertyHint.Range, "0, 200, 1, prefer_slider")]
	private short CardSpacing { get; set; } = 150;

	[Export(PropertyHint.Range, "0, 200, 1, prefer_slider")]
	private short ArcStrength { get; set; } = 100;
	
	[Export(PropertyHint.Range, "0, 2000, 10, prefer_slider")]
	private float BaseY { get; set; } = 1200;

	[Export(PropertyHint.Range, "0, 90, 1, prefer_slider")]
	private int MaxRotation { get; set; } = 20;
	

	[ExportGroup("Nodes")] 
	[Export] private PackedScene _cardBaseTscn;
	[Export] private Control _cardCentralPoint;
	

	public List<CardBase> CardOnHand { get; private set; } = [];
	private float CenterX { get; set; } = 960;
	private float MaxWidth { get; set; } = 1000f;
	
	#endregion
	
	
	public override  void _Ready()
	{
		CardsAmmountChanged += ArangeCards;
		// LoadPlayerDeck();
	}
	
	public async void ArangeCards()
	{
		CardOnHand = new List<CardBase>(GetChildren().OfType<CardBase>());
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
			Vector2 newPos = new Vector2(xPos, yPos);
			float newRotation = weight * MaxRotation;
			CardBase currentCard = CardOnHand[i];
			
			currentCard.AnimationComponent.CardEntry(_cardCentralPoint.GlobalPosition, newPos, newRotation);
			await ToSignal(GetTree().CreateTimer(0.25), SceneTreeTimer.SignalName.Timeout);
			// TODO Zrobić dynamiczne pochylenie kart
			// TODO Zrobić animacje kart
		}
	}

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

