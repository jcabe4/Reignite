using UnityEngine;
using System.Collections;

public class spinCube : MonoBehaviour 
{
	private Transform obj;
	public Vector3 newRotation = new Vector3();
	// Use this for initialization
	void Start () 
	{
		obj = GameObject.FindGameObjectWithTag ("Cube").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		newRotation.Set (obj.rotation.x + 5f, obj.rotation.y + 5f, 0f);
		obj.Rotate (newRotation);
	}
}
