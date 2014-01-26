using UnityEngine;
using System.Collections;

public class cube2 : MonoBehaviour 
{
	private Transform obj;
	public Vector3 newRotation = new Vector3();
	// Use this for initialization
	void Start () 
	{
		obj = GameObject.FindGameObjectWithTag ("cube2").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		newRotation.Set (obj.rotation.x + 5f, obj.rotation.y + 5f, 0f);
		obj.Rotate (newRotation);
	}
}
