using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour 
{
	private Transform turn;
	public Vector3 newRotation = new Vector3();
	// Use this for initialization
	void Start () 
	{
		turn = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.RightArrow)) transform.Rotate(0, 1, 0);
		if (Input.GetKey(KeyCode.LeftArrow)) transform.Rotate(0, -1, 0);
	}
}
