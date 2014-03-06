/**************************************************************************
 * Item.cs                                                                *
 * Item class for PlayerInformationReignite.cs and Pickup.cs              *
 * BY:Jonathan Robinson and Robert Rojas                                  *
 * 3.5.2014                                                               *
 * ************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item : MonoBehaviour
{
	public string itemName;
	public bool contains;
	public string itemDescription;
	public int itemID;
}

