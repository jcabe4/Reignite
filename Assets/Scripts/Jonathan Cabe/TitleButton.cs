using UnityEngine;
using System.Collections;

public class TitleButton : MonoBehaviour 
{
	public enum TButtonType {NewGame, LoadGame, Practice, Options, Exit}
	public TButtonType type;

	private UISprite sprite;

	void Start()
	{
		sprite = gameObject.GetComponent<UISprite>();

		sprite.alpha = 0f;
	}

	void OnHover(bool hover)
	{
		if (hover)
		{
			sprite.alpha = 1f;
		}
		else
		{
			sprite.alpha = 0f;
		}
	}

	void OnClick()
	{
		switch (type)
		{
			case TButtonType.NewGame:
			{
				Application.LoadLevel("Library 2F");
				break;
			}
			case TButtonType.LoadGame:
			{
				break;
			}
			case TButtonType.Practice:
			{
				Application.LoadLevel("Rhythm Section");
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
}
