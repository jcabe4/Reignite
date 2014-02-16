///********Reignite
// * Script: PlayerInformation.cs
// * Author: Jonathan Robinson
// * Updated: 2.12.2014
// * Description: This script holds all relevant data
// * associated with the player as well as mutator and
// * accessor functions.  This script also loads and saves
// * these values from/into an xml. Based on work from Jonathan Cabe.
// * ******************************************************/
//
//
//using UnityEngine;
//using System.IO;
//using System.Xml;
//using System.Xml.Linq;
//using System.Collections;
//using System.Collections.Generic;
//
//
//public class PlayerInformationReignite : MonoBehaviour 
//{
//	public bool hasKey = false;
//	public bool hasBook = false;
//	private Inventory inventory; 
//	private GameObject Player;  
//
//
//	public static PlayerInformationReignite Instance
//	{
//		get
//		{
//			return instance;
//		}
//	}
//
//
//	private static PlayerInformationReignite instance;
//#if UNITY_EDITOR || UNITY_PC 	
//	private string savePath = "Assets/Resources/Player Data/Info.xml";
//	private string resourcePath = "Player Data/Info";
//#endif
//
//
//	void Awake()
//	{				
//		Player = GameObject.FindGameObjectWithTag("Player");  
//		inventory = Player.GetComponent<Inventory>();  
//		Initialize();
//		instance = this;
//		DontDestroyOnLoad(gameObject);
//
//
//		if (File.Exists(savePath))
//		{
//			LoadData();
//		}
//		else
//		{			
//			SaveData();
//		}
//	}
//
//
//
//
//	public bool GetKey()
//	{
//		return hasKey;
//	}
//
//
//	public bool GetBook()
//	{
//		return hasBook;
//	}
//
//
//	public void Initialize()
//	{
//		hasKey = false;
//		hasBook = false;
//	}
//
//
//	public void SaveData()
//	{
//		Debug.Log ("Saving...");
//
//
//		if (File.Exists (savePath))
//		    {
//				File.Delete (savePath);
//			}
//		XmlWriter writer = new XmlTextWriter(savePath, System.Text.Encoding.UTF8);
//
//
//		writer.WriteStartDocument();
//		writer.WriteWhitespace("\n");
//		writer.WriteStartElement("Root");
//		writer.WriteWhitespace("\n\t");
//		writer.WriteElementString("hasKey", inventory.hasKey.ToString());
//		writer.WriteWhitespace("\n\t");
//		writer.WriteElementString("hasBook", inventory.hasBook.ToString());
//		writer.WriteEndElement();
//		writer.WriteEndDocument ();
//		writer.Close();
//
//
//			//XMLFileManager.EncryptFile(savePath);
//	}
//
//
//	public void LoadData()
//	{
//		Debug.Log("Loading...");
//		if (!File.Exists(savePath))
//		{
//			return;
//		}
//		else
//		{
//			//XMLFileManager.DecryptFile(savePath);
//		}
//
//
//		TextAsset asset = (TextAsset)Resources.Load (resourcePath);
//		XmlReader reader = XmlReader.Create (new StringReader (asset.text));
//
//
//		while (reader.Read())
//		{
//			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
//			{
//				switch(reader.Name)
//				{
//					case "hasKey": inventory.hasKey = bool.Parse(reader.ReadElementString());
//						break;
//					case "hasBook": inventory.hasBook = bool.Parse(reader.ReadElementString());
//						break;
//					default:
//						break;
//				}
//			}
//		}
//		reader.Close();
//
//
//		//XMLFileManager.EncryptFile(savePath);
//	}
//}
//
