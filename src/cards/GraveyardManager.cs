using Godot;
using System;
using System.Collections.Generic;
using BeyondTheWorlds.cards;

namespace BeyondTheWorlds.cards;

[GlobalClass]
public partial class GraveyardManager : Node
{
	private Stack<CardData> Graveyard { get; } = [];

	public override void _Ready()
	{
		
	}

	public void PushCardToGraveyard(CardData card)
	{
		Graveyard.Push(card);
	}
}
