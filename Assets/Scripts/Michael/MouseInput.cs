using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour 
{
	public Vector3 futurePosition;
	public float moveSpeed = .075f;

	Ray towards;

	private LayerMask ignoreMask = ~(1 << 2);

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

		RaycastHit[] hits = Physics.RaycastAll(towards, Mathf.Infinity, ignoreMask);
		if(hits.Length > 0)
		{
			for(int count = 0; count < hits.Length; count++)
			{
				// Object has HIGHEST precedence. Interacting with objects comes before colliding with them.
				if(hits[count].collider.gameObject.tag == "Interactable")
				{
					// interact with object
				}

				// Since we are raycasting through everything, if we find an obstacle anywhere, we
				// don't want to move through it.
				if(hits[count].collider.gameObject.tag == "Obstacle")
				{
					futurePosition = transform.position;
				}

				// But if we don't find one, we should be able to move towards where we wanted to go.
				else if(hits[count].collider.gameObject.tag == "Environment")
				{
					transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
				}
			}
		}
	}
}
