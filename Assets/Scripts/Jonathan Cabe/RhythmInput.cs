using UnityEngine;
using System.Collections;

public class RhythmInput : MonoBehaviour 
{
	public UISprite cursor;
	public Vector3 position;
	public float anchorPos = 0f;

	private UIAnchor cursorPos;

	void Start()
	{
		cursorPos = cursor.GetComponent<UIAnchor> ();
	}

	void Update()
	{
		MoveCursor ();
	}

	private void MoveCursor()
	{
		position = Input.mousePosition;
		anchorPos = Input.mousePosition.y / Screen.height - 0.5f;

		if (cursorPos)
		{
			if (anchorPos >= -0.225f && anchorPos <= 0.45f)
			{
				cursorPos.relativeOffset.y = anchorPos;
			}
			else if (anchorPos < -0.225f)
			{
				cursorPos.relativeOffset.y = -0.225f;
			}
			else if (anchorPos > 0.45f)
			{
				cursorPos.relativeOffset.y = 0.45f;
			}
		}
	}

	private void KeyPressed()
	{
		//if (Input.ke
	}
}
