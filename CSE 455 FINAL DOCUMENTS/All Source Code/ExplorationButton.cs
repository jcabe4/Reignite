using UnityEngine;
using System.Collections;

public class ExplorationButton : MonoBehaviour 
{
	public enum ButtonType {Inventory, Pause, Resume, Save, Load, Exit};

	public ButtonType type;

	void Start()
	{
	}

	void OnClick()
	{
		switch (type)
		{
			case ButtonType.Inventory:
			{
				InGameElements.Instance.ToggleInventory();
				break;
			}
			case ButtonType.Pause:
			{
				InGameElements.Instance.TogglePause();
				break;
			}
			case ButtonType.Resume:
			{
				InGameElements.Instance.TogglePause();
				break;
			}
			case ButtonType.Save:
			{
				InGameElements.Instance.TogglePause();
				PlayerInformation.Instance.SaveData();
				break;
			}
			case ButtonType.Load:
			{
				InGameElements.Instance.TogglePause();
				PlayerInformation.Instance.LoadGame();
				break;
			}
			case ButtonType.Exit:
			{
				if (LoadingDisplay.Instance)
				{
					LoadingDisplay.Instance.Load("Title Screen");
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
