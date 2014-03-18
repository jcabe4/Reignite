/**************************************************************************
 * Player Data Script based on Jon Robinson's Item Pickup script.         *
 * Will be edited in the future to correspond with Item.cs                *
 *                                                                        *
 * Robert R. Rojas                                                        *
 *************************************************************************/

using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

public class ItemPickup : MonoBehaviour 
{
	/*
	private Item item;
	private PlayerInformation PI;
	private GameObject PlayerInformation; //?

	public void Awake()
	{
		item = GameObject.FindGameObjectWithTag("Player").GetComponent<Item>();
		PI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation> ();
	}

	/*public void Pickup()
	{
		if (Input.GetMouseButtonUp(0))
		{
			int index = PI.items.Count;
			int qindex = PI.quests.Count;
			print("Item picked up.");
			PI.items.Add(item);
			PI.items[index].hasItem = true;
			PI.quests[qindex].questActive = true;
			//Display item in inventory
			Destroy(gameObject);
			PI.SavePI();
		}
		else
		{
			print("Item pickup failed.");
		}
	}

	public void Use()
	{
		//Item usage & removal from inventory.
		for (int qindex = 0; qindex < PI.quests.Count; qindex++)
		{
			for (int index = 0; index < PI.items.Count; index++)
			{
				if (PI.quests[qindex].itemName == PI.items[index].itemName && PI.items[index].hasItem == true && PI.quests[qindex].questComplete == false)
	        	{
			 		//set index
					
	         	  	PI.items[index].questComplete = true;
			 	  	PI.items[index].hasItem = false;
					//Continuation Dialogue
					//Load Rythm Section
	         		PI.SavePI();
				}
				else
				{
	      	  	//Prompting dialogue.
				}
			}
		}
	}*/
}
