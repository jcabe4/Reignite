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
	public string itemToAdd;
	public string requiredItem;
	public GameObject[] affectedObjects;
	public string[] dialog;
	public bool isInventoryItem;
	public bool removeOnComplete;
	public bool requiresItem;

	private GameObject player;
	private Inv inv;

	public void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		inv = player.GetComponent<Inv>();
	}

	public void BeginInteraction() 
	{
		player.GetComponent<MouseInput>().enabled = false;

		HandleDialog();
		PerformActions();
		ChooseScene();
		End();
	}

	void HandleDialog()
	{
		if(dialog.Length != 0)
		{
			for(int i = 0; i < dialog.Length; i++)
			{
				Debug.Log(dialog[i]);
			}
		}
	}

	void PerformActions()
	{
		if(itemToAdd != "")
		{
			inv.items.Add(itemToAdd);
		}

		if(affectedObjects.Length != 0)
		{
			// do something to them
		}
	}

	void ChooseScene()
	{
		if(sceneToGoTo != "")
		{
			Application.LoadLevel(sceneToGoTo);
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