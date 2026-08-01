using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using BeyondTheWorlds.common.debug_console;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class TableManager : Node
{
	#region Events
	[Signal] public delegate void NextTurnStartedEventHandler();
	[Signal] public delegate void NewBattleStartedEventHandler();
	[Signal] public delegate void StackChangedEventHandler();
	[Signal] public delegate void GraveyardChangedEventHandler();
	
	public static void EmitNextTurnStarted()
	{
		Instance.EmitSignalNextTurnStarted();
	}

	public static void EmitNewBattleStarted()
	{
		Instance.EmitSignalNewBattleStarted();
	}
	
	public static void EmitStackChanged()
	{
		Instance.EmitSignalStackChanged();
	}

	public static void EmitGraveyardChanged()
	{
		Instance.EmitSignalGraveyardChanged();
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
	
	[ExportGroup("User Interface")] 
	[Export] private Button _endTourButton;
	[Export] private Label _graveyardCards;
	[Export] private Label _deckCards;
	
	[ExportGroup("")]
	[Export] private PackedScene _cardBaseTscn;  
	#endregion
	
	
	public static TableManager Instance { get; private set; }
	
	public override  void _Ready()
	{
		Instance = this;
		_endTourButton.Pressed += RemoveCardsFromHand;
		_endTourButton.Pressed += AddCardsToHand;
		StackChanged += OnStackChanged;
		StackChanged += UpdateStackText;
		GraveyardChanged += UpdateGraveyardText;
		EmitSignalGraveyardChanged();
		EmitSignalStackChanged();
	}

	private void OnStackChanged()
	{
		
		if (_deckManager.GetCardsCount() == 0)
		{
			if (_graveyardManager.GetCardsCount() == 0)
			{
				DebugConsole.Log("WARNING", "CardSys", "There are no cards in the deck and on the graveyard!");
				return;
			}
			
			List<CardData> cardsToMove = new List<CardData>(_graveyardManager.GetCards());


			foreach (var card in cardsToMove)
			{
				_deckManager.AddCards(card);
			}			
			
			_graveyardManager.ClearGraveyard();
			UpdateGraveyardText();
			UpdateStackText();
		}
	}

	private void RemoveCardsFromHand()
	{
		foreach (CardBase card in _handManager.GetChildren().OfType<CardBase>())
		{
			CardData cardData = card.CardInfo;
			_graveyardManager.PushCard(cardData);
			_handManager.RemoveChild(card);
			card.QueueFree();
		}
	}
	
	private void AddCardsToHand()
	{

		while (_handManager.GetCardsCount() < _defaultCardsNumber && _deckManager.GetCardsCount() > 0)
		{
			CardData cardData = _deckManager.GetCard();
			var cardNode = _cardBaseTscn.Instantiate<CardBase>();
			cardNode.Initialize(cardData);
			_handManager.AddChild(cardNode);
			DebugConsole.Log("INFO", "CardSys", "Succesfully added card to Player hand.");
			//TODO: Dodać animacje dodawnia karty
		}
		
		_handManager.ArangeCards();
		EmitStackChanged();
	}

	private void UpdateGraveyardText() => _graveyardCards.Text = $"{_graveyardManager.GetCardsCount()}";
	private void UpdateStackText() => _deckCards.Text = $"{_deckManager.GetCardsCount()}";
}


	