using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using BeyondTheWorlds.common.debug_console;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace BeyondTheWorlds.cards;

/// <summary>
/// Instancja menadżera gry, obsługuje System rozgrywki, zarządza grą i kartami. 
/// </summary>
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
	
	/// <summary>
	/// Instancja umożliwająca odwołanie się do niej w każdym skrypcie
	/// </summary>
	public static TableManager Instance { get; private set; }

	// Podłącza sygnały, inicjalizuje liczniki
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
	
	/// <summary>
	/// Aktaulizuję liczniki w przypadku pustego stosu dobierania.
	/// </summary>
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
				_deckManager.AddCards(card);
			
			_graveyardManager.ClearGraveyard();
			UpdateGraveyardText();
			UpdateStackText();
		}
	}

	
	/// <summary>
	/// Iteruję po dzieciach <see cref="HandManager"/>, ekstarkuje czyste dane karty i przesyła na cmentarz, usuwając węzęł na koniec
	/// </summary>
	private void RemoveCardsFromHand()
	{
		Array<CardBase> cards = new Array<CardBase>(_handManager.GetChildren().OfType<CardBase>());
		foreach (CardBase card in cards)
		{
			CardData cardData = card.CardInfo;
			_graveyardManager.PushCard(cardData);
			card.QueueFree();
		}
	}
	
	/// <summary>
	/// Instancjonuje puste obiekty kart, incjalizuje je przesyłając dane karty i dodaje je jako dzieci węzlą <see cref="HandManager"/>.
	/// Na końcu za pomocą metody <see cref="HandManager.ArangeCards"/>, ustawia je na właściwuch pozycjąch w ręce.
	/// </summary>
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

	/// <summary>
	/// Aktualizuje licznik kart na cmentarzu
	/// </summary>
	private void UpdateGraveyardText() => _graveyardCards.Text = $"{_graveyardManager.GetCardsCount()}";
	
	/// <summary>
	/// Aktualizuje licznik kart w tali
	/// </summary>
	private void UpdateStackText() => _deckCards.Text = $"{_deckManager.GetCardsCount()}";
}


	