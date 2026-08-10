using Godot;

namespace BeyondTheWorlds.cards;

/// <summary>
/// Przechowuje dane i parametry karty budowlanej.
/// </summary>
/// <param name="cardTexture">Dwuwymiarowa grafika karty (nie model 3D).</param>
/// <param name="cardName">Wyświetlana nazwa karty.</param>
/// <param name="cardType">Kategoria/typ karty.</param>
/// <param name="cardClass">Klasa lub przynależność karty.</param>
/// <param name="cardDescription">Tekst opisowy lub efekty karty.</param>
/// <param name="cardCost">Koszt punktowy/zasobów wymagany do zagrania karty.</param>
[GlobalClass]
public partial class CardData(
    Texture2D cardTexture = null,
    string cardName = null,
    string cardType = null,
    string cardClass = null,
    string cardDescription = null,
    int cardCost = 0) : Resource
{
    /// <summary>Dwuwymiarowa grafika karty (nie model 3D)</summary>
    [Export]
    public Texture2D CardTexture { get; set; } = cardTexture;

    [Export]
    public string CardName { get; set; } = cardName;

    [Export]
    public string CardType { get; set; } = cardType;

    [Export]
    public string CardClass { get; set; } = cardClass;

    [Export(PropertyHint.MultilineText)] 
    public string CardDescription { get; set; } = cardDescription;

    [Export(PropertyHint.Range, "0, 5, 1, prefer_slider")] 
    public int CardCost { get; set; } = cardCost; 
}