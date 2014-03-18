/*****************************************************
 * Program: Reignite
 * Script: Quote.cs
 * Author: Jonathan Cabe
 * Description: This is a class representation of what
 * a quote is.  This is used in both the dialogue
 * creation tool and the dialogue in the game itself.
 * ***************************************************/
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Quote
{
	public int quoteIndex;
	public string speaker;
	public string statement;
	public bool bDisplayed = false;
}
