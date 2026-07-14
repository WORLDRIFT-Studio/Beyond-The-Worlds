using Godot;
using System;
using System.Collections.Generic;
using BeyondTheWorlds.common.debug_console;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class DeckManager : Node
{
	[Export] private CardData TestCard { get; set; }
	
	public Stack<CardData> PlayerCards { get; } = [];

	public override void _Ready()
	{
		PlayerCards.Push(TestCard);
		PlayerCards.Push(TestCard);
	}

	private void LoadPlayerDeck()
	{
		//TODO: dodac ladowanie decku
		throw new NotImplementedException();
	}

	public CardData GetCard()
	{
		if (PlayerCards.Count == 0)
		{
			DebugConsole.Log("WARNING", "CardSys", "Tried to get card, from empty stack");
			return null;
		}
		
		CardData cardData = PlayerCards.Pop();
		TableManager.EmitStackChanged((short) PlayerCards.Count);
		return cardData;
	}
}
