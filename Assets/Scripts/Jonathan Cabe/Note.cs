using UnityEngine;
using System.Collections;

[System.Serializable]
public class Note 
{
	public enum NoteColor {None, Blue, Green, Red, Yellow,
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
