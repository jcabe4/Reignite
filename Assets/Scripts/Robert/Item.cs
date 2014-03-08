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
	public int itemID;
	public bool hasItem;
	public bool questComplete;
	public string itemName;
	public string spriteName;
	public string itemDescription;
}
