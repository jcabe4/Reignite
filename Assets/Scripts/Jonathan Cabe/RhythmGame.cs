using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class RhythmGame : MonoBehaviour 
{
	public delegate void ScrollMusic(float distance);
	public static event ScrollMusic Scroll;

	struct Multiplier
	{
		const float bad = 1.25f;
		const float good = 2f;
		const float perfect = 4f;
	}

	struct PointValue
	{
		const float initial = 100f;
		const float hold = 10f;
	}

	public string song;
	public UISprite beatBar;
	public UILabel scoreLabel;
	public UISlider songProgressBar;
	public Object singlePrefab;
	public Object doublePrefab;
	public Transform noteParent;
	public AudioClip [] songDatabase;
	public UIAnchor [] spawnPositions;
	
	private UIAnchor pos;
	private float noteWidth;
	private RhythmNote rNote;
	private Note.NoteColor color;
	private PointValue pointValue;
	private Multiplier multiplier;

	private int score = 0;
	private int noteIndex = 0;
	private int notesSpawned = 0;
	private float deltaTime = 0f;
	private float timeLapsed = 0f;
	private float timeOffset = 0f;
	private float lengthRatio = 1f;
	private float moveDistance = 0f;
	private float totalNoteTime = 0f;
	private bool bPlaySong = false;
	private bool bEarnedPoints = false;
	private Color32 defaultColor = new Color32(0, 200, 255, 150);
	
	private string path;
	private RhythmInput rInput;
	private AudioSource musicSource;
	private Note currentNote = new Note();
	private List<Note> notes = new List<Note>();
	
	void Start()
	{
		rInput = gameObject.GetComponent<RhythmInput>();
		musicSource = gameObject.GetComponent<AudioSource>();
		
		LoadSong(song);
		CalculateLength();
		timeOffset = 1f / lengthRatio;
		timeLapsed -= timeOffset;
	}

	void OnEnable()
	{
		RhythmInput.KeyPress += PauseGame;
		RhythmInput.ColorPress += AddScore;
		RhythmInput.ColorPress += ChangeBeatBar;
	}

	void OnDisable()
	{
		RhythmInput.KeyPress -= PauseGame;
		RhythmInput.ColorPress -= AddScore;
		RhythmInput.ColorPress -= ChangeBeatBar;
	}
	
	void Update()
	{
		if (bPlaySong)
		{
			deltaTime = Time.deltaTime;
			timeLapsed += deltaTime;
			moveDistance = lengthRatio * Time.deltaTime;

			CreateNotes();
			AddCursorScore();

			if (Scroll != null)
			{
				Scroll(moveDistance);
			}
		}

		if (songProgressBar && timeLapsed < totalNoteTime)
		{
			songProgressBar.value = (timeLapsed / totalNoteTime);
		}

		if (!Input.anyKey)
		{
			beatBar.color = defaultColor;
		}
	}

	public void AddScore(Note.NoteColor colorPressed)
	{
		if (bPlaySong)
		{
			if (currentNote.endTotal < timeLapsed)
			{
				for (int i = 0; i < notes.Count; i++)
				{
					if (notes[i].startTotal < timeLapsed && notes[i].endTotal > timeLapsed)
					{
						currentNote = notes[i];
						break;
					}
				}
			}

			if (currentNote.color == colorPressed)
			{
				Debug.Log("ADD SCORE!");
			}
			else
			{
				Debug.Log("NO SCORE!");
			}
		}
	}

	private void ChangeBeatBar(Note.NoteColor colorPressed)
	{
		if (beatBar)
		{
			if (colorPressed == Note.NoteColor.Blue || colorPressed == Note.NoteColor.BlueGreen ||
			    colorPressed == Note.NoteColor.BlueRed || colorPressed == Note.NoteColor.BlueYellow)
			{
				beatBar.color = RhythmNote.blue;
			}
			else if (colorPressed == Note.NoteColor.Green || colorPressed == Note.NoteColor.GreenRed ||
			         colorPressed == Note.NoteColor.GreenYellow)
			{
				beatBar.color = RhythmNote.green;
			}
			else if (colorPressed == Note.NoteColor.Red || colorPressed == Note.NoteColor.RedYellow)
			{
				beatBar.color = RhythmNote.red;
			}
			else if (colorPressed == Note.NoteColor.Yellow)
			{
				beatBar.color = RhythmNote.yellow;
			}
			else if (colorPressed == Note.NoteColor.Pause)
			{
				beatBar.color = defaultColor;
			}
		}
	}

	private void PauseGame(KeyCode key)
	{
		if (rInput.options == key)
		{
			bPlaySong = !bPlaySong;
		}
	}
	private void AddCursorScore()
	{
		if (rInput.CheckHover())
		{
			score += 1;

			scoreLabel.text = score.ToString("0000000000");
		}
	}

	private void CreateNotes()
	{
		if (noteIndex < notes.Count)
		{
			if (notes[noteIndex].startTotal < timeLapsed + timeOffset)
			{
				pos = spawnPositions[notes[noteIndex].pitchPosition];
				color = notes[noteIndex].color;
				noteWidth = notes[noteIndex].noteDuration * lengthRatio;
				
				switch(notes[noteIndex].color)
				{
				case Note.NoteColor.Blue:
				case Note.NoteColor.Yellow:
				case Note.NoteColor.Green:
				case Note.NoteColor.Red:
				{
					GameObject newNote = (GameObject)Instantiate(singlePrefab);
					newNote.transform.parent = noteParent.transform;
					newNote.transform.localScale = Vector3.one;
					newNote.name = notesSpawned.ToString() + ": Note (" + notes[noteIndex].color.ToString() + ")";
					
					rNote = newNote.GetComponent<RhythmNote>();
					rNote.Init(pos, color, noteWidth, gameObject);
					notesSpawned++;
					break;
				}
				default:
				{
					GameObject newNote = (GameObject)Instantiate(doublePrefab);
					newNote.transform.parent = noteParent.transform;
					newNote.transform.localScale = Vector3.one;
					newNote.name = notesSpawned.ToString() + ": Note (" + notes[noteIndex].color.ToString() + ")";
					
					rNote = newNote.GetComponent<RhythmNote>();
					rNote.Init(pos, color, noteWidth, gameObject);
					notesSpawned++;
					break;
				}
				}
				
				noteIndex++;
			}
		}
	}

	private void CalculateLength()
	{
		for (int i = 0; i < notes.Count; i++)
		{
			totalNoteTime += notes[i].noteDuration;
		}
	}
	
	private int FindSong(string songName)
	{
		for (int i = 0; i < songDatabase.Length; i++)
		{
			if (songDatabase[i].name == songName)
			{
				return i;
			}
		}
		
		return -1;
	}
	
	private void LoadSong (string songName)
	{
		song = songName;
		int setIndex = 0;
		int songIndex = 0;
		string resourcePath = "Song Data/" + songName;
		
		songIndex = FindSong(songName);
		path = "Assets/Resources/Song Data/" + songName + ".xml";
		
		if (songIndex >= 0)
		{
			musicSource.clip = songDatabase[songIndex];
		}
		else
		{
			Debug.Log("Song Clip Not Found!!");
			//return;
		}
		
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
						lengthRatio = float.Parse(reader.ReadElementString());
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
