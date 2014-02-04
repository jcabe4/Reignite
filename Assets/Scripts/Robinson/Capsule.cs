using UnityEngine;
using System.Collections;

public class Capsule : MonoBehaviour 
{
	private Transform spin;
	public Vector3 newRotation = new Vector3();
	// Use this for initialization
	void Start () 
	{
		spin = GameObject.FindGameObjectWithTag ("Capsule").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		newRotation.Set (spin.rotation.x + 5f, spin.rotation.y + 5f, 0f);
		spin.Rotate (newRotation);
	}
}
