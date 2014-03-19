/*****************************************************
 * Program: Reignite
 * Script: MouseInput.cs
 * Author: Michael Swedo
 * Description: This script handles mouse interaction,
 * including movement and object interaction via clicking.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour 
{
	public Vector3 futurePosition;
	public float moveSpeed = .075f;
	public Sprite walkLeftFrame;
	public Sprite walkRightFrame; //expand these to arrays with animation
	public Sprite idleFrame;

	// The ignore mask makes it so our raycast ignores the Player sprite.
	private LayerMask ignoreMask = ~(1 << 2);
	private SpriteRenderer spriteRenderer;
	
	private Ray towards;

	void Start() 
	{
		futurePosition = transform.position;
		spriteRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
	}
	
	void Update() 
	{
		HandleMouse();
		if(Input.GetMouseButtonDown(0))
		{
			HandleClickingObjects();
		}
	}

	void HandleClickingObjects() 
	{
		RaycastHit2D hit; 
		towards = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

		hit = Physics2D.GetRayIntersection(towards, Mathf.Infinity, ignoreMask);

		/*
		 * Check to see if the collider we hit contains the center point
		 * of our collider.
		 * IE, if that collider has an OverlapPoint of our center point.
	 	*/

		if(hit)
		{
			if(hit.collider.gameObject.GetComponent<DialogInteraction>())
			{
				hit.collider.gameObject.GetComponent<DialogInteraction>().BeginInteraction();
			}
		}
	}

	void HandleMouse()
	{
		Ray uiRay = new Ray (InGameElements.Instance.uiCamera.ScreenToWorldPoint (Input.mousePosition), Vector3.forward);
		uiRay.origin -= Vector3.forward;

		if(Input.GetMouseButton(0) && !(Physics.Raycast(uiRay)))
		{
			towards = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
			futurePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			futurePosition.z = transform.position.z;
		}

		RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(towards, Mathf.Infinity, ignoreMask);

		if(hits.Length > 0)
		{
			for(int count = 0; count < hits.Length; count++)
			{
				// Since we are raycasting through everything, if we find an obstacle anywhere, we
				// don't want to move through it.
				
				if(InGameElements.Instance.activeItemIndex != -1)
				{
					if(hits[count].collider.gameObject.GetComponent<UseItem>())
					{
						hits[count].collider.gameObject.GetComponent<UseItem>().ItemUsable();
					}

					else if(hits[count].collider.gameObject.tag == "Environment")
					{
						if(transform.position.x > futurePosition.x)
						{
							spriteRenderer.sprite = walkLeftFrame;
						}
						else if(transform.position.x < futurePosition.x)
						{
							spriteRenderer.sprite = walkRightFrame;
						}
						
						transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
					}
				}

				else if(hits[count].collider.gameObject.tag == "Environment")
				{
					if(transform.position.x > futurePosition.x)
					{
						spriteRenderer.sprite = walkLeftFrame;
					}
					else if(transform.position.x < futurePosition.x)
					{
						spriteRenderer.sprite = walkRightFrame;
					}
					
					transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
				}
			}
		}
	}
}
