using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyPressIndication : MonoBehaviour 
{
	public KeyCode key;
	public string offSprite;
	public string onSprite;

	private RhythmInput rhythmInput;
	private List<KeyCode> keyCodes;

	void Start () 
	{
		rhythmInput = GameObject.FindGameObjectWithTag("GameController").GetComponent<RhythmInput>();
		keyCodes = rhythmInput.colorKeys;
	}

	void Update () 
	{
		keyCodes = rhythmInput.colorKeys;
		
		if(keyCodes.Count == 0)
		{
			gameObject.GetComponent<UISprite>().spriteName = offSprite;
		}

		foreach(KeyCode code in keyCodes)
		{
			if (key == code)
			{
				gameObject.GetComponent<UISprite>().spriteName = onSprite;
			}
			else
			{
				gameObject.GetComponent<UISprite>().spriteName = offSprite;
			}
		}
	}
}
