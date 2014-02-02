using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private PolygonCollider2D backgroundCollider;
	public Vector3 futurePosition;
	public float moveSpeed = .075f;

	void Start() 
	{
		backgroundCollider = GameObject.FindGameObjectWithTag("Environment").GetComponent<PolygonCollider2D>();
		futurePosition = transform.position;
	}
	
	void Update() 
	{
		if(Input.GetMouseButton(0))
		{
			futurePosition = Input.mousePosition;
			futurePosition = Camera.main.ScreenToWorldPoint(futurePosition);
			futurePosition.z = transform.position.z;
		}

		transform.position = Vector3.MoveTowards(transform.position, futurePosition, moveSpeed);
	}
}
