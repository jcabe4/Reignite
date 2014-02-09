using UnityEngine;
using System.Collections;

public class RhythmNote : MonoBehaviour 
{
	public Note.NoteColor color;
	
	public UISprite bar1;
	public UISprite bar2;

	private UIAnchor anchor;
	private UIStretch barStretch1;
	private UIStretch barStretch2;
	private Vector2 size = new Vector2();
	
	private static float singleNote = .07f;
	private static float doubleNote = .035f;
	public static Color32 blue = new Color32(0, 0, 255, 255);
	public static Color32 green = new Color32(0, 150, 0, 255);
	public static Color32 red = new Color(255, 0, 0, 255);
	public static Color32 yellow = new Color32(255, 255, 0, 255);
	
	void Start()
	{
		if (!anchor)
		{
			anchor = gameObject.GetComponent<UIAnchor>();
		}
		
		if (bar1 && !barStretch1)
		{
			barStretch1 = bar1.gameObject.GetComponent<UIStretch>();
		}
		
		if (bar2 && !barStretch2)
		{
			barStretch2 = bar2.gameObject.GetComponent<UIStretch>();
		}
	}
	
	void OnEnable()
	{
		RhythmGame.Scroll += MoveNote;
	}
	
	void OnDisable()
	{
		RhythmGame.Scroll -= MoveNote;
	}

	void Update()
	{
		DestroySelf();
	}

	public void Init(UIAnchor pos, Note.NoteColor newColor, float width, GameObject gameObj)
	{
		Start();
		
		color = newColor;
		anchor.relativeOffset.x = pos.relativeOffset.x;
		anchor.relativeOffset.y = pos.relativeOffset.y;

		SetColor();
		SetWidth(width);
	}
	
	private void MoveNote(float distance)
	{
		anchor.relativeOffset.x -= distance;
	}
	
	private void SetColor()
	{
		switch (color)
		{
			case Note.NoteColor.Pause:
			{
				DestroyObject(gameObject);
				break;
			}
			case Note.NoteColor.Blue:
			{
				bar1.color = blue;
				break;
			}
			case Note.NoteColor.Green:
			{
				bar1.color = green;
				break;
			}
			case Note.NoteColor.Red:
			{
				bar1.color = red;
				break;
			}
			case Note.NoteColor.Yellow:
			{
				bar1.color = yellow;
				break;
			}
			case Note.NoteColor.BlueGreen:
			{
				bar1.color = blue;
				
				if (bar2)
					bar2.color = green;
				
				break;
			}
			case Note.NoteColor.BlueRed:
			{
				bar1.color = blue;
				
				if (bar2)
					bar2.color = red;
				
				break;
			}
			case Note.NoteColor.BlueYellow:
			{
				bar1.color = blue;
				
				if (bar2)
					bar2.color = yellow;
				
				break;
			}
			case Note.NoteColor.GreenRed:
			{
				bar1.color = green;
				
				if (bar2)
					bar2.color = red;
				
				break;
			}
			case Note.NoteColor.GreenYellow:
			{
				bar1.color = green;
				
				if (bar2)
					bar2.color = yellow;
				
				break;
			}
			case Note.NoteColor.RedYellow:
			{
				bar1.color = red;
				
				if (bar2)
					bar2.color = yellow;
				
				break;
			}
			default:
			{
				Debug.Log("SetColor() Failed!");
				break;
			}
		}
	}
	
	private void SetWidth(float width)
	{
		if (barStretch1 && barStretch2)
		{
			size.Set(width, doubleNote);
			
			barStretch1.relativeSize = size;
			barStretch2.relativeSize = size;
		}
		else if (barStretch1)
		{
			size.Set(width, singleNote);
			
			barStretch1.relativeSize = size;
		}
		else
		{
			Debug.Log("SetWidth() Failed!");
		}
	}

	private void DestroySelf()
	{
		if (anchor.relativeOffset.x < (-1.5f - barStretch1.relativeSize.x))
		{
			RhythmGame.Scroll -= MoveNote;
			Destroy(gameObject);
		}
	}

}
