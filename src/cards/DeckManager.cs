using BeyondTheWorlds.common.debug_console;
using Godot;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class DeckManager : Node
{
    [Export]
    private CardData? TestCard { get; set; }

    public Stack<CardData> PlayerCards { get; private set; } = [];

    public override void _Ready()
    {
        // TODO: Usunąć testowe napełnianie po wdrożeniu LoadPlayerDeck
        for (var i = 0; i < 9; i++)
            if (TestCard == null)
                return;
        if (TestCard == null)
            return;
        PlayerCards.Push(TestCard);
    }

    /// <summary>
    ///     Pobiera i usuwa kartę z góry stosu.
    /// </summary>
    /// <returns>Dane karty lub <c>null</c>, jeśli stos jest pusty.</returns>
    public CardData? GetCard()
    {
        if (PlayerCards.Count == 0)
        {
            DebugConsole.Log("WARNING", "CardSys", "Tried to get card from empty stack");
            return null;
        }

        return PlayerCards.Pop();
    }

    /// <summary>
    ///     Nadpisuje obecną talię gracza nowym stosem kart.
    /// </summary>
    /// <param name="receivedCards">Nowy stos kart do załadowania.</param>
    public void AddCards(Stack<CardData> receivedCards)
    {
        DebugConsole.Log("INFO", "CardSys", "Received new card set");
        PlayerCards = receivedCards;
    }

    /// <summary>
    ///     Dodaje pojedynczą kartę na szczyt stosu.
    /// </summary>
    public void AddCards(CardData receivedCard)
    {
        PlayerCards.Push(receivedCard);
    }

    /// <summary>
    ///     Zwraca aktualną liczbę kart na stosie.
    /// </summary>
    public int GetCardsCount()
    {
        return PlayerCards.Count;
    }

    private void LoadPlayerDeck()
    {
        // TODO: Dodać ładowanie decku z zapisu/pliku
        throw new NotImplementedException();
    }
}
