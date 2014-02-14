using UnityEngine;
using System.Collections;

public class LayerHandling : MonoBehaviour 
{
	private GameObject[] objects;

	void Start() 
	{
		objects = GameObject.FindGameObjectsWithTag("Static Object");
	}

	void Update() 
	{
		foreach(GameObject o in objects)
		{
			if(transform.position.y >= o.transform.position.y)
			{
				o.renderer.sortingLayerName = "Foreground";
			}
			else o.renderer.sortingLayerName = "Background";
		}
	}
}
