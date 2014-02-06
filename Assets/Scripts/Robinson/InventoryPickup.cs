using UnityEngine;
using System.Collections;

//This is a sample script that handles the picking up of items.

public class InventoryPickup : MonoBehaviour 
{
	//public AudioClip pickup;  //Plays a sound when the item is picked up.

	private Collider playerCollider;
	private Inventory inventory;  //private reference to the inventory script, inventory.cs The first inventory is the name of the script, the second is the name of the reference.
	public bool interactable = false;

	void Start() 
	{
		playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();  //sets inventory reference
	}

	void Update()
	{
		if(collider.bounds.Contains(playerCollider.bounds.center))
		{
			interactable = true;
			//Debug.Log ("I'm interactable now!");
		}
		else interactable = false;
	}

	public bool IsInteractable()
	{
		return interactable;
	}
}//To apply a sound, in the inspector, drag the audio file to the key grab vairable, set with the audiosource line above.
