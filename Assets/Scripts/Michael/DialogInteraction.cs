/*****************************************************
 * Program: Reignite
 * Script: DialogInteraction.cs
 * Author: Michael Swedo
 * Description: The DialogInteraction class is a template
 * for how interactions handled through dialog are 
 * implemented before Jonathan Cabe finishes his dialog
 * system. This is useful as a reference and temporary
 * solution until such time as a better solution is
 * established.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class DialogInteraction : MonoBehaviour 
{
	public string sceneToGoTo;
	public string songName;
	public Item itemToAdd; // change to Item later
	public Item requiredItem; // change to Item later
	public GameObject[] affectedObjects;
	public bool isInventoryItem;
	public bool removeItemAfterUse;
	public bool removeOnComplete;
	public bool requiresItem;
	public bool isConversation;
	public int conversationIndex = -1;
	public int quoteIndex;
	public int postItemGetConversationIndex = -1;
	public int postItemGetQuoteIndex;
	
	private bool dialogComplete = false;
	
	private GameObject player;
	private float quoteOffset;
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		quoteOffset = 6f;
	}
	
	void OnEnable()
	{
		DialogueController.ConversationEnd += Load;
	}
	
	void OnDisable()
	{
		DialogueController.ConversationEnd -= Load;
		
	}
	
	void Load(int newConversationIndex)
	{
		if(newConversationIndex == conversationIndex)
		{
			if(!requiresItem)
			{
				if (sceneToGoTo == "Rhythm Section")
				{
					PlayerInformation.Instance.LoadRhythm(songName);
				}
				else if (sceneToGoTo != "")
				{
					LoadingDisplay.Instance.Load(sceneToGoTo);
				}
			}
		}

		else if(newConversationIndex == postItemGetConversationIndex)
		{
			if(requiresItem)
			{
				if (sceneToGoTo == "Rhythm Section")
				{
					PlayerInformation.Instance.LoadRhythm(songName);
				}
				else if (sceneToGoTo != "")
				{
					LoadingDisplay.Instance.Load(sceneToGoTo);
				}
			}
		}

		End();
	}
	public void BeginInteraction() 
	{
		player.GetComponent<MouseInput>().enabled = false;
		
		HandleDialog();
		if(dialogComplete || isInventoryItem)
		{
			PerformActions();
			//ChooseScene();
		}
		if(!isConversation)
		{
			End();
		}
	}
	
	public void HandleDialog()
	{
		if(!isConversation)
		{
			if(!requiresItem)
			{
				if(conversationIndex != -1)
				{
					Vector3 quotePosition = new Vector3(player.transform.position.x, player.transform.position.y + quoteOffset, player.transform.position.z);
					DialogueController.Instance.SpawnQuoteBubble(Camera.main.WorldToViewportPoint(quotePosition), conversationIndex, quoteIndex);
				}
			}
			else
			{
				if(!PlayerInformation.Instance.HasItem(requiredItem))
				{
					if(conversationIndex != -1)
					{
						Vector3 quotePosition = new Vector3(player.transform.position.x, player.transform.position.y + quoteOffset, player.transform.position.z);
						DialogueController.Instance.SpawnQuoteBubble(Camera.main.WorldToViewportPoint(quotePosition), conversationIndex, quoteIndex);
					}
				}
				else
				{
					if(postItemGetConversationIndex != -1)
					{
						Vector3 quotePosition = new Vector3(player.transform.position.x, player.transform.position.y + quoteOffset, player.transform.position.z);
						DialogueController.Instance.SpawnQuoteBubble(Camera.main.WorldToViewportPoint(quotePosition), conversationIndex, quoteIndex);
						
					}
				}
			}
		}
		else
		{
			if(!PlayerInformation.Instance.HasItem(requiredItem))
			{
				DialogueController.Instance.BeginConversation(conversationIndex);
			}
			else
			{
				DialogueController.Instance.BeginConversation(postItemGetConversationIndex);
			}
		}
	}
	
	public void PerformActions()
	{
		if(itemToAdd.itemName != "")
		{
			if (PlayerInformation.Instance)
			{
				PlayerInformation.Instance.AddItem(itemToAdd);
			}
		}
		
		if(affectedObjects.Length != 0)
		{
			// do something to them
		}
	}
	
	void End()
	{
		player.GetComponent<MouseInput>().enabled = true;
		if(removeOnComplete)
		{
			Destroy(gameObject);
		}
	}
}