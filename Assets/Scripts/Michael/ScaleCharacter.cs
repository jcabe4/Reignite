/*****************************************************
 * Program: Reignite
 * Script: ScaleCharacter.cs
 * Author: Michael Swedo, with help from Robert Rojas
 * Description: This script handles scaling the character
 * as they move from the back of the screen to the front.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class ScaleCharacter : MonoBehaviour 
{
	public const float factor = 2f;

	public float maxY;
	public float minY;

	Vector3 currentPosition;
	Vector3 currentScale;

	float minScale = .9f;
	float maxScale = 1.1f;

	void Start () 
	{
		currentPosition = transform.position;
		currentScale = transform.localScale;
	}

	public void changeScaleFactors(float newMax, float newMin)
	{
		maxY = newMax;
		minY = newMin;
	}

	void Update() 
	{
		currentPosition = transform.position;

		float s = (maxY - currentPosition.y) / (maxY - minY);

		float scaleMod = minScale + (s / factor) * maxScale;

		currentScale.x = scaleMod;
		currentScale.y = scaleMod;

		transform.localScale = currentScale;
	}
}