using Godot;

namespace BeyondTheWorlds.cards.cards_bases;

[GlobalClass]
public partial class CardData(ImageTexture cardTexture, string cardName, string cardType, string cardClass,
    string cardDescription, int cardCost) : Resource
{
    
    [Export]
    public ImageTexture CardTexture { get; set; } = cardTexture;

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

    public CardData() : this(null, null, null, null, null, 0) {}

}