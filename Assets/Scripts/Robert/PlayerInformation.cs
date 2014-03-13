/**************************************************************************
 * Player Data Script based on based on Jon Cabe's MusicSheetBuilder tool *
 * & Jon Robinson's Player Information/Item Pickup scripts.               *
 * Will be edited in the future to correspond with Item.cs                *
 *                                                                        *
 * Robert R. Rojas                                                        *
 *************************************************************************/

using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerInformation : MonoBehaviour
{
	private string savePath;
	private Vector3 playerPos;
	private Vector3 cameraPos;
	private GameObject player;
	private GameObject npc;
	private Camera camera;
	private Item item;
	//private string songName;
	//private int score;
	//private bool hasItem = false;
	//private bool questComplete = false;

	public List<string> songs = new List<string>();
	public List<int> scores = new List<int>();
	public List<Item> items = new List<Item>();
	public List<string> scenes = new List<string>();
	
	public static PlayerInformation Instance
	{
		get
		{
			return instance;
		}
	}
	
	private static PlayerInformation instance;

	public void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		npc = GameObject.FindGameObjectWithTag("NPC");
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		item = GameObject.FindGameObjectWithTag("Player").GetComponent<Item>(); 

		if (File.Exists(savePath))
		{
			LoadPI();
		}
		else
		{
			SavePI();
		}
	}

	public void Update()
	{
		int index;

//		if (Application.LoadLevel)
//		{

			if (Application.loadedLevelName != "Rhythm Section")
			{
				index = scenes.Count;
				scenes[index] = Application.loadedLevelName;
			}
			else if (Application.loadedLevelName == "Rhythm Section")
			{
				index = songs.Count;
				songs[index] = RhythmGame.Instance.song;
				scores[index] = RhythmGame.Instance.score;
			}
			for (index = 0; index < items.Count; index++)
			{
				if (items[index].hasItem == true)
				{
					Destroy(items[index].gameObject);
				}
			}
//		}
	}

	/*public void AddItem(Item newItem)
	{
		items.Add(newItem);
	}*/

	public void LoadPI()
	{
		int sceneIndex = 0;
		int songIndex = 0;
		int itemIndex = 0;
		string resourcePath = "Player Data/PlayerInfo.xml";
		savePath = "Assets/Resources/Player Data/PlayerInfo.xml";

		if (!File.Exists(savePath)) 
		{
			Debug.Log("PlayerInfo.xml not found @ " + savePath);
			return;
		} 
		else 
		{
			Debug.Log("Loading PlayerInfo.xml @ \n" + savePath);
		}

		TextAsset asset = (TextAsset)Resources.Load(resourcePath);
		XmlReader reader = XmlReader.Create(new StringReader(asset.text));

		items.Clear(); //needed?

		while (reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
			{
				switch (reader.Name)
				{
				case "Scene":
					sceneIndex = scenes.Count - 1;
					break;
				case "SceneName":
					scenes[sceneIndex] = reader.ReadElementString();
					break;
				case "PlayerPosX":
					playerPos.x = float.Parse(reader.ReadElementString());
					break;
				case "PlayerPosY":
					playerPos.y = float.Parse(reader.ReadElementString());
					break;
				case "PlayerPosZ":
					playerPos.z = float.Parse(reader.ReadElementString());
					break;
				case "CameraPosX":
					cameraPos.x = float.Parse(reader.ReadElementString());
					break;
				case "CameraPosY":
					cameraPos.y = float.Parse(reader.ReadElementString());
					break;
				case "CameraPosZ":
					cameraPos.z = float.Parse(reader.ReadElementString());
					break;

				case "Song":
					songIndex = songs.Count - 1;
					break;
				case "SongName":
					songs[songIndex] = reader.ReadElementString();
					break;
				case "HighScore":
					scores[songIndex] = int.Parse(reader.ReadElementString());
					break;

				case "Item":
					items.Add(new Item());
					itemIndex = items.Count - 1;
					break;
				case "ItemName":
					items[itemIndex].itemName = reader.ReadElementString();
					break;
				case "ItemID":
					items[itemIndex].itemID = int.Parse(reader.ReadElementString());
					break;
				case "ItemDescription":
					items[itemIndex].itemDescription = reader.ReadElementString();
					break;
				case "SpriteName":
					items[itemIndex].spriteName = reader.ReadElementString();
					break;
				case "ItemSprite":
					items[itemIndex].itemSprite.spriteName = reader.ReadElementString();
					break;
				case "HasItem":
					items[itemIndex].hasItem = bool.Parse(reader.ReadElementString());
					break;
				case "QuestComplete":
					items[itemIndex].questComplete = bool.Parse (reader.ReadElementString());
					break;
				default:
					break;
				}
			}
		}
	}

	public void SavePI()
	{
		int i;
		savePath = "Assets/Resources/Player Data/PlayerInfo.xml";
		playerPos = player.transform.position;
		cameraPos = camera.transform.position;

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

		//save player and camera position
		for (i = 0; i < songs.Count; i++)
		{
			writer.WriteWhitespace("\n\t\t");
			writer.WriteStartElement("Scene");
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteStartElement("SceneName", Application.loadedLevelName.ToString ());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("PlayerPosX", playerPos.x.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("PlayerPosY", playerPos.y.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("PlayerPosZ", playerPos.z.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("CameraPosX", cameraPos.x.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("CameraPosY", cameraPos.y.ToString());
			writer.WriteWhitespace("\n\t\t\t\t");
			writer.WriteElementString("CameraPosZ", cameraPos.z.ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteEndElement();
			writer.WriteWhitespace("\n\t\t");
			writer.WriteEndElement();
		}

		//save song scores
		for (i = 0; i < songs.Count; i++)
		{
			writer.WriteWhitespace("\n\t\t");
			writer.WriteStartElement ("Song");
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("SongName", songs[i].ToString());
			writer.WriteWhitespace("\n\t\t\t");
			writer.WriteElementString("HighScore", scores[i].ToString());
			//writer.WriteWhitespace("\n\t\t\t");
			//writer.WriteElementString("SongCompletion", .ToString());
			writer.WriteWhitespace("\n\t\t");
		}

		for (i = 0; i < items.Count; i++)
		{
			if (items[i].hasItem == true) //&& items[i].questComplete == false
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
				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteElementString("ItemSprite", items[i].itemSprite.ToString());
				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteElementString("HasItem", items[i].itemSprite.ToString());
				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteElementString("QuestComplete", items[i].itemSprite.ToString());
				writer.WriteWhitespace("\n\t\t");
				writer.WriteEndElement();
			}
			//story progression??
			else if (items[i].hasItem == false && items[i].questComplete == true)
			{
				writer.WriteWhitespace("\n\t\t");
				writer.WriteStartElement ("Item");
				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteElementString("HasItem", items[i].hasItem.ToString());
				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteElementString("QuestComplete", items[i].questComplete.ToString());
				writer.WriteWhitespace("\n\t\t");
				writer.WriteEndElement();
			}
		}

		writer.WriteWhitespace("\n\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Close();
	}
}





