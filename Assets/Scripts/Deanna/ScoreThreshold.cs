using UnityEngine;
using System.Collections;

public class ScoreThreshold : MonoBehaviour 
{
	public int threshold;
	public string spriteName;

	private int score;
	private RhythmGame rhythmGame;
	void Start () 
	{
		rhythmGame = GameObject.FindGameObjectWithTag("GameController").GetComponent<RhythmGame>();
		score = rhythmGame.score;
	}

	void Update ()
	{
		score = rhythmGame.score;

		if (score >= threshold)
		{
			gameObject.GetComponent<UISprite>().spriteName = spriteName;
		}
	}
}
