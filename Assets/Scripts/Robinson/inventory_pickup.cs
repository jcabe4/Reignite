/************************************************************
 * Script: Inventory_pickup.cs
 * Author: Jonathan Robinson
 * Description: Inventory pickup mechanism.
 ************************************************************/

using UnityEngine;
using System.Collections;

public class Inventory_pickup : MonoBehaviour 
{
	private GameObject Player;  
	private Inventory inventory;  

	void Awake() 
	{
		Player = GameObject.FindGameObjectWithTag("Player");  
		inventory = Player.GetComponent<Inventory>(); 
	}

	void Update()
	{
		if (inventory.hasKey == true) 
		{
			Destroy (gameObject);
		}
		if (Input.GetMouseButton (0))
		{
			print ("Key picked up!");
			inventory.hasKey = true;
			Destroy (gameObject);
			PlayerInformationReignite.Instance.SaveData();
		}
	}
}