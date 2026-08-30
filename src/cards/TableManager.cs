// -----------------------------------------------------------------------
// <copyright file="TableManager.cs" company="World Rift Studio">
// Copyright (c) World Rift Studio. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using BeyondTheWorlds.common.debug_console;
using Godot;
using Godot.Collections;

namespace BeyondTheWorlds.cards;

/// <summary>
///     Instancja menadżera gry obsługuje System rozgrywki, zarządza grą i kartami.
/// </summary>
[GlobalClass]
public partial class TableManager : Node
{
    /// <summary>
    ///     Instancja umożliwiająca odwołanie się do niej w każdym skrypcie
    /// </summary>
    public static TableManager? Instance { get; private set; }

    // Podłącza sygnały, inicjalizuje liczniki
    public override void _Ready()
    {
        Instance = this;
        if (_endTourButton != null)
        {
            _endTourButton.Pressed += RemoveCardsFromHand;
            _endTourButton.Pressed += AddCardsToHand;
        }

        StackChanged += OnStackChanged;
        StackChanged += UpdateStackText;
        GraveyardChanged += UpdateGraveyardText;
        EmitSignalGraveyardChanged();
        EmitSignalStackChanged();
    }

    /// <summary>
    ///     Aktaulizuję liczniki w przypadku pustego stosu dobierania.
    /// </summary>
    private void OnStackChanged()
    {
        if (_deckManager?.GetCardsCount() == 0)
        {
            if (_graveyardManager?.GetCardsCount() == 0)
            {
                DebugConsole.Log(
                    "WARNING",
                    "CardSys",
                    "There are no cards in the deck and on the graveyard!"
                );
                return;
            }

            var cardsToMove = new List<CardData>(_graveyardManager?.GetCards() ?? []);
            foreach (var card in cardsToMove)
                _deckManager.AddCards(card);

            _graveyardManager?.ClearGraveyard();
            UpdateGraveyardText();
            UpdateStackText();
        }
    }

    /// <summary>
    ///     Iteruję po dzieciach <see cref="HandManager" />, ekstarkuje czyste dane karty i przesyła na cmentarz, usuwając
    ///     węzęł na koniec
    /// </summary>
    private void RemoveCardsFromHand()
    {
        var cards = new Array<CardBase>(_handManager?.GetChildren().OfType<CardBase>() ?? []);
        foreach (var card in cards)
        {
            var cardData = card.CardInfo;
            if (cardData != null)
                _graveyardManager?.PushCard(cardData);
            card.QueueFree();
        }
    }

    /// <summary>
    ///     Instancjonuje puste obiekty kart, incjalizuje je przesyłając dane karty i dodaje je jako dzieci węzła
    ///     <see cref="HandManager" />.
    ///     Na końcu za pomocą metody <see cref="HandManager.ArangeCards" />, ustawia je na właściwych pozycjąch w ręce.
    /// </summary>
    private void AddCardsToHand()
    {
        while (
            _handManager?.GetCardsCount() < _defaultCardsNumber && _deckManager?.GetCardsCount() > 0
        )
        {
            var cardData = _deckManager.GetCard();
            var cardNode = _cardBaseTscn?.Instantiate<CardBase>();
            if (cardData != null)
                cardNode?.Initialize(cardData);
            _handManager.AddChild(cardNode);
            DebugConsole.Log("INFO", "CardSys", "Succesfully added card to Player hand.");
            //TODO: Dodać animacje dodawnia karty
        }

        _handManager?.ArangeCards();
        EmitStackChanged();
    }

    /// <summary>
    ///     Aktualizuje licznik kart na cmentarzu
    /// </summary>
    private void UpdateGraveyardText()
    {
        if (_graveyardManager != null && _graveyardCards != null)
            _graveyardCards.Text = $"{_graveyardManager.GetCardsCount()}";
    }

    /// <summary>
    ///     Aktualizuje licznik kart w talii
    /// </summary>
    private void UpdateStackText()
    {
        if (_deckManager != null && _deckCards != null)
            _deckCards.Text = $"{_deckManager.GetCardsCount()}";
    }

    #region Events

    [Signal]
    public delegate void NextTurnStartedEventHandler();

    [Signal]
    public delegate void NewBattleStartedEventHandler();

    [Signal]
    public delegate void StackChangedEventHandler();

    [Signal]
    public delegate void GraveyardChangedEventHandler();

    public static void EmitNextTurnStarted()
    {
        Instance?.EmitSignalNextTurnStarted();
    }

    public static void EmitNewBattleStarted()
    {
        Instance?.EmitSignalNewBattleStarted();
    }

    public static void EmitStackChanged()
    {
        Instance?.EmitSignalStackChanged();
    }

    public static void EmitGraveyardChanged()
    {
        Instance?.EmitSignalGraveyardChanged();
    }

    #endregion


    #region Exports

    [ExportCategory("Table Manager")]
    [ExportGroup("Components")]
    [Export]
    private DeckManager? _deckManager;

    [Export]
    private HandManager? _handManager;

    [Export]
    private GraveyardManager? _graveyardManager;

    [ExportGroup("Properties")]
    [Export(PropertyHint.Range, "1, 10, 1, prefer_slider")]
    private short _defaultCardsNumber = 5;

    [ExportGroup("User Interface")]
    [Export]
    private Button? _endTourButton;

    [Export]
    private Label? _graveyardCards;

    [Export]
    private Label? _deckCards;

    [ExportGroup("")]
    [Export]
    private PackedScene? _cardBaseTscn;

    #endregion
}
