/**************************************************************************
 * Generic Item class. Will be edited in the future as needed.            *
 *                                                                        *
 * Robert R. Rojas                                                        *
 *************************************************************************/

using UnityEngine;
using System.Collections;

//[System.Serializable]
public class Item : MonoBehaviour
{
	public string itemName;
	public string itemDescription;
	public string spriteName;
	public int itemID;
	public UISprite itemSprite;
	public bool hasItem;
	public bool questComplete;
}
