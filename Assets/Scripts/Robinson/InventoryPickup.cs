using UnityEngine;
using System.Collections;

//This is a sample script that handles the picking up of items.

public class InventoryPickup : MonoBehaviour 
{
	//public AudioClip pickup;  //Plays a sound when the item is picked up.

	private Inventory inventory;  //private reference to the inventory script, inventory.cs The first inventory is the name of the script, the second is the name of the reference.

	private bool interactable = false;

	void Awake() 
	{
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();  //sets inventory reference
	}

	void OnTriggerStay(Collider other) 
	{
		if(other.gameObject.tag == "Player")  //is the trigger being triggered by the player
		{
			interactable = true;
			//AudioSource.PlayClipAtPoint(keyGrab, Transform.position);  //plays sound when key is picked up
			//inventory.hasKey = true;  //sets the variable haskey in inventory script to true
			//Destroy(gameObject);  //destroys the key.  Becuase this script was attached to the key itself, we can just destroy the game object being the key, otherwise, destroy the key via tag
		}
	}

	public bool IsInteractable()
	{
		return interactable;
	}
}//To apply a sound, in the inspector, drag the audio file to the key grab vairable, set with the audiosource line above.
