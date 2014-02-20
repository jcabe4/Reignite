using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class DialogueBuilder : EditorWindow 
{
	private class Conversation
	{
		public bool displayed = false;
		public Quote newQuote = new Quote();
		public List<Quote> quotes = new List<Quote>();

		public void SortQuotes()
		{
			int min;
			Quote temp;
			
			for (int i = 0; i < quotes.Count; i++)
			{
				min = i;
				
				for (int j = i; j < quotes.Count; j++)
				{
					if (quotes[min].quoteIndex > quotes[j].quoteIndex)
					{
						min = j;
					}
				}
				
				temp = quotes[min];
				quotes[min] = quotes[i];
				quotes[i] = temp;
			}

			for (int i = 0; i < quotes.Count; i++)
			{
				quotes[i].quoteIndex = i;
			}
		}

		public bool AddQuote(Quote quote)
		{
			bool bCanAdd = true;

			for (int i = 0; i < quotes.Count; i++)
			{
				if (quote.quoteIndex == quotes[i].quoteIndex)
				{
					bCanAdd = false;
				}
			}

			if (bCanAdd)
			{
				quotes.Add(quote);

				Debug.Log("SUCCESS");
				SortQuotes();
			}
			else
			{
				Debug.Log("FAIL");
			}
		}
	}

	private int sceneNum = 0;
	private int chapterNum = 0;
	private int selectedQuote = 0;
	private int selectedConversation = 0;
	private Vector2 scrollPos;
	private List<Conversation> conversations = new List<Conversation>();

	[MenuItem("JC Tools / Dialogue Builder")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow<DialogueBuilder> ();
	}

	void OnGUI()
	{
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		{
			EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(650));
			{
				EditorGUILayout.BeginVertical();
				{
					chapterNum = EditorGUILayout.IntField("Chapter\t\t#: ", chapterNum, GUILayout.MaxWidth(300));
					sceneNum = EditorGUILayout.IntField("Scene\t\t#: ", sceneNum, GUILayout.MaxWidth(300));

					chapterNum =  Mathf.Clamp(chapterNum, 0, int.MaxValue);
					sceneNum =  Mathf.Clamp(sceneNum, 0, int.MaxValue);

					EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
					{
						if(GUILayout.Button("Load File"))
						{
						}

						if(GUILayout.Button("Save File"))
						{
						}
					}
					EditorGUILayout.EndHorizontal();

					if(GUILayout.Button("Add Conversation", GUILayout.MaxWidth(300)))
					{
						conversations.Add(new Conversation());
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

		Debug.Log("Selected: " + selectedQuote.ToString());
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
}
