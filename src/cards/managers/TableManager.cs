using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeyondTheWorlds.common.debug_console;

namespace BeyondTheWorlds.cards.managers;

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
	
	public static void EmitNextTurnStarted() => Instance.EmitSignalNextTurnStarted();
	public static void EmitNewBattleStarted() => Instance.EmitSignalNewBattleStarted();
	public static void EmitStackChanged() => Instance.EmitSignalStackChanged();
	public static void EmitGraveyardChanged() => Instance.EmitSignalGraveyardChanged();
	
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
	[ExportSubgroup("Card Anchor Points")]
	[Export] private Control _cardSpawnPoint;
	[Export] private Control _cardDespawnPoint;
	[Export] private Control _cardCentralPoint;
	
	[ExportGroup("")]
	[Export] private PackedScene _cardBaseTscn;
	[Export] private PackedScene _cardReverseDummy;
	#endregion
	
	/// <summary>
	/// Instancja umożliwająca odwołanie się do niej w każdym skrypcie
	/// </summary>
	public static TableManager Instance { get; private set; }

	// Podłącza sygnały, inicjalizuje liczniki
	public override  void _Ready()
	{
		Instance = this;
		_endTourButton.Pressed += OnEndTourButtonPress;
		StackChanged += UpdateStackText;
		GraveyardChanged += UpdateGraveyardText;
		EmitSignalGraveyardChanged();
		EmitSignalStackChanged();
	}

	/// <summary>
	/// Zarządza przebiegiem akcji po zakończeniu tury
	/// </summary>
	private void OnEndTourButtonPress() => _ = ExecuteEndTour();

	private async Task ExecuteEndTour()
	{
		_endTourButton.Disabled = true;
		
		await RemoveCardsFromHand();
		await AddCardsToHand();

		_endTourButton.Disabled = false;
	}
	/// <summary>
	/// Iteruję po dzieciach <see cref="HandManager"/>, ekstarkuje czyste dane karty i przesyła na cmentarz, usuwając węzęł na koniec
	/// </summary>
	private async Task RemoveCardsFromHand()
	{
		CardBase[] cards = _handManager.GetChildren().OfType<CardBase>().ToArray();
		
		foreach (CardBase card in cards)
		{
			await card.AnimationComponent.CardLeave(_cardCentralPoint.GlobalPosition, _cardDespawnPoint.GlobalPosition);
			CardData cardData = card.CardInfo;
			_graveyardManager.PushCard(cardData);
			card.QueueFree();
		}
	}
	/// <summary>
	/// Instancjonuje puste obiekty kart, incjalizuje je przesyłając dane karty i dodaje je jako dzieci węzlą <see cref="HandManager"/>.
	/// Na końcu za pomocą metody <see cref="HandManager.ArangeCards"/>, ustawia je na właściwuch pozycjąch w ręce.
	/// </summary>
	private async Task AddCardsToHand()
	{

		while (_handManager.GetCardsCount() < _defaultCardsNumber)
		{
			if (_deckManager.GetCardsCount() == 0)
			{
				if (_graveyardManager.GetCardsCount() == 0)
					break;

				await ReshuffleGraveyardIntoDeck();
			}
			
			CardData cardData = _deckManager.GetCard();
			CardBase cardNode = _cardBaseTscn.Instantiate<CardBase>();
			cardNode.Initialize(cardData);
			cardNode.SetGlobalPosition(_cardSpawnPoint.GlobalPosition);
			_handManager.AddChild(cardNode);
			
			DebugConsole.Log("INFO", "CardSys", "Succesfully added card to Player hand.");
			// EmitStackChanged();
			
		}
		
		await _handManager.ArangeCardsAsync();
	}
	
	
	private async Task ReshuffleGraveyardIntoDeck()
	{
			
			List<CardData> cardsToMove = new List<CardData>(_graveyardManager.GetCards());
			
			_graveyardManager.ClearGraveyard();
			EmitGraveyardChanged();
			
			List<Task> animation = [];

			
			foreach (var card in cardsToMove)
			{
				_deckManager.AddCards(card);
				
				Control node = (Control) _cardReverseDummy.Instantiate();
				node.SetGlobalPosition(_cardDespawnPoint.GlobalPosition);
				AddChild(node);

				await ToSignal(GetTree().CreateTimer(.1d), SceneTreeTimer.SignalName.Timeout);
				animation.Add(TransferCardAnimation(node));
				EmitSignalStackChanged();
			}

			await Task.WhenAll(animation);
			EmitSignalStackChanged();
			EmitGraveyardChanged();
	}

	private async Task TransferCardAnimation(Control node)
	{
		Tween tween = CreateTween()
			.BindNode(node)
			.SetParallel();

		tween.TweenProperty(node, "global_position:x", _cardSpawnPoint.GlobalPosition.X, .5d)
			.SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(node, "global_position:y", _cardCentralPoint.GlobalPosition.Y, .5d/2d)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
		
		Tween tweenY2 = CreateTween().BindNode(node);
		tweenY2.TweenInterval(0.5d/2); 
		tweenY2.TweenProperty(node, "global_position:y", _cardSpawnPoint.GlobalPosition.Y, .5d/2)
			.SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.In);

		await ToSignal(tweenY2, Tween.SignalName.Finished);
		node.QueueFree();
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


	