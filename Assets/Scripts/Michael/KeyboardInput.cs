using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour 
{
	void Start() 
	{

	}

	void Update() 
	{
		HandleKeyboard();
	}

	void HandleKeyboard()
	{
		if(Input.GetButtonDown("Inventory"))
		{
			Debug.Log("Make me open the inventory!");
		}

		if(Input.GetButtonDown("Menu"))
		{
			Debug.Log ("Make me open the menu!");
		}
	}
}
