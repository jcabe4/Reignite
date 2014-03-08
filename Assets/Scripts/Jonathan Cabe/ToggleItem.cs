using UnityEngine;
using System.Collections;

public class ToggleItem : MonoBehaviour 
{
	public delegate void OnSelect(int selectedIndex);
	public static OnSelect Select;

	public int itemIndex = 0;
	public string normalSprite = "Box - Item";
	public string hoverSprite = "Box - Dialogue";
	public string selectedSprite = "Box - Inventory";

	private bool bIsSelected = false;

	private UISprite sprite;

	void Start()
	{
		sprite = gameObject.GetComponent<UISprite> ();
	}

	void OnEnable()
	{
		Select += SelectItem;
	}

	void OnDisable()
	{
		Select -= SelectItem;
	}

	void OnClick()
	{
		Select (itemIndex);
	}

	void OnHover(bool bIsOver)
	{
		if (bIsOver)
		{
			if (!bIsSelected)
			{
				sprite.spriteName = hoverSprite;
			}
		}
		else
		{
			if (!bIsSelected)
			{
				sprite.spriteName = normalSprite;
			}
		}
	}

	void SelectItem(int selectedIndex)
	{
		if (itemIndex == selectedIndex)
		{
			if (!bIsSelected)
			{
				bIsSelected = true;
				sprite.spriteName = selectedSprite;
			}
			else
			{
				bIsSelected = false;
				sprite.spriteName = hoverSprite;
			}

			InGameElements.Instance.SetItemState(bIsSelected, itemIndex);
		}
		else
		{
			bIsSelected = false;
			sprite.spriteName = normalSprite;
		}
	}
}
