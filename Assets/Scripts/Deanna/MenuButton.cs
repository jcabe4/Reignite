using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour 
{
	public enum TButtonType {NewGame, LoadGame, Practice, Options, Exit}
	public TButtonType type;
	public UIAnchor cursor;

	void OnClick()
	{
		switch (type)
		{
			case TButtonType.NewGame:
			{
				LoadingDisplay.Instance.Load("Library 2F");
				break;
			}
			case TButtonType.LoadGame:
			{
				PlayerInformation.Instance.LoadGame();
				break;
			}
			case TButtonType.Practice:
			{
				LoadingDisplay.Instance.Load("Rhythm Section");
				break;
			}
			case TButtonType.Options:
			{
				break;
			}
			case TButtonType.Exit:
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

	void OnHover(bool bIsOver)
	{
		if (bIsOver)
		{
			cursor.container = gameObject;
		}
	}
}
