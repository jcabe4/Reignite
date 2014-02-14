using UnityEngine;
using System.Collections;

public class TitleButton : MonoBehaviour 
{
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
}
