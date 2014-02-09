using UnityEngine;
using System;
using System.Collections;

public class RhythmInput : MonoBehaviour 
{
	public static Action<KeyCode> KeyPress;

	public UISprite cursor;
	public KeyCode redKey;
	public KeyCode blueKey;
	public KeyCode greenKey;
	public KeyCode yellowKey;
	public KeyCode options;

	private int keys;
	private float anchorPos = 0f;
	private bool bCursorAbove = false;

	private Ray ray;
	private Vector3 position;
	private RaycastHit[] hit;
	private UIAnchor cursorPos;

	void Start()
	{
		cursorPos = cursor.GetComponent<UIAnchor> ();
		keys = System.Enum.GetNames(typeof(KeyCode)).Length;
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			KeyPressed();
		}

		MoveCursor ();
	}

	private void MoveCursor()
	{
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

			bCursorAbove = CheckCursor();
		}
	}

	private bool CheckCursor()
	{
		position = new Vector3(cursorPos.relativeOffset.x * Screen.width, (cursorPos.relativeOffset.y + .5f) * Screen.height, 0f);
		ray = Camera.main.ScreenPointToRay (position);
		hit = Physics.RaycastAll(ray, Mathf.Infinity);

		return (hit.Length > 0);
	}

	private void KeyPressed()
	{
		for(int i = 0; i < keys; i++)
		{
			if (Input.GetKey((KeyCode)i))
			{
				if (KeyPress != null)
				{
					KeyPress((KeyCode)i);
				}
			}
		}
	}
}
