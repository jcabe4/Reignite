using UnityEngine;
using System.Collections;

public class LayerHandling : MonoBehaviour 
{
	private GameObject[] objects;

	// Use this for initialization
	void Start() 
	{
	//	Debug.Log (this.renderer.sortingOrder);
	// SORTING ORDER IS HOW WE DO THIS. PLAYER SORTING ORDER IS 5. ADJUST OTHER OBJECTS AROUND THIS BASED ON POSITION.
		objects = GameObject.FindGameObjectsWithTag("Object");
	}
	
	// Update is called once per frame
	void Update() 
	{
		foreach(GameObject o in objects)
		{
			if(transform.position.y >= o.transform.position.y)
			{
				o.renderer.sortingOrder = 6;
			}
			else o.renderer.sortingOrder = 4;
		}
	}
}
