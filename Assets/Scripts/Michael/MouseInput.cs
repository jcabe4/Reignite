using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour 
{
	public Vector3 futurePosition;
	public float moveSpeed = .075f;

	RaycastHit hit;
	Ray towards;

	void Start() 
	{
		futurePosition = transform.position;
	}
	
	void Update() 
	{
		HandleMouse();
	}

	void HandleMouse()
	{
		if(Input.GetMouseButton(0))
		{
			towards = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
			futurePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			futurePosition.z = transform.position.z;
		}
		
		if(Physics.Raycast(towards, out hit, 20))
		{
			if(hit.collider.gameObject.tag == "Object")
			{
				if(hit.collider.bounds.Contains(transform.collider.bounds.center))
				{
					if(Input.GetMouseButtonDown(0))
					{
						hit.collider.audio.Play();
						Debug.Log ("you interacted with me!");
					}
				}
				else
				{
					transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
				}
			}

			else if(hit.collider.gameObject.tag != "Obstacle")
			{
				transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
			}
		}
	}
}
