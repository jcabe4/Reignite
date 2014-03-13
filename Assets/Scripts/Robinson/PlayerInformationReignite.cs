/**************************************************************************
 * Inventory tool based on Jon Cabe's MusicSheetBuilder tool.            * 
 * This script saves and loads items in the game to/from Info.xml        *
 * Needs Pickup.cs in order to pickup items                              *
 * Robert R. Rojas                                                       *
 * Modified by Jonathan Robinson                                         *
 * 3.5.2014                                                              *
 *************************************************************************/

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class PlayerInformationReignite : MonoBehaviour
{
	private string fileName;
	private string savePath;
	private string inventory;
	private Vector2 scrollPos;
	private string itemName = "Item Name";
	private string itemDescription = "Item Description";
	private int itemID = 0;
	private bool bNewItem = false;
	private Item item;
	public List<Item> items = new List<Item>();

	void Awake() //Either loads game file or creates one at start.
	{
		item = GameObject.FindGameObjectWithTag ("Player").GetComponent<Item> ();
		if (File.Exists (savePath))
		{
			LoadInventory();
		}
		else
		{
			SaveInventory();
		}
	}

	public void AddItem(Item newItem) 
	{
		items.Add (newItem);
	}

	public void LoadInventory()
	{
		int setIndex = 0;
		string resourcePath = "Player Data/Info";
		savePath = "Assets/Resources/Player Data/Info.xml";

		if (!File.Exists(savePath)) 
		{
			Debug.Log("cannot find" + savePath);
			return;
		} 
		else 
		{
			Debug.Log("Loading " + savePath);
		}

		TextAsset asset = (TextAsset)Resources.Load(resourcePath);
		XmlReader reader = XmlReader.Create(new StringReader(asset.text));

		while (reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
			{
				switch (reader.Name)
				{
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
					default:
					{
						break;
					}
				}
			}
		}
	}

	public void SaveInventory() 
	{
		savePath = "Assets/Resources/Player Data/Info.xml";
		if (File.Exists (savePath)) {
			Debug.Log ("Overwriting " + savePath);
			File.Delete (savePath);
		} else {
			Debug.Log ("Saving " + savePath);
		}
		XmlWriter writer = new XmlTextWriter (savePath, System.Text.Encoding.UTF8);
		writer.WriteStartDocument ();
		writer.WriteWhitespace ("\n");
		writer.WriteStartElement ("Root");
		writer.WriteWhitespace ("\n\t");
		writer.WriteStartElement ("Body");
		for (int i = 0; i < items.Count; i++) {
			writer.WriteWhitespace ("\n\t\t");
			writer.WriteStartElement ("Item");
			writer.WriteWhitespace ("\n\t\t\t");
			writer.WriteElementString ("ItemName", items [i].itemName.ToString ());
			writer.WriteWhitespace ("\n\t\t\t");
			writer.WriteElementString ("ItemID", items [i].itemID.ToString ());
			writer.WriteWhitespace ("\n\t\t\t");
			writer.WriteElementString ("ItemDescription", items [i].itemDescription.ToString ());
			writer.WriteWhitespace ("\n\t\t");
			writer.WriteElementString ("Contains", items [i].contains.ToString ());
			writer.WriteWhitespace ("\n\t\t");
			writer.WriteEndElement ();
		}
		writer.WriteWhitespace ("\n\t");
		writer.WriteEndElement ();
		writer.WriteWhitespace ("\n");
		writer.WriteEndElement ();
		writer.WriteEndDocument ();
		writer.Close ();
	}
}