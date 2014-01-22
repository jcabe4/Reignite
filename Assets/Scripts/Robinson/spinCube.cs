using UnityEngine;
using System.Collections;

public class spinCube : MonoBehaviour 
{
	private Transform obj;
	public Vector3 newRotation = new Vector3();
	// Use this for initialization
	void Start () 
	{
		obj = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		newRotation.Set (transform.rotation.x + 10f, transform.rotation.y + 10f, 0f);
		obj.Rotate (newRotation);
	}
}
