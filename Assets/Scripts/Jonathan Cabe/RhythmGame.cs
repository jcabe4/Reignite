/*****************************************************
 * Program: Reignite
 * Script: RhythmGame.cs
 * Author: Jonathan Cabe
 * Description: This is the game controller for the
 * rhythm game.  This handles audio playback, moving
 * the notes, score handling, and interpreting input.
 * ***************************************************/

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
		public const float bad = 1.25f;
		public const float good = 2f;
		public const float perfect = 4f;
	}

	struct PointValue
	{
		public const float initial = 100f;
		public const float hold = 10f;
	}

	public static RhythmGame Instance
	{
		get
		{
			return instance;
		}
	}

	public string song;
	public UISprite beatBar;
	public UILabel scoreLabel;
	public UIPanel optionsPanel;
	public UISlider songProgressBar;
	public Object labelPrefab;
	public Object singlePrefab;
	public Object doublePrefab;
	public Transform noteParent;
	public Transform modifierParent;
	public AudioClip [] songDatabase;
	public UIAnchor [] spawnPositions;
	
	private UIAnchor pos;
	private float noteWidth;
	private RhythmNote rNote;
	private Note.NoteColor color;

	private int score = 0;
	private int noteIndex = 0;
	private int hoverBonus = 0;
	private int notesSpawned = 0;
	private float deltaTime = 0f;
	private float timeLapsed = 0f;
	private float timeOffset = 0f;
	private float lengthRatio = 1f;
	private float moveDistance = 0f;
	private float totalNoteTime = 0f;
	private bool bMissed = false;
	public bool bPlaySong = false;
	public bool bBeginGame = false;
	public bool bEarnedPoints = false;
	public bool bCanEarnPoints = true;
	private Color32 defaultColor = new Color32(219, 219, 219, 150);
	private byte beatBarAlpha = 150;

	private string path;
	private RhythmInput rInput;
	private UILabel modifierLabel;
	private AudioSource musicSource;
	private Note currentNote = new Note();
	private List<Note> notes = new List<Note>();
	
	private static RhythmGame instance;
	
	void Start()
	{
		rInput = gameObject.GetComponent<RhythmInput>();
		musicSource = gameObject.GetComponent<AudioSource>();
		
		LoadSong(song);
		CalculateLength();
		timeOffset = 1f / lengthRatio;
		timeLapsed -= timeOffset;

		instance = this;
	}

	void OnEnable()
	{
		RhythmInput.KeyPress += KeyCommand;
		RhythmInput.ColorPress += AddScore;
		RhythmInput.ColorPress += ChangeBeatBar;
	}

	void OnDisable()
	{
		RhythmInput.KeyPress -= KeyCommand;
		RhythmInput.ColorPress -= AddScore;
		RhythmInput.ColorPress -= ChangeBeatBar;
	}
	
	void Update()
	{
		if (bBeginGame && bPlaySong && timeLapsed < audio.clip.length)
		{
			if (timeLapsed < 0f || timeLapsed + 1f > audio.clip.length)
			{
				deltaTime = Time.deltaTime;
			}
			else
			{
				if (!audio.isPlaying && timeLapsed < totalNoteTime)
				{
					audio.Play();
				}

				deltaTime = audio.time - timeLapsed;
			}
			
			timeLapsed += deltaTime;
			moveDistance = lengthRatio * deltaTime;

			CreateNotes();
			CheckCurrentNote();

			if (Scroll != null)
			{
				Scroll(moveDistance);
			}
			if (rInput.GetLiftUp())
			{
				bCanEarnPoints = true;
			}
		}
		else if (bBeginGame && bPlaySong)
		{
			bBeginGame = false;
			bPlaySong = false;

			Application.LoadLevel("Title Screen");
		}

		if (songProgressBar && timeLapsed < totalNoteTime)
		{
			songProgressBar.value = (timeLapsed / totalNoteTime);
		}

		if (!Input.anyKey)
		{
			beatBar.color = defaultColor;

			if (audio.volume > .1f && currentNote.color != Note.NoteColor.Pause)
			{
				audio.volume -= Time.deltaTime * .5f;
			}
		}
	}

	public void BeginGame()
	{
		if (!bBeginGame)
		{
			bPlaySong = true;
			bBeginGame = true;
		}
	}

	private void AddScore(Note.NoteColor colorPressed)
	{
		if (bPlaySong)
		{			
			if (rInput.CheckHover())
			{
				hoverBonus = 2;
			}
			else
			{
				hoverBonus = 1;
			}

			if (currentNote.color == Note.NoteColor.Pause)
			{
				bCanEarnPoints = false;
			}

			if (currentNote.color == colorPressed && colorPressed != Note.NoteColor.Pause)
			{
				if (bCanEarnPoints)
				{
					if (!bEarnedPoints)
					{
						if (timeLapsed >= currentNote.startTotal &&
						    timeLapsed < (currentNote.startTotal + currentNote.noteDuration * .125f))
						{
							score += (int)(PointValue.initial * Multiplier.perfect * hoverBonus);
							SpawnText("perfect");
						}
						else if (timeLapsed >= currentNote.startTotal && 
						         timeLapsed < (currentNote.startTotal + currentNote.noteDuration * .25f))
						{
							score += (int)(PointValue.initial * Multiplier.good * hoverBonus);
							SpawnText("good");
						}
						else if (timeLapsed >= currentNote.startTotal && 
						         timeLapsed < (currentNote.startTotal + currentNote.noteDuration * .5f))
						{
							score += (int)(PointValue.initial * Multiplier.bad * hoverBonus);
							SpawnText("bad");
						}
						else
						{
							score += (int)(PointValue.initial * hoverBonus);
							SpawnText("miss");
						}

						bEarnedPoints = true;
					}
					else
					{
						score +=(int)(PointValue.hold * hoverBonus);
					}

					if (rInput.CheckHover())
					{
						audio.volume += Time.deltaTime * .75f;
					}
					else if (audio.volume < .4f)
					{
						audio.volume += Time.deltaTime * .5f;
					}
				}
				else
				{
					if (audio.volume > .1f)
					{
						audio.volume -= Time.deltaTime * .5f;
					}
				}
			}
			else if (currentNote.color != Note.NoteColor.Pause)
			{
				if (audio.volume > .1f)
				{
					audio.volume -= Time.deltaTime * .5f;
				}

				if (!bEarnedPoints && !bMissed)
				{
					SpawnText("miss");
					StartCoroutine("Delay", .5f);
				}
			}
			
			scoreLabel.text = score.ToString("0000000000");
		}
	}

	private void CheckCurrentNote()
	{
		if (currentNote.endTotal < timeLapsed)
		{
			for (int i = 0; i < notes.Count; i++)
			{
				if (notes[i].startTotal < timeLapsed && notes[i].endTotal > timeLapsed)
				{
					currentNote = notes[i];
					bEarnedPoints = false;

					if (!bMissed)
					{
						StartCoroutine("Delay", .5f);
					}

					break;
				}
			}
		}
	}

	private void SpawnText(string Text)
	{
		switch (Text)
		{
			case "miss":
			{
				GameObject text = (GameObject)Instantiate(labelPrefab, rInput.cursor.transform.position, Quaternion.identity);
				text.transform.parent = modifierParent.transform;
				modifierLabel = text.GetComponent<UILabel>();

				modifierLabel.text = "miss";
				modifierLabel.color = Color.white;
				modifierLabel.depth = 5;

				text.rigidbody.AddForce(Vector3.up * 100f);

				DestroyObject(text, 1f);
				
				break;
			}
			case "bad":
			{
				GameObject text = (GameObject)Instantiate(labelPrefab, rInput.cursor.transform.position, Quaternion.identity);
				text.transform.parent = modifierParent.transform;
				modifierLabel = text.GetComponent<UILabel>();

				modifierLabel.text = "bad";
				modifierLabel.color = Color.red;
				modifierLabel.depth = 5;

				text.rigidbody.AddForce(Vector3.up * 100f);

				DestroyObject(text, 1f);
				
				break;
			}
			case "good":
			{
				GameObject text = (GameObject)Instantiate(labelPrefab, rInput.cursor.transform.position, Quaternion.identity);
				text.transform.parent = modifierParent.transform;
				modifierLabel = text.GetComponent<UILabel>();

				modifierLabel.text = "GOOD";
				modifierLabel.color = Color.green;
				modifierLabel.depth = 5;

				text.rigidbody.AddForce(Vector3.up * 100f);

				DestroyObject(text, 1f);
				
				break;
			}
			case "perfect":
			{
				GameObject text = (GameObject)Instantiate(labelPrefab, rInput.cursor.transform.position, Quaternion.identity);
				text.transform.parent = modifierParent.transform;
				modifierLabel = text.GetComponent<UILabel>();

				modifierLabel.text = "PERFECT!";
				modifierLabel.color = Color.yellow;
				modifierLabel.depth = 5;

				text.rigidbody.AddForce(Vector3.up * 100f);

				DestroyObject(text, 1f);
				
				break;
			}
			default:
			{
				Debug.Log("Spawn Text: No Case For Argument!");
				break;
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
				beatBar.color = new Color32(RhythmNote.blue.r, RhythmNote.blue.g, RhythmNote.blue.b, beatBarAlpha);
			}
			else if (colorPressed == Note.NoteColor.Green || colorPressed == Note.NoteColor.GreenRed ||
			         colorPressed == Note.NoteColor.GreenYellow)
			{
				beatBar.color = new Color32(RhythmNote.green.r, RhythmNote.green.g, RhythmNote.green.b, beatBarAlpha);
			}
			else if (colorPressed == Note.NoteColor.Red || colorPressed == Note.NoteColor.RedYellow)
			{
				beatBar.color = new Color32(RhythmNote.red.r, RhythmNote.red.g, RhythmNote.red.b, beatBarAlpha);
			}
			else if (colorPressed == Note.NoteColor.Yellow)
			{
				beatBar.color = new Color32(RhythmNote.yellow.r, RhythmNote.yellow.g, RhythmNote.yellow.b, beatBarAlpha);
			}
			else if (colorPressed == Note.NoteColor.Pause)
			{
				beatBar.color = defaultColor;
			}
		}
	}

	private void KeyCommand(KeyCode key)
	{
		if (rInput.confirm == key)
		{
			if (!bBeginGame && optionsPanel.alpha == 0f)
			{
				bPlaySong = true;
				bBeginGame = true;
			}
		}
		else if (rInput.options == key)
		{
			if (bBeginGame)
			{
				bPlaySong = !bPlaySong;

				if (!bPlaySong)
				{
					audio.Pause();
					optionsPanel.alpha = 1f;
				}
				else
				{
					optionsPanel.alpha = 0f;

					if (timeLapsed > 0f)
					{
						audio.Play();
						
					}
				}
			}
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
			//return;
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

	IEnumerator Delay(float time)
	{
		bMissed = true;
		yield return new WaitForSeconds(time);
		bMissed = false;
	}
}
