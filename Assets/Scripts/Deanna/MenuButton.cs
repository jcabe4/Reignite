using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour 
{
	public enum ButtonType {NewGame, LoadGame, Practice, Options, Exit}
	public ButtonType type;
	
	void OnClick()
	{
		switch (type)
		{
		case ButtonType.NewGame:
		{
			Application.LoadLevel("Library 2F");
			break;
		}
		case ButtonType.LoadGame:
		{
			break;
		}
		case ButtonType.Practice:
		{
			Application.LoadLevel("Rhythm Section");
			break;
		}
		case ButtonType.Options:
		{
			break;
		}
		case ButtonType.Exit:
		{
			Application.Quit();
			break;
		}
		default:
		{
			break;
		}
		}
	}
}
