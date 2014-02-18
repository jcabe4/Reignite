using UnityEngine;
using System.Collections;

public class GUIAnimationGrabber : MonoBehaviour 
{
	public bool defaultContainer;
	private UIAnchor GUIAnimationAnchor;

	void Start () 
	{
		GUIAnimationAnchor = GameObject.FindGameObjectWithTag("GUI Animation").GetComponent<UIAnchor>();
		if (defaultContainer == true)
		{
			GUIAnimationAnchor.container = gameObject;
		}
	}

	void OnHover(bool hover)
	{
		if(hover)
		{
			GUIAnimationAnchor.container = gameObject;
		}
	}
}
