using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour 
{
	
	Vector3 futurePosition;
	Vector3 currentPosition;

	float moveSpeed = .075f;

	void Start() 
	{
		currentPosition = transform.position;
		futurePosition = transform.position;
	}

	void Update() 
	{
		HandleKeyboard();
	}

	void HandleKeyboard()
	{
		if(Input.GetAxis("Horizontal") > 1)
		{

		}
	}
}
