/*****************************************************
 * Program: Reignite
 * Script: ExplorationMenuButton.cs
 * Author: Deanna Sulli
 * Description: This script will handle buttons in the
 * exploration section of the game.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class ExplorationMenuButton : MonoBehaviour 
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
			GameObject.FindGameObjectWithTag("Player").GetComponent<MouseInput>().enabled = true;
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
