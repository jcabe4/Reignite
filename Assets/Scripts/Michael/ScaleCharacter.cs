using UnityEngine;
using System.Collections;

public class ScaleCharacter : MonoBehaviour 
{
	Vector3 startPosition;
	Vector3 startScale;
	Vector3 currentPosition;
	Vector3 currentScale;

	public Vector3 minScale = new Vector3 (.7f, .7f, 1.0f);
	public Vector3 maxScale = new Vector3 (1.2f, 1.2f, 1.0f);
	public float scaleModifier = .000001f;

	void Start () 
	{
		startPosition = transform.position;
		startScale = transform.localScale;

		currentPosition = startPosition;
		currentScale = startScale;
	}
	
	void Update() 
	{
		currentPosition = transform.position;
		
		if(currentPosition.y < startPosition.y && currentScale.x <= maxScale.x && currentScale.y <= maxScale.y)
		{
			currentScale.x += scaleModifier;
			currentScale.y += scaleModifier;
			
			transform.localScale = currentScale;
		}
		else if(currentPosition.y >= startPosition.y && currentScale.x >= maxScale.x && currentScale.y >= maxScale.y)
		{
			currentScale.x -= scaleModifier;
			currentScale.y -= scaleModifier;
			transform.localScale = currentScale;
		}
	}
}
