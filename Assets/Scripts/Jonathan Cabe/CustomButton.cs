/*****************************************************
 * Program: Reignite
 * Script: CustomButton.cs
 * Author: Jonathan Cabe
 * Description: This is the script that will handle
 * click events on the buttons located on the options
 * menu. 
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class CustomButton : MonoBehaviour 
{	
	public enum buttonType {Start, Continue, Options, Exit};

	public bool bSwapSprite = false;

	public buttonType type;
	public UIPanel togglePanel;
	public UIPanel optionsPanel;
	public UISprite buttonSprite;

	public string hoverSprite = "Button - Icon (Hover)";
	public string pushedSprite = "Button - Icon (Active)";
	public string normalSprite = "Button - Icon";
	public string activeSprite = "Button - Icon (Active)";
	
	private bool bIsSelected;

	void Awake()
	{
		if (!buttonSprite)
		{
			buttonSprite = gameObject.GetComponent<UISprite>();
		}
	}

	void Start()
	{
		if (togglePanel)
		{
			togglePanel.alpha = 1f;
		}
	}

	void Update()
	{
		if (togglePanel && Input.GetKeyUp(KeyCode.Return))
		{
			togglePanel.alpha = 0f;
		}
	}

	void OnPress (bool isPressed)
	{
		if (isPressed)
		{
			if (bSwapSprite)
			{
				buttonSprite.spriteName = pushedSprite;
			}
		}
		else
		{
		
			if (bSwapSprite)
			{
				buttonSprite.spriteName = normalSprite;
			}

			if (type == buttonType.Start)
			{
				togglePanel.alpha = 0f;
				RhythmGame.Instance.BeginGame();
			}
			else if (type == buttonType.Continue)
			{
				RhythmInput.KeyPress(RhythmInput.Instance.options);
			}
			else if (type == buttonType.Exit)
			{
				Application.LoadLevel("Title Screen");
			}
		}
	}

	void OnClick ()
	{
		if (type == buttonType.Start)
		{
			RhythmGame.Instance.BeginGame();
		}
	}

	void OnHover (bool isOver)
	{
		if (bSwapSprite)
		{
			if (isOver)
			{
				buttonSprite.spriteName = hoverSprite;
			}
			else
			{
				buttonSprite.spriteName = normalSprite;
			}
		}
	}
}
