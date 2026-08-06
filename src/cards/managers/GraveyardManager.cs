using Godot;
using System;
using System.Collections.Generic;
using BeyondTheWorlds.cards;

namespace BeyondTheWorlds.cards;

<<<<<<< Updated upstream
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
=======
[GlobalClass]
public partial class GraveyardManager : Node
{
	public Stack<CardData> Graveyard { get; set; } = [];

>>>>>>> Stashed changes
	public void PushCard(CardData card)
	{
		Graveyard.Push(card);
		managers.TableManager.EmitGraveyardChanged();
	}

<<<<<<< Updated upstream
	/// <summary>
	/// Zwraca stos lub <c>null</c> jeśli stos jest pusty.
	/// </summary>
=======
>>>>>>> Stashed changes
	public Stack<CardData> GetCards()
	{
		return Graveyard;
	}

<<<<<<< Updated upstream
	/// <summary>
	/// Zwraca liczbę kart na stosie
	/// </summary>
=======
>>>>>>> Stashed changes
	public int GetCardsCount()
	{
		return Graveyard.Count;
	}

<<<<<<< Updated upstream
	/// <summary>
	/// Oczyszcza stos z kart 
	/// </summary>
=======
>>>>>>> Stashed changes
	public void ClearGraveyard()
	{
		Graveyard.Clear();
		Graveyard = [];
	}
}
