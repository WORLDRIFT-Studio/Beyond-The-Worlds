using Godot;
using System;
using System.Linq;
using BeyondTheWorlds.common.debug_console;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class TableManager : Node
{
	#region Events
	[Signal] public delegate void NextTurnStartedEventHandler();
	[Signal] public delegate void NewBattleStartedEventHandler();
	[Signal] public delegate void StackChangedEventHandler(short count);
	
	public static void EmitNextTurnStarted()
	{
		Instance.EmitSignalNextTurnStarted();
	}

	public static void EmitNewBattleStarted()
	{
		Instance.EmitSignalNewBattleStarted();
	}
	
	public static void EmitStackChanged(short count)
	{
		Instance.EmitSignalStackChanged(count);
	}
	#endregion

	
	
	#region Exports
	[ExportCategory("Table Manager")]
	
	[ExportGroup("Components")]
	[Export] private DeckManager _deckManager;
	[Export] private HandManager _handManager;
	[Export] private GraveyardManager _graveyardManager;
		
	
	[ExportGroup("Properties")]
	[Export(PropertyHint.Range, "1, 10, 1, prefer_slider")]
	private short _defaultCardsNumber = 5;
	
	[ExportGroup("Nodes")] 
	[Export] private Button _endTourButton;
	[Export] private PackedScene _cardBaseTscn;  
	#endregion
	

	
	public static TableManager Instance { get; private set; }
	
	public override void _Ready()
	{
		Instance = this;
		_endTourButton.Pressed += RemoveCardsFromHand;
		_endTourButton.Pressed += AddCardsToHand;
		EmitSignalNewBattleStarted();
	}

	private void RemoveCardsFromHand()
	{
		foreach (CardBase card in _handManager.GetChildren().OfType<CardBase>())
		{
			CardData cardData = card.CardInfo;
			card.QueueFree();
			_graveyardManager.PushCardToGraveyard(cardData);
		}
	}
	
	private void AddCardsToHand()
	{

		while (_handManager.CardOnHand.Count < _defaultCardsNumber && _deckManager.PlayerCards.Count > 0)
		{
			CardData cardData = _deckManager.GetCard();
			var cardNode = _cardBaseTscn.Instantiate<CardBase>();
			cardNode.Initialize(cardData);
			_handManager.AddChild(cardNode);
			DebugConsole.Log("INFO", "CardSys", "Succesfully added card to Player hand.");
			//TODO: Dodać animacje dodawnia karty
		}
		
		_handManager.ArangeCards();
	}

	
}


	