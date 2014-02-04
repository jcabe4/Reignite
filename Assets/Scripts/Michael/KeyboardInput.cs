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
		if(Input.GetButton ("Inventory"))
		{
			// open Inventory menu;
		}
		else if(Input.GetButton ("Menu"))
		{
			// open main menu;
		}
	}
}
