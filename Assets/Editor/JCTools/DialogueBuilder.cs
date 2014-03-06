using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class DialogueBuilder : EditorWindow 
{
	private int sceneNum = 0;
	private int chapterNum = 0;
	private int selectedQuote = 0;
	private int selectedConversation = 0;

	private string fileName;
	private string savePath;
	private string resourcePath;
	private Vector2 scrollPos;
	private List<Conversation> conversations = new List<Conversation>();

	[MenuItem("JC Tools / Dialogue Builder")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow<DialogueBuilder> ().maxSize = new Vector2(600f, 600f);
		EditorWindow.GetWindow<DialogueBuilder> ().minSize = new Vector2(600f, 300f);
	}

	void OnGUI()
	{
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		{
			EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(600f));
			{
				EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
				{
					chapterNum = EditorGUILayout.IntField("Chapter\t\t#: ", chapterNum, GUILayout.MaxWidth(300));
					sceneNum = EditorGUILayout.IntField("Scene\t\t#: ", sceneNum, GUILayout.MaxWidth(300));

					chapterNum =  Mathf.Clamp(chapterNum, 0, int.MaxValue);
					sceneNum =  Mathf.Clamp(sceneNum, 0, int.MaxValue);

					EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
					{
						if(GUILayout.Button("Load File"))
						{
							LoadData();
						}

						if(GUILayout.Button("Save File"))
						{
							SaveData();
						}
					}
					EditorGUILayout.EndHorizontal();

					if(GUILayout.Button("Add Conversation", GUILayout.MaxWidth(300)))
					{
						conversations.Add(new Conversation());
					}

					if (GUILayout.Button("Reset Everything"))
					{
						sceneNum = 0;
						chapterNum = 0;
						selectedQuote = 0;
						selectedConversation = 0;
						conversations.Clear();

						EditorWindow.GetWindow<DialogueBuilder>().position = new Rect(position.x, position.y, 650f, 300f);
					}

					for (int i = 0; i < conversations.Count; i++)
					{
						EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
						{
							conversations[i].displayed = EditorGUILayout.BeginToggleGroup("Conversation " + (i + 1).ToString(), conversations[i].displayed);
							{
								if (conversations[i].displayed)
								{
									SetConvoFocus(i);

									if (GUILayout.Button("Remove"))
									{
										conversations.RemoveAt(i);
									}
								}
							}
							EditorGUILayout.EndToggleGroup();
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.Space();
						EditorGUILayout.Space();
					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
				{
					if (selectedConversation < conversations.Count && conversations[selectedConversation].displayed)
					{
						if (CheckEditQuote() && selectedQuote < conversations[selectedConversation].quotes.Count)
						{
							conversations[selectedConversation].quotes[selectedQuote].quoteIndex = EditorGUILayout.IntField("Quote Index:", conversations[selectedConversation].quotes[selectedQuote].quoteIndex, GUILayout.MaxWidth(300));
							conversations[selectedConversation].quotes[selectedQuote].speaker = EditorGUILayout.TextField("Speaker:", conversations[selectedConversation].quotes[selectedQuote].speaker, GUILayout.MaxWidth(300));
							conversations[selectedConversation].quotes[selectedQuote].statement = EditorGUILayout.TextArea(conversations[selectedConversation].quotes[selectedQuote].statement, GUILayout.MinWidth(100), GUILayout.MaxWidth(300), GUILayout.MinHeight(100));
						
							EditorGUILayout.BeginHorizontal();
							{
								if (GUILayout.Button("Clear Quote"))
								{
									if (CheckEditQuote())
									{
										conversations[selectedConversation].quotes[selectedQuote].speaker = "";
										conversations[selectedConversation].quotes[selectedQuote].statement = "";

										GUI.FocusControl("");
									}
								}

								if (GUILayout.Button("Delete Quote"))
								{
									if (CheckEditQuote())
									{
										conversations[selectedConversation].quotes.RemoveAt(selectedQuote);
										conversations[selectedConversation].SortQuotes();
										selectedQuote = 0;

										GUI.FocusControl("");
									}
								}
							}
							EditorGUILayout.EndHorizontal();
						}
						else
						{
							while (conversations[selectedConversation].newQuote.quoteIndex < conversations[selectedConversation].quotes.Count)
							{
								conversations[selectedConversation].newQuote.quoteIndex++;
							}

							conversations[selectedConversation].newQuote.quoteIndex = EditorGUILayout.IntField("Quote Index:", conversations[selectedConversation].newQuote.quoteIndex, GUILayout.MaxWidth(300));
							conversations[selectedConversation].newQuote.speaker = EditorGUILayout.TextField("Speaker:", conversations[selectedConversation].newQuote.speaker, GUILayout.MaxWidth(300));
							conversations[selectedConversation].newQuote.statement = EditorGUILayout.TextArea(conversations[selectedConversation].newQuote.statement, GUILayout.MinWidth(100), GUILayout.MaxWidth(300), GUILayout.MinHeight(100));
						

							EditorGUILayout.BeginHorizontal();
							{
								if (GUILayout.Button("Clear Quote"))
								{
									conversations[selectedConversation].newQuote.speaker = "";
									conversations[selectedConversation].newQuote.statement = "";

									GUI.FocusControl("");
								}

								if (GUILayout.Button("Add Quote"))
								{
									Quote addedQuote = conversations[selectedConversation].newQuote;

									if(conversations[selectedConversation].AddQuote(addedQuote))
									{
										conversations[selectedConversation].newQuote = new Quote();
									}

									GUI.FocusControl("");
								}
							}
							EditorGUILayout.EndHorizontal();
						}

						if (GUILayout.Button("Sort Quotes"))
						{
							conversations[selectedConversation].SortQuotes();
						}

						for (int i = 0; i < conversations[selectedConversation].quotes.Count; i++)
						{
							conversations[selectedConversation].quotes[i].bDisplayed = EditorGUILayout.ToggleLeft("Quote " + i.ToString(), conversations[selectedConversation].quotes[i].bDisplayed);
							
							if (conversations[selectedConversation].quotes[i].bDisplayed)
							{
								SetQuoteFocus(i);
							}
						}
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();

		GUI.SetNextControlName("");
	}

	private void SetConvoFocus(int index)
	{
		selectedConversation = index;

		for (int i = 0; i < conversations.Count; i++)
		{
			if (i != index)
			{
				conversations[i].displayed = false;
			}
		}
	}

	private void SetQuoteFocus(int index)
	{
		selectedQuote = index;

		for (int i = 0; i < conversations[selectedConversation].quotes.Count; i++)
		{
			if (i != index)
			{
				conversations[selectedConversation].quotes[i].bDisplayed = false;
			}
		}
	}

	private bool CheckEditQuote()
	{
		for (int i = 0; i < conversations[selectedConversation].quotes.Count; i++)
		{
			if (conversations[selectedConversation].quotes[i].bDisplayed)
			{
				return true;
			}
		}

		return false;
	}

	private void LoadData()
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

	private void SaveData()
	{
		fileName = "Dialogue (Ch " + chapterNum.ToString () + ", Scene " + sceneNum.ToString () + ")";
		savePath = Application.dataPath + "/Resources/Dialogue Data/" + fileName + ".xml";

		if (File.Exists(savePath))
		{
			Debug.Log("Overwriting " + fileName + ".xml @ \n" + savePath);
			File.Delete(savePath);
		}
		else
		{
			Debug.Log("Saving " + fileName + ".xml @ \n" + savePath);
		}
		
		XmlWriter writer = new XmlTextWriter (savePath, System.Text.Encoding.UTF8);

		writer.WriteStartDocument();
		writer.WriteWhitespace ("\n");
		writer.WriteStartElement ("Root");
		writer.WriteWhitespace ("\n\t");
		writer.WriteStartElement ("Body");

		for (int i = 0; i < conversations.Count; i++)
		{
			writer.WriteWhitespace("\n\t\t");
			writer.WriteStartElement("Conversation");
			writer.WriteAttributeString("index", i.ToString());

			for (int j = 0; j < conversations[i].quotes.Count; j++)
			{
				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteStartElement("Quote");
				writer.WriteAttributeString("quoteIndex", conversations[i].quotes[j].quoteIndex.ToString());
				writer.WriteAttributeString("speaker", conversations[i].quotes[j].speaker);
				writer.WriteAttributeString("statement", conversations[i].quotes[j].statement);

				writer.WriteWhitespace("\n\t\t\t");
				writer.WriteEndElement();
			}
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
