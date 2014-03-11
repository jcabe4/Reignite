﻿/*****************************************************
 * Program: Reignite
 * Script: DialogueController.cs
 * Author: Jonathan Cabe
 * Description: This script that handles the flow of
 * dialogue in scene.  By calling its functions,
 * dialogue can be loaded and show via a full dialogue
 * window or just a dialogue bubble.
 * ***************************************************/

using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour 
{
	public static DialogueController Instance
	{
		get
		{
			return instance;
		}
	}

	public Vector2 defaultDialogue;
	public Object quoteBubble;
	public GameObject quoteParent;
	public UIPanel conversationPanel;
	public UILabel speakerLabel;
	public UILabel statementLabel;

	private int currentQuoteIndex = 0;
	private int charactersDisplayed = 0;
	private int currentConversationIndex = 0;
	private float timer = 0f;
	private float textSpeed = 5f;
	private float speedResetValue = 0f;
	private bool bDisplayText = false;
	private bool bTextFinished = false;

	private string fileName;
	private string savePath;
	private string resourcePath;

	private Quote currentQuote = new Quote();
	private static DialogueController instance;
	private List<Conversation> conversations = new List<Conversation>();

	void Start()
	{
		instance = this;
		conversationPanel.alpha = 0f;
		LoadFile(Mathf.FloorToInt(defaultDialogue.x), Mathf.FloorToInt(defaultDialogue.y));
	}

	void Update()
	{
		if (bDisplayText)
		{
			timer += Time.deltaTime * textSpeed;
			charactersDisplayed = Mathf.FloorToInt(timer);

			ShowStatement(charactersDisplayed);
		}
	}

	void SpawnQuoteBubble(Vector3 pos, int conversationIndex, int quoteIndex)
	{
		GameObject newBubble = (GameObject)Instantiate(quoteBubble);
		newBubble.transform.parent = quoteParent.transform;
		newBubble.transform.localScale = Vector3.one;
		newBubble.name = "Bubble Quote (" + conversationIndex.ToString () + ", " + quoteIndex.ToString () + ")";

		UILabel quoteLabel = newBubble.GetComponentInChildren<UILabel>();
		UIAnchor anchor = newBubble.GetComponent<UIAnchor>();

		if (quoteLabel && anchor)
		{
			if (conversationIndex < conversations.Count)
			{
				if (quoteIndex < conversations[conversationIndex].quotes.Count)
				{
					anchor.relativeOffset = pos;
					quoteLabel.text = conversations[conversationIndex].quotes[quoteIndex].statement;
				}
			}
		}
	}

	void BeginConversation(int index)
	{
		if (index < conversations.Count)
		{
			bDisplayText = true;
			bTextFinished = false;
			currentQuoteIndex = 0;
			conversationPanel.alpha = 1f;
			currentConversationIndex = index;
			speedResetValue = textSpeed;

			if (currentQuoteIndex < conversations[index].quotes.Count)
			{
				currentQuote = conversations[currentConversationIndex].quotes[currentQuoteIndex];
				ShowStatement(0);
			}
		}
	}

	void NextQuote()
	{
		if (bTextFinished)
		{
			currentQuoteIndex++;

			if (currentQuoteIndex < conversations[currentConversationIndex].quotes.Count)
			{
				timer = 0f;
				bDisplayText = true;
				bTextFinished = false;
				textSpeed = speedResetValue;
				currentQuote = conversations[currentConversationIndex].quotes[currentQuoteIndex];
			}
			else
			{
				EndConversation();
			}
		}
		else if (bDisplayText)
		{
			textSpeed *= 1.5f;
		}
	}

	void EndConversation()
	{
		timer = 0f;
		bDisplayText = false;
		bTextFinished = false;
		currentQuoteIndex = 0;
		speakerLabel.text = "";
		statementLabel.text = "";
		conversationPanel.alpha = 0f;
		currentConversationIndex = 0;
		textSpeed = speedResetValue;
		currentQuote = null;
	}

	void ShowStatement(int numOfCharacters)
	{
		if (currentQuote != null)
		{
			if (numOfCharacters <= currentQuote.statement.Length)
			{
				speakerLabel.text = currentQuote.speaker;
				statementLabel.text = currentQuote.statement.Substring(0, numOfCharacters);
			}
			else
			{
				bDisplayText = false;
				bTextFinished = true;
				speakerLabel.text = currentQuote.speaker;
				statementLabel.text = currentQuote.statement;
			}
		}
	}

	void LoadFile(int chapterNum, int sceneNum)
	{
		fileName = "Dialogue (Ch " + chapterNum.ToString () + ", Scene " + sceneNum.ToString () + ")";
		savePath = Application.dataPath + "/Resources/Dialogue Data/" + fileName + ".xml";
		resourcePath = "Dialogue Data/" + fileName;
		int temp = 0;
		
		if (!File.Exists(savePath))
		{
			Debug.Log(fileName + ".xml Not Found @ \n" + savePath);
			return;
		}
		else
		{
			Debug.Log("Loading " + fileName + ".xml @ \n" + savePath);
		}
		
		TextAsset asset = (TextAsset)Resources.Load(resourcePath);
		XmlReader reader = XmlReader.Create(new StringReader(asset.text));
		
		conversations.Clear();
		
		while(reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element)
			{
				switch(reader.Name)
				{
				case "Conversation":
				{
					temp = int.Parse(reader.GetAttribute("index"));
					
					if (conversations.Count == temp)
					{
						Conversation newConversation = new Conversation();
						conversations.Add(newConversation);
					}
					break;
				}
				case "Quote":
				{
					if (conversations.Count > 0)
					{
						Quote newQuote = new Quote();
						
						newQuote.quoteIndex = int.Parse(reader.GetAttribute("quoteIndex"));
						newQuote.speaker = reader.GetAttribute("speaker");
						newQuote.statement = reader.GetAttribute("statement");
						
						conversations[conversations.Count - 1].AddQuote(newQuote);
					}
					break;
				}
				default:
				{
					break;
				}
				}
			}
		}
		
		reader.Close();
	}
}
