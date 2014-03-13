/*****************************************************
 * Program: Reignite
 * Script: LayerHandling.cs
 * Author: Michael Swedo
 * Description: This script handles layers of the player
 * and all static objects in the scene, and dynamically
 * changes object layers to appear in front of or behind
 * the player as they move around the scene.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class LayerHandling : MonoBehaviour 
{
	private float transformOffset = 2.5f;

	private GameObject[] staticObjects;
	private GameObject[] interactiveObjects;

	void Start() 
	{
		staticObjects = GameObject.FindGameObjectsWithTag("Static Object");
		interactiveObjects = GameObject.FindGameObjectsWithTag("Interactive Object");
	}

	void Update() 
	{
		foreach(GameObject so in staticObjects)
		{
			if(so)
			{
				if(transform.position.y >= so.transform.position.y)
				{
					foreach(GameObject io in interactiveObjects)
					{
						if(io)
						{
							//if(so.collider2D.OverlapPoint (io.GetComponent<BoxCollider2D>().center))
							if(transform.position.y >= io.transform.position.y)
							{
								io.renderer.sortingLayerName = "Interactive Foreground";
							}
						}
					}
					so.renderer.sortingLayerName = "Foreground";

				}

				else
				{
					foreach(GameObject io in interactiveObjects)
					{
						if(io)
						{
							if(transform.position.y + transformOffset <= io.transform.position.y)
							{
								io.renderer.sortingLayerName = "Interactive Background";
							}
							else
							{
								io.renderer.sortingLayerName = "Above Static Foreground";
							}
						}
					}
					so.renderer.sortingLayerName = "Background";
				}
			}
		}
	}
}
