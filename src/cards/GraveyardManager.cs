using Godot;
using System;
using System.Collections.Generic;
using BeyondTheWorlds.cards;

namespace BeyondTheWorlds.cards;

/// <summary>
/// Cmentzarz kart. Trafiają tu wykorzystane (nie seksulanie) karty.
/// </summary>
[GlobalClass]
public partial class GraveyardManager : Node
{
	private Stack<CardData> Graveyard { get; set; } = [];

	/// <summary>
	/// Dodaje pojedyńczą kartę na szczyt stosu
	/// </summary>
	/// <param name="card"></param>
	public void PushCard(CardData card)
	{
		Graveyard.Push(card);
		TableManager.EmitGraveyardChanged();
	}

	/// <summary>
	/// Zwraca stos lub <c>null</c> jeśli stos jest pusty.
	/// </summary>
	public Stack<CardData> GetCards()
	{
		return Graveyard;
	}

	/// <summary>
	/// Zwraca liczbę kart na stosie
	/// </summary>
	public int GetCardsCount()
	{
		return Graveyard.Count;
	}

	/// <summary>
	/// Oczyszcza stos z kart 
	/// </summary>
	public void ClearGraveyard()
	{
		Graveyard.Clear();
		Graveyard = [];
	}
}
