using UnityEngine;
using System.Collections;

public class ScaleCharacter : MonoBehaviour 
{
	Vector3 startPosition;
	Vector3 startScale;
	Vector3 currentPosition;
	Vector3 currentScale;

	int frameCount;

	public Vector3 minScale = new Vector3 (.9f, .9f, 1.0f);
	public Vector3 maxScale = new Vector3 (1.1f, 1.1f, 1.0f);
	public float scaleModifier = .05f;

	void Start () 
	{
		startPosition = transform.position;
		startScale = transform.localScale;

		currentPosition = startPosition;
		currentScale = startScale;

		frameCount = 0;
	}
	
	void Update() 
	{
		currentPosition = transform.position;

		if(frameCount >= 2)
		{
			if(currentPosition.y < startPosition.y && currentScale.x <= maxScale.x && currentScale.y <= maxScale.y)
			{
				currentScale.x += scaleModifier;
				currentScale.y += scaleModifier;
				
				transform.localScale = currentScale;
			}
			else if(currentPosition.y > startPosition.y && currentScale.x >= minScale.x && currentScale.y >= minScale.y)
			{
				currentScale.x -= scaleModifier;
				currentScale.y -= scaleModifier;
				transform.localScale = currentScale;
			}
			frameCount = 0;
		}

		frameCount++;
	}
}