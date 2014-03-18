/*****************************************************
 * Program: Reignite
 * Script: PlayerInformation.cs
 * Author: Jonathan Cabe
 * Description: 
 * ***************************************************/

using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class PlayerInformation : MonoBehaviour
{
	public string songToLoad;
	public string currentScene;
	public bool bPractice = false;
	public List<Item> items = new List<Item>();
	
	private string savePath;
	private string fileName = "Info";
	private bool bToRhythm = false;
	private bool bFromRhythm = false;
	private bool bLoadingGame = false;
	private bool bDisplayTutorial = false;

	private Vector3 playerPos;
	private Vector3 cameraPos;
	private GameObject player;

	public static PlayerInformation Instance
	{
		get
		{
			return instance;
		}
	}

	private static PlayerInformation instance;

	void Awake()
	{
		#if UNITY_EDITOR
		savePath = Application.dataPath + "/Resources/Player Data/" + fileName + ".xml";
		#endif

		#if !UNITY_EDITOR
		savePath = Application.persistentDataPath + "/" + fileName + ".xml";
		#endif

		if (!instance)
		{
			instance = this;
		}

		DontDestroyOnLoad (gameObject);
	}
	
	void OnLevelWasLoaded()
	{
		if (bFromRhythm)
		{
			player = GameObject.FindGameObjectWithTag("Player");
			
			if (player)
			{
				player.transform.position = playerPos;
				Camera.main.transform.position = cameraPos;
			}
			
			bFromRhythm = false;
		}
		else if (bLoadingGame)
		{
			player = GameObject.FindGameObjectWithTag("Player");
			
			if (player)
			{
				player.transform.position = playerPos;
				Camera.main.transform.position = cameraPos;
			}
			
			bLoadingGame = false;
		}
		else
		{
			player = null;
		}
	}

	public void AddItem(Item newItem)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].itemName == newItem.itemName)
			{
				return;
			}
		}
		items.Add (newItem);
		SaveData ();
	}

	public void RemoveItem(Item selectedItem)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].itemName == selectedItem.itemName)
			{
				items.RemoveAt(i);
				SaveData();
				return;
			}
		}
	}

	public void LoadRhythm(string songName)
	{
		if (!bToRhythm)
		{
			player = GameObject.FindGameObjectWithTag("Player");

			if (player)
			{
				playerPos = player.transform.position;
				cameraPos = Camera.main.transform.position;
			}
			
			bToRhythm = true;
			songToLoad = songName;
			currentScene = Application.loadedLevelName;
			LoadingDisplay.Instance.Load ("Rhythm Section");
		}
	}

	public void FinishedLoading()
	{
		if (bToRhythm)
		{
			songToLoad = "";
			bToRhythm = false;

			if (bPractice)
			{
				currentScene = "";
			}

			RhythmGame.Instance.BeginGame();
		}
	}

	public void ReturnFromRhythm()
	{
		if (!bFromRhythm)
		{
			bFromRhythm = true;
			LoadingDisplay.Instance.Load(currentScene);
		}
	}

	public void NewData()
	{
		if (File.Exists(savePath))
		{
			File.Delete(savePath);
		}

		songToLoad = "";
		currentScene = "";
		bPractice = false;
		items.Clear ();

		bToRhythm = false;
		bFromRhythm = false;
		bLoadingGame = false;
		bDisplayTutorial = false;
		
		playerPos = Vector3.zero;
		cameraPos = Vector3.zero;
		player = null;
	}

	public void SaveData()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		if (player)
		{
			playerPos = player.transform.position;
			cameraPos = Camera.main.transform.position;
			currentScene = Application.loadedLevelName;
		}

		if (File.Exists(savePath))
		{
			Debug.Log("Overwriting player information @ \n" + savePath);
			File.Delete(savePath);
		}
		else
		{
			Debug.Log("Saving player information @\n" + savePath);
		}
		
		XmlWriter writer = new XmlTextWriter (savePath, System.Text.Encoding.UTF8);
		
		writer.WriteStartDocument();
		writer.WriteWhitespace("\n");
		writer.WriteStartElement("Root");
		writer.WriteWhitespace("\n\t");
		writer.WriteStartElement("Body");

		writer.WriteWhitespace("\n\t\t");
		writer.WriteElementString ("bDisplayTutorial", bDisplayTutorial.ToString ());
		writer.WriteWhitespace("\n\t\t");
		writer.WriteElementString ("CurrentScene", currentScene);
		writer.WriteWhitespace("\n\t\t");
		writer.WriteStartElement ("PlayerPos");
		writer.WriteAttributeString ("X", playerPos.x.ToString ());
		writer.WriteAttributeString ("Y", playerPos.y.ToString ());
		writer.WriteAttributeString ("Z", playerPos.z.ToString ());
		writer.WriteWhitespace("\n\t\t");
		writer.WriteStartElement ("CameraPos");
		writer.WriteAttributeString ("X", cameraPos.x.ToString ());
		writer.WriteAttributeString ("Y", cameraPos.y.ToString ());
		writer.WriteAttributeString ("Z", cameraPos.z.ToString ());
		writer.WriteEndElement ();

		//Save Items In Inventory
		for (int i = 0; i < items.Count; i++)
		{
			writer.WriteWhitespace("\n\t\t");
			writer.WriteStartElement ("Item");
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("ItemName", items[i].itemName.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("ItemID", items[i].itemID.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("ItemDescription", items[i].itemDescription.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("SpriteName", items[i].spriteName.ToString());
			writer.WriteWhitespace("\n\t\t");
			writer.WriteEndElement();
		}

		writer.WriteWhitespace("\n\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Close();
	}

	public void LoadData()
	{
		int setIndex = 0;
		
		if (!File.Exists(savePath)) 
		{
			Debug.Log("PlayerInfo.xml not found @ " + savePath);
			return;
		} 
		else 
		{
			Debug.Log("Loading PlayerInfo.xml @ \n" + savePath);
		}

		XmlReader reader = new XmlTextReader (savePath);
		
		items.Clear();
		
		while (reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
			{
				switch (reader.Name)
				{
					case "bDisplayTutorial":
					{
						bDisplayTutorial = bool.Parse(reader.ReadElementString());
						break;
					}
					case "CurrentScene":
					{
						currentScene = reader.ReadElementString();
						break;
					}
					case "PlayerPos":
					{
						float x = float.Parse(reader.GetAttribute(0));
						float y = float.Parse(reader.GetAttribute(1));
						float z = float.Parse(reader.GetAttribute(2));

						playerPos = new Vector3 (x, y, z);
						break;
					}
					case "CameraPos":
					{
						float x = float.Parse(reader.GetAttribute(0));
						float y = float.Parse(reader.GetAttribute(1));
						float z = float.Parse(reader.GetAttribute(2));

						cameraPos = new Vector3 (x, y, z);
						break;
					}
					case "Item":
					{
						items.Add(new Item());
						setIndex = items.Count - 1;
						break;
					}
					case "ItemName":
					{
						items[setIndex].itemName = reader.ReadElementString();
						break;
					}
					case "ItemID":
					{
						items[setIndex].itemID = int.Parse(reader.ReadElementString());
						break;
					}
					case "ItemDescription":
					{
						items[setIndex].itemDescription = reader.ReadElementString();
						break;
					}
					case "SpriteName":
					{
						items[setIndex].spriteName = reader.ReadElementString();
						break;
					}
					case "HasItem":
					{
						items[setIndex].hasItem = bool.Parse(reader.ReadElementString());
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

	public void LoadGame()
	{
		LoadData ();
		bLoadingGame = true;
		LoadingDisplay.Instance.Load (currentScene);
	}
}





