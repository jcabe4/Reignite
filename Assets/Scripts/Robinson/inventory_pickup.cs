using UnityEngine;
using System.Collections;

//This is a sample script that handles the picking up of items.

public class inventory_pickup : MonoBehaviour {

	public AudioClip pickup;  //Plays a sound when the item is picked up.

	private GameObject player;  //private reference to the player
	private inventory Inventory;  //private reference to the inventory script, inventory.cs The first inventory is the name of the script, the second is the name of the reference.

	void Awake() {  //sets references
		player = GameObject.FindGameObjectWithTag(Tags.player);  //finds the player via tag
		Inventory = player.GetComponent<inventory>();  //sets inventory reference
	}

	void OnTriggerEnter(Collider other) { //
		if(other.gameObject == player)  //is the trigger being triggered by the player
		{
			AudioSource.PlayClipAtPoint(keyGrab, Transform.position);  //plays sound when key is picked up
			inventory.haskey = true;  //sets the variable haskey in inventory script to true
			Destroy(gameObject);  //destroys the key.  Becuase this script was attached to the key itself, we can just destroy the game object being the key, otherwise, destroy the key via tag
		}
	}
}//To apply a sound, in the inspector, drag the audio file to the key grab vairable, set with the audiosource line above.

	//This 
	