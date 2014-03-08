using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class InGameElements : MonoBehaviour 
{
	public string defaultSprite = "Box - Item";
	public UIPanel pausePanel;
	public GameObject inventoryBar;
	public UISprite[] itemSprites;

	public static InGameElements Instance
	{
		get
		{
			return instance;
		}
	}
	
	private int activeItemIndex = -1;
	private bool bItemSelected = false;

	private static InGameElements instance;
	private List<Item> items = new List<Item>();

	void Start()
	{
		instance = this;
		Debug.Log (Application.persistentDataPath);
		Debug.Log (System.DateTime.Now.Date);
	}

	void Update()
	{
		UpdateInventory ();
	}
	
	private void UpdateInventory()
	{
		if (inventoryBar.activeSelf)
		{
			if (PlayerInformation.Instance)
			{
				items = PlayerInformation.Instance.items;
			}
			
			for (int i = 0; i < itemSprites.Length; i++)
			{
				if (i < items.Count)
				{
					itemSprites[i].spriteName = items[i].spriteName;
					itemSprites[i].alpha = 1f;
				}
				else
				{
					itemSprites[i].spriteName = defaultSprite;
					itemSprites[i].alpha = 0f;
				}
			}
		}
	}

	public void ToggleInventory()
	{
		if (inventoryBar)
		{
			inventoryBar.SetActive(!inventoryBar.activeSelf);
		}
	}

	public void TogglePause()
	{
		if (pausePanel)
		{
			if (pausePanel.alpha == 1f)
			{
				pausePanel.alpha = 0f;
			}
			else
			{
				pausePanel.alpha = 1f;
			}
		}
	}

	public void SetItemState(bool selected, int itemIndex)
	{
		if (selected)
		{
			activeItemIndex = itemIndex;
			bItemSelected = selected;
		}
		else
		{
			activeItemIndex = -1;
			bItemSelected = false;
		}

		Debug.Log (activeItemIndex.ToString () + ", " + bItemSelected.ToString ());
	}
}
