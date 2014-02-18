using UnityEngine;
using System.Collections;

public class GUIAnimation : MonoBehaviour 
{
	public int numberOfFrames;
	public string prefix;

	private UISprite currentSprite;

	private int index = 1;
	private int frameCount = 0;

	// Use this for initialization
	void Start () 
	{
		currentSprite = gameObject.GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		frameCount++;
		
		if(frameCount >= 8)
		{
			if(index < numberOfFrames)
			{
				index++;
				currentSprite.spriteName = prefix + index.ToString();
			}
			else
			{
				index = 1;
				currentSprite.spriteName = prefix + index.ToString();
			}

			frameCount = 0;
		}
	}
}