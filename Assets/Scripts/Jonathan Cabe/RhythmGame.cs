using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class RhythmGame : MonoBehaviour 
{
	public delegate void ScrollMusic(float distance);
	public static event ScrollMusic Scroll;

	public string song;

	public float timeLapsed = 0f;
	public float lengthRatio = 1f;

	public Object singlePrefab;
	public Object doublePrefab;

	public Transform noteParent;

	public UIAnchor[] spawnPositions;
	
	private string path;
	private float moveDistance;
	private List<Note> notes = new List<Note>();

	void Start()
	{
		LoadSong(song);
	}

	void Update()
	{
		timeLapsed += Time.deltaTime;
		moveDistance = lengthRatio * Time.deltaTime;

		Scroll(moveDistance);
	}

	void LoadSong (string songName)
	{
		int setIndex = 0;
		string resourcePath = "Song Data/" + songName;
		path = "Assets/Resources/Song Data/" + songName + ".xml";
		
		if (!File.Exists(path))
		{
			Debug.Log(songName + ".xml Not Found @ " + path);
			return;
		}
		else
		{
			Debug.Log("Loading " + songName + ".xml @ \n" + path);
		}
		
		TextAsset asset = (TextAsset)Resources.Load(resourcePath);
		XmlReader reader = XmlReader.Create(new StringReader(asset.text));
		
		notes.Clear();
		
		while(reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
			{
				switch (reader.Name)
				{
					case "LengthRatio":
					{
						lengthRatio = int.Parse(reader.ReadElementString());
						break;
					}
					case "Note":
					{
						notes.Add(new Note());
						setIndex = notes.Count - 1;
						break;
					}
					case "Color":
					{
						notes[setIndex].color = (Note.NoteColor)int.Parse(reader.ReadElementString());
						break;
					}
					case "StartTotal":
					{
						notes[setIndex].startTotal = float.Parse(reader.ReadElementString());
						break;
					}
					case "EndTotal":
					{
						notes[setIndex].endTotal = float.Parse(reader.ReadElementString());
						break;
					}
					case "NoteDuration":
					{
						notes[setIndex].noteDuration = float.Parse(reader.ReadElementString());
						break;
					}
					case "PitchPosition":
					{
						notes[setIndex].pitchPosition = int.Parse(reader.ReadElementString());
						break;
					}
					case "StartTime":
					{
						Vector2 time = new Vector2();
						reader.ReadToDescendant("Minutes");
						time.x = float.Parse(reader.ReadElementContentAsString());
						reader.ReadToNextSibling("Seconds");
						time.y = float.Parse(reader.ReadElementContentAsString());
						
						notes[setIndex].startTime = time;
						break;
					}
					case "EndTime":
					{
						Vector2 time = new Vector2();
						reader.ReadToDescendant("Minutes");
						time.x = float.Parse(reader.ReadElementString());
						reader.ReadToNextSibling("Seconds");
						time.y = float.Parse(reader.ReadElementString());
						
						notes[setIndex].endTime = time;
						break;
					}
					default:
					{
						break;
					}
				}
			}
		}
	}
}
