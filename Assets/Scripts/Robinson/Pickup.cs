/**************************************************************************
 * Pickup.cs                                                              *
 * Uses item.cs and PlayerInformationReignite.cs to save items picked up  *
 * and destry game objects once they are saved.                           *
 * BY:Jonathan Robinson                                                   *
 * 3.5.2014                                                               *
 * ************************************************************************/

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class Pickup : MonoBehaviour 
{
	private GameObject Player;
	private Item item;
	private PlayerInformationReignite PIR;
	private GameObject PlayerInformation;
	
	void Awake () //Sets up object references
	{
		Player = GameObject.FindGameObjectWithTag ("Player");
		item = GameObject.FindGameObjectWithTag("Item").GetComponent<Item>();
		PIR = GameObject.FindGameObjectWithTag("Item").GetComponent<PlayerInformationReignite> ();
	}

	void FixedUpdate() 
	{
		if (Input.GetMouseButtonUp(0))
		{
			int index = 0;
			print ("Item Picked!");
			PIR.items.Add(item);  //Adds items to arrayList
			PIR.SaveInventory();  //Saves items with PlayerInformationReignite.cs
			Destroy (gameObject);
		}
	}
}