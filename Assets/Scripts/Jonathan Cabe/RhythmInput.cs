/*****************************************************
 * Program: Reignite
 * Script: RhythmInput.cs
 * Author: Jonathan Cabe
 * Description: This script that handles the input of
 * the player in the rhythm game scene.  It also 
 * generates events for any key pressed in the case
 * that they should be handled specifically.
 * ***************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RhythmInput : MonoBehaviour 
{
	public static Action<KeyCode> KeyPress;
	public static Action<Note.NoteColor> ColorPress;

	public Note.NoteColor noteColor;

	public UISprite cursor;
	public KeyCode redKey;
	public KeyCode blueKey;
	public KeyCode greenKey;
	public KeyCode yellowKey;
	public KeyCode options;
	public KeyCode confirm;

	public static RhythmInput Instance
	{
		get
		{
			return instance;
		}
	}
	private int keys;
	private float anchorPos = 0f;
	private bool bLiftUp = false;
	private bool bCursorAbove = false;

	private Ray ray;
	private Vector3 position;
	private RaycastHit[] hit;
	private UIAnchor cursorPos;
	private static RhythmInput instance;

	public List<KeyCode> colorKeys = new List<KeyCode>();
	public List<KeyCode> otherKeys = new List<KeyCode>();

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		cursorPos = cursor.GetComponent<UIAnchor> ();
		keys = System.Enum.GetNames(typeof(KeyCode)).Length;
	}

	void OnEnable()
	{
		KeyPress += CheckKeyDown;
	}

	void OnDisable()
	{
		KeyPress -= CheckKeyDown;
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			KeyPressed();
		}

		if (Input.anyKey)
		{
			CheckColorPress();
		}
		else
		{
			if (colorKeys.Count > 0)
			{
				colorKeys.Clear();
			}

			if (otherKeys.Count > 0)
			{
				otherKeys.Clear();
			}

			bLiftUp = true;
		}
		
		CheckKeyUp();
		MoveCursor ();
	}

	public bool CheckHover()
	{
		return bCursorAbove;
	}

	public bool GetLiftUp()
	{
		return bLiftUp;
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

		Debug.DrawRay(ray.origin, ray.direction, Color.green);

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
	
	private void CheckKeyUp()
	{
		for (int i = 0; i < colorKeys.Count; i++)
		{
			if (Input.GetKeyUp(colorKeys[i]))
			{
				colorKeys.RemoveAt(i);
				break;
			}
		}

		for (int i = 0; i < otherKeys.Count; i++)
		{
			if (Input.GetKeyUp(otherKeys[i]))
			{
				otherKeys.RemoveAt(i);
				break;
			}
		}
	}
	
	private void CheckKeyDown(KeyCode key)
	{
		if (!colorKeys.Contains(key) && (key == redKey || key == blueKey || key == greenKey || key == yellowKey))
		{
			bLiftUp = false;

			if (colorKeys.Count < 2)
			{
				colorKeys.Add(key);
			}
			else
			{
				otherKeys.Add(key);
			}
		}
		else if (!colorKeys.Contains(key) && !otherKeys.Contains(key) )
		{
			otherKeys.Add(key);
		}
	}

	private void CheckColorPress()
	{
		if (otherKeys.Count == 0 && colorKeys.Count > 0)
		{
			if (colorKeys.Contains(blueKey) && colorKeys.Contains(greenKey))
			{
				noteColor = Note.NoteColor.BlueGreen;
				ColorPress(Note.NoteColor.BlueGreen);
			}
			else if (colorKeys.Contains(blueKey) && colorKeys.Contains(redKey))
			{
				noteColor = Note.NoteColor.BlueRed;
				ColorPress(Note.NoteColor.BlueRed);
			}
			else if (colorKeys.Contains(blueKey) && colorKeys.Contains(yellowKey))
			{
				noteColor = Note.NoteColor.BlueYellow;
				ColorPress(Note.NoteColor.BlueYellow);
			}
			else if (colorKeys.Contains(greenKey) && colorKeys.Contains(redKey))
			{
				noteColor = Note.NoteColor.GreenRed;
				ColorPress(Note.NoteColor.GreenRed);
			}
			else if (colorKeys.Contains(greenKey) && colorKeys.Contains(yellowKey))
			{
				noteColor = Note.NoteColor.GreenYellow;
				ColorPress(Note.NoteColor.GreenYellow);
			}
			else if (colorKeys.Contains(redKey) && colorKeys.Contains(yellowKey))
			{
				noteColor = Note.NoteColor.RedYellow;
				ColorPress(Note.NoteColor.RedYellow);
			}
			else if (colorKeys.Contains(blueKey))
			{
				noteColor = Note.NoteColor.Blue;
				ColorPress(Note.NoteColor.Blue);
			}
			else if (colorKeys.Contains(greenKey))
			{
				noteColor = Note.NoteColor.Green;
				ColorPress(Note.NoteColor.Green);
			}
			else if (colorKeys.Contains(redKey))
			{
				noteColor = Note.NoteColor.Red;
				ColorPress(Note.NoteColor.Red);
			}
			else if (colorKeys.Contains(yellowKey))
			{
				noteColor = Note.NoteColor.Yellow;
				ColorPress(Note.NoteColor.Yellow);
			}
		}
		else if (otherKeys.Count > 0 || colorKeys.Count == 0)
		{
			ColorPress(Note.NoteColor.Pause);
		}
	}
}
