using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MusicSheetBuilder : EditorWindow 
{
	private string fileName;
	private string songName;
	private string savePath;
	private Vector2 endTime;
	private Vector2 startTime;
	private Vector2 scrollPos;
	
	private int pitchPos = 0;
	private float endTotal = 0f;
	private float startTotal = 0f;
	private float lengthRatio = 1f;
	private float noteDuration = 0f;
	private bool bNewNote = false;
	
	private List<Note> notes = new List<Note>();
	private List<bool> displayNotes = new List<bool>();
	private Note.NoteColor color = Note.NoteColor.Pause;
	
	[MenuItem("JC Tools / Music Builder")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow<MusicSheetBuilder> ();
	}
	
	void OnGUI()
	{
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
		{
			songName = EditorGUILayout.TextField ("Song Name: ", songName);
			fileName = songName + ".xml";
			
			EditorGUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Load File"))
				{
					LoadSong(songName);
				}
				
				if(GUILayout.Button("Save File"))
				{
					if (notes.Count > 0)
					{
						SaveFile(fileName);
					}
					else
					{
						Debug.Log("Song Not Saved! Please Create Notes First!");
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Label("Relative Second To Screen Size Ratio", EditorStyles.boldLabel);
			lengthRatio = EditorGUILayout.Slider(lengthRatio, .001f, 10f);
			
			EditorGUILayout.Space();
			
			bNewNote = EditorGUILayout.BeginToggleGroup("Create New Note", bNewNote);
			{
				EditorGUI.indentLevel++;
				pitchPos = EditorGUILayout.IntField("Pitch Position: ", pitchPos);
				color = (Note.NoteColor)EditorGUILayout.EnumPopup("Note Color: ", color);
				
				startTime = EditorGUILayout.Vector2Field("Start Time\t(Min:Sec): ", startTime);
				startTime.x = Mathf.Floor(startTime.x);
				endTime = EditorGUILayout.Vector2Field("End Time\t(Min:Sec): ", endTime);
				endTime.x = Mathf.Floor(endTime.x);
				
				startTime.x = Mathf.Clamp(startTime.x, 0f, Mathf.Infinity);
				startTime.y = Mathf.Clamp(startTime.y, 0f, 59.99f);
				endTime.x = Mathf.Clamp(endTime.x, startTime.x, Mathf.Infinity);
				
				if (startTime.x == endTime.x)
				{
					endTime.y = Mathf.Clamp(endTime.y, startTime.y, 59.99f);
				}
				else
				{
					endTime.y = Mathf.Clamp(endTime.y, 0f, 60f);
				}
				
				
				startTotal = startTime.x * 60f + startTime.y;
				endTotal = endTime.x * 60f + endTime.y;
				noteDuration = endTotal - startTotal;
				
				EditorGUILayout.BeginHorizontal();
				{
					if(GUILayout.Button("Reset Fields"))
					{
						pitchPos = 0;
						startTotal = 0;
						
						color = Note.NoteColor.Pause;
						endTime = Vector2.zero;
						startTime = Vector2.zero;
					}
					
					if(GUILayout.Button("Save Note"))
					{
						Note createdNote = new Note();
						
						createdNote.color = color;
						createdNote.endTime = endTime;
						createdNote.endTotal = endTotal;
						createdNote.startTime = startTime;
						createdNote.startTotal = startTotal;
						createdNote.pitchPosition = pitchPos;
						createdNote.noteDuration = noteDuration;
						
						notes.Add(createdNote);
						displayNotes.Add(false);
						SortNotes();
					}
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndToggleGroup();
			
			for(int i = 0; i < notes.Count; i++)
			{
				if (displayNotes.Count != notes.Count)
				{
					Debug.Log("DisplayNotes != Notes");
					break;
				}
				
				displayNotes[i] = EditorGUILayout.Foldout(displayNotes[i], "Note " + (i+1).ToString() + ": ");
				
				if (displayNotes[i])
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Pitch Position: ", notes[i].pitchPosition.ToString());
					EditorGUILayout.LabelField("Color: ", notes[i].color.ToString());
					EditorGUILayout.LabelField("Note Duration: ", notes[i].noteDuration.ToString("0.00") + " Seconds");
					EditorGUILayout.LabelField("Start Time: ", notes[i].startTime.x.ToString() + ":" + notes[i].startTime.y.ToString("00.00"));
					EditorGUILayout.LabelField("End Time: ", notes[i].endTime.x.ToString() + ":" + notes[i].endTime.y.ToString("00.00"));
					
					if (GUILayout.Button("Delete Note"))
					{
						notes.RemoveAt(i);
						displayNotes.RemoveAt(i);
						break;
					}
					
					EditorGUI.indentLevel--;
				}
			}
		}
		EditorGUILayout.EndScrollView ();
	}
	
	void SortNotes()
	{
		int min;
		Note temp;
		
		for (int i = 0; i < notes.Count; i++)
		{
			min = i;
			
			for (int j = i; j < notes.Count; j++)
			{
				if (notes[min].startTotal > notes[j].startTotal)
				{
					min = j;
				}
			}
			
			temp = notes[min];
			notes[min] = notes[i];
			notes[i] = temp;
		}
	}
	
	void LoadSong (string songName)
	{
		int setIndex = 0;
		string resourcePath = "Song Data/" + songName;
		savePath = "Assets/Resources/Song Data/" + songName + ".xml";
		
		if (!File.Exists(savePath))
		{
			Debug.Log(songName + ".xml Not Found @ " + savePath);
			return;
		}
		else
		{
			Debug.Log("Loading " + songName + ".xml @ \n" + savePath);
		}
		
		TextAsset asset = (TextAsset)Resources.Load(resourcePath);
		XmlReader reader = XmlReader.Create(new StringReader(asset.text));
		
		notes.Clear();
		displayNotes.Clear();
		
		while(reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
			{
				switch (reader.Name)
				{
				case "LengthRatio":
				{
					lengthRatio = float.Parse(reader.ReadElementString());
					break;
				}
				case "Note":
				{
					notes.Add(new Note());
					displayNotes.Add(false);
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
	
	void SaveFile (string file)
	{
		savePath = "Assets/Resources/Song Data/" + file;
		
		if (File.Exists(savePath))
		{
			Debug.Log("Overwriting " + file + " @ \n" + savePath);
			File.Delete(savePath);
		}
		else
		{
			Debug.Log("Saving " + file + " @ \n" + savePath);
		}
		
		XmlWriter writer = new XmlTextWriter (savePath, System.Text.Encoding.UTF8);
		
		writer.WriteStartDocument();
		writer.WriteWhitespace ("\n");
		writer.WriteStartElement ("Root");
		writer.WriteWhitespace ("\n\t");
		writer.WriteStartElement ("Body");
		
		for (int i = 0; i < notes.Count; i++)
		{
			writer.WriteWhitespace("\n\t\t");
			writer.WriteElementString("LengthRatio", lengthRatio.ToString());
			writer.WriteWhitespace("\n\t\t");
			writer.WriteStartElement("Note");
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("Color", ((int)notes[i].color).ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("StartTotal", notes[i].startTotal.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("EndTotal", notes[i].endTotal.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("NoteDuration", notes[i].noteDuration.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("PitchPosition", notes[i].pitchPosition.ToString());
			
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteStartElement("StartTime");
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("Minutes", notes[i].startTime.x.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("Seconds", notes[i].startTime.y.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteEndElement();
			
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteStartElement("EndTime");
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("Minutes", notes[i].endTime.x.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("Seconds", notes[i].endTime.y.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteEndElement();
			
			writer.WriteWhitespace("\n\t\t");
			writer.WriteEndElement();
		}
		
		writer.WriteWhitespace ("\n\t");
		writer.WriteEndElement ();
		writer.WriteWhitespace ("\n");
		writer.WriteEndElement ();
		writer.WriteEndDocument ();
		
		writer.Close ();
	}
}
