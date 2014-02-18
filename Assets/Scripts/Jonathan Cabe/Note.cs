/*****************************************************
 * Program: Reignite
 * Script: Note.cs
 * Author: Jonathan Cabe
 * Description: This is a class representation of what
 * a note is.  This is used in both the score creation
 * tool and the rhythm game itself.
 * ***************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Note 
{
	public enum NoteColor {Pause, Blue, Green, Red, Yellow,
							BlueGreen, BlueRed, BlueYellow,
							GreenRed, GreenYellow, RedYellow};

	public NoteColor color;
	public Vector2 endTime;
	public Vector2 startTime;

	public float endTotal = 0f;
	public float startTotal = 0f;
	public float noteDuration = 0f;
	public int pitchPosition = 0;
}
