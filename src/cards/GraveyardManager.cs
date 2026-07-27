using Godot;
using System;
using System.Collections.Generic;
using BeyondTheWorlds.cards;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class GraveyardManager : Node
{
	public Stack<CardData> Graveyard { get; set; } = [];

	public void PushCard(CardData card)
	{
		Graveyard.Push(card);
		TableManager.EmitGraveyardChanged();
	}

	public Stack<CardData> GetCards()
	{
		return Graveyard;
	}

	public int GetCardsCount()
	{
		return Graveyard.Count;
	}

	public void ClearGraveyard()
	{
		Graveyard.Clear();
		Graveyard = [];
	}
}
