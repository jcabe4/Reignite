/*****************************************************
 * Program: Reignite
 * Script: RhythmMenuButton.cs
 * Author: Deanna Sulli
 * Description: This script will handle buttons in the
 * rhythm section of the game.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class RhythmMenuButton : MonoBehaviour 
{
	public enum ButtonType {Resume, Save, Load, Options, Exit}
	public ButtonType type;
	
	public UIPanel menuPanel;
	public UIPanel optionsPanel; // for the options we will reveal to the player in-game
	public UIPanel savePanel;
	public UIPanel loadPanel;
	public UIPanel exitPanel;
	
	void OnClick()
	{
		switch (type)
		{
		case ButtonType.Resume:
		{
			menuPanel.alpha = 0;
			RhythmInput.KeyPress(RhythmInput.Instance.options);
			break;
		}
		case ButtonType.Save:
		{
			// open up the save panel
			break;
		}
		case ButtonType.Load:
		{
			// open up the load panel
			break;
		}
		case ButtonType.Options:
		{
			// open up the options panel
			break;
		}
		case ButtonType.Exit:
		{
			if(exitPanel)
			{
				//open the exit panel
			}
			else
			{
				Application.LoadLevel("Title Screen");
			}
			break;
		}
		default:
		{
			break;
		}
		}
	}
}
