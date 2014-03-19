using UnityEngine;
using System.Collections;

public class UseItem : MonoBehaviour 
{
	// check click event
	// which item is highlighted
	// what item by name is highlighted
	// IGE.SetItemState

	public Item requiredItem;
	public bool destroyItemOnUse;

	private ScreenTransition transition;
	private DialogInteraction interaction;

	void Start () 
	{
		transition = gameObject.GetComponent<ScreenTransition>();
		interaction = gameObject.GetComponent<DialogInteraction>();
	}
	
	void Update () 
	{
	
	}

	public void ItemUsable()
	{
		if(PlayerInformation.Instance.items[InGameElements.Instance.activeItemIndex].itemName == requiredItem.itemName)
		{
			if(transition)
			{
				transition.MoveToRoom();
			}

			if(interaction)
			{
				interaction.HandleDialog();
			}

			if(destroyItemOnUse)
			{
				PlayerInformation.Instance.RemoveItem(requiredItem);
			}
		}
	}
}
