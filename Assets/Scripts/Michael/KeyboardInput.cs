using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour 
{
	
	Vector3 futurePosition;

	float moveSpeed = .075f;

	void Start() 
	{
		futurePosition = transform.position;
	}

	void Update() 
	{
		HandleKeyboard();
	}

	void HandleKeyboard()
	{
		if(Input.GetAxis("Horizontal") > 0)
		{
			futurePosition.x += moveSpeed;
			if(IsColliding() == false)
			{
				this.transform.position = futurePosition;
			}
		}
		if(Input.GetAxis("Horizontal") < 0)
		{
			futurePosition.x -= moveSpeed;
			if(IsColliding() == false)
			{
				this.transform.position = futurePosition;
			}
		}
		if(Input.GetAxis("Vertical") > 0)
		{
			futurePosition.y += moveSpeed;
			if(IsColliding() == false)
			{
				this.transform.position = futurePosition;
			}
		}
		if(Input.GetAxis("Vertical") < 0)
		{
			futurePosition.y -= moveSpeed;
			if(IsColliding() == false)
			{
				this.transform.position = futurePosition;
			}
		}
	}
	
	bool IsColliding()
	{
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Obstacle");

		for (int i = 0; i <= obj.Length; i++)
		{
			if(obj[i].collider.bounds.Contains(collider.bounds.center) == false)
			{
				return false;
			}
		}
		return true;
	}

}
