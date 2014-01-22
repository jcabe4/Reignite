using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	private Transform obj;
	public Vector3 newRotation = new Vector3();
	// Use this for initialization
	void Start () {
		obj = GameObject.FindGameObjectWithTag ("Cube").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		newRotation.Set (obj.rotation.x + 10f, obj.rotation.y + 10f, 0f);
		obj.Rotate (newRotation);
	}
}
