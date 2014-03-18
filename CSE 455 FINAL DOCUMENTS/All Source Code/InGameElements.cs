using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class InGameElements : MonoBehaviour 
{
	public Camera uiCamera;
	public string defaultSprite = "Box - Item";
	public UIPanel pausePanel;
	public UIPanel mainDialoguePanel;
	public GameObject inventoryBar;
	public UISprite[] itemSprites;

	public static InGameElements Instance
	{
		get
		{
			if (!instance)
			{
				instance = GameObject.Find("In Game UI").GetComponent<InGameElements>();
			}

			return instance;
		}
	}
	
	private int activeItemIndex = -1;
	private bool bItemSelected = false;

	private static InGameElements instance;
	private List<Item> items = new List<Item>();

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		pausePanel.alpha = 0f;
		mainDialoguePanel.alpha = 0f;
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
			KeyboardInput.KeyPress(KeyCode.Escape);
		}
	}

	public void SetItemState(bool selected, int itemIndex)
	{
		if (selected)
		{
			activeItemIndex = itemIndex;
			bItemSelected = selected;
			Debug.Log (activeItemIndex.ToString () + ", " + bItemSelected.ToString ());
		}
		else
		{
			activeItemIndex = -1;
			bItemSelected = false;
		}
	}
}
