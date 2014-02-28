using UnityEngine;
using System.Collections;

public class LabelEffect : MonoBehaviour 
{
	public Color textColor;
	public Color hoverColor;
	private UILabel label;
	
	void Start () 
	{
		label = gameObject.GetComponent<UILabel>();
		label.color = textColor;
	}

	void OnHover(bool hover)
	{
		if (hover)
		{
			label.color = hoverColor;
		}
		else
		{
			label.color = textColor;
		}
	}
}
