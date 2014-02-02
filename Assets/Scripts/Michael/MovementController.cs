using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private PolygonCollider2D backgroundCollider;
	public Vector3 futurePosition;
	public float moveSpeed = .075f;
	
	
	RaycastHit hit;
	Ray towards;

	void Start() 
	{
		backgroundCollider = GameObject.FindGameObjectWithTag("Environment").GetComponent<PolygonCollider2D>();
		futurePosition = transform.position;
	}
	
	void Update() 
	{

		/* if(Input.GetMouseButton(0))
		{
			futurePosition = Input.mousePosition;
			futurePosition = Camera.main.ScreenToWorldPoint(futurePosition);
			futurePosition.z = transform.position.z;
		}

		transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
		*/

		if(Input.GetMouseButton(0))
		{
			towards = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
			futurePosition = Input.mousePosition;
			futurePosition = Camera.main.ScreenToWorldPoint(futurePosition);
			futurePosition.z = transform.position.z;
		}

		if(Physics.Raycast(towards, out hit, 20))
		{
			
			Debug.Log (towards);
			if(hit.collider.gameObject.tag != "Obstacle" || !hit.collider.bounds.Contains(collider.bounds.center))
			{
				transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
			}
		}
	}
}
