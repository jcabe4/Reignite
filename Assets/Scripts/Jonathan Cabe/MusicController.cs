/*****************************************************
 * Program: Reignite
 * Script: MusicController.cs
 * Author: Jonathan Cabe
 * Description: This script will control environment
 * music depending on which scene you are in.  For now
 * it mostly ensures music flows uninterrupted.
 * ***************************************************/

using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour 
{
	public static MusicController Instance
	{
		get
		{
			return instance;
		}
	}

	private static MusicController instance;

	void Start()
	{
		if (MusicController.Instance && MusicController.Instance != this)
		{
			Destroy(gameObject);
		}
		else if (!MusicController.Instance)
		{
			instance = this;
		}

		DontDestroyOnLoad (gameObject);
	}

	void OnLevelWasLoaded()
	{
		Debug.Log (Application.loadedLevelName);
		if (Application.loadedLevelName == "Rhythm Section")
		{
			Destroy(gameObject);
		}
	}
}
