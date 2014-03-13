/*****************************************************
 * Program: Reignite
 * Script: LoadingDisplay.cs
 * Author: Jonathan Cabe
 * Description: This script operates the loading of
 * other scenes in collaboration with an actual NGUI
 * UI in scene.  This will persist through every scene.
 * When loading is called, it will fade to a specified
 * loading window and then when the next scene is
 * loaded, it will fade into the scene.
 * ***************************************************/
using UnityEngine;
using System.Collections;

public class LoadingDisplay : MonoBehaviour 
{
	public UILabel title;
	public UIPanel window;
	public UIPanel fader;
	public UISlider loadingBar;
	public string nextScene;

	public static LoadingDisplay Instance
	{
		get
		{
			return instance;
		}
	}

	private float timer = 0f;
	private float speed = 7.5f;
	private float interval = 0.5f;
	private float displayTime = 1f;
	private	float currentTime = 0f;
	private	float initialTime = 0f;
	private bool fade = true;
	private bool loading = false;
	private bool transition = false;
	
	private AsyncOperation async;
	private static LoadingDisplay instance;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		
		fader.alpha = 0f;
		window.alpha = 0f;
		instance = this;

		Load (nextScene);
	}
	
	IEnumerator ChangeScenes()
	{
		currentTime = 0f;
		initialTime = Time.realtimeSinceStartup;
		
		while (transition)
		{
			if (fade && fader.alpha < 1f)
			{
				currentTime = (Time.realtimeSinceStartup - initialTime) * speed;
				fader.alpha = currentTime;
			}
			else if (fade && fader.alpha >= 1f)
			{
				timer = (Time.realtimeSinceStartup - initialTime) * speed;
				
				if (Application.HasProLicense() && timer >= displayTime)
				{
					if (!loading)
					{
						window.alpha = 1f;
						LoadScene();
					}
					
					if (async != null)
					{
						loadingBar.value = (async.progress * 100f) / 100f;
					}
					else
					{
						loadingBar.value = 0f;
					}
					
					if (timer < displayTime + (1f * interval))
					{
						title.text = "LOADING";
					}
					else if (timer < displayTime + (2f * interval))
					{
						title.text = "LOADING.";
					}
					else if (timer < displayTime + (3f * interval))
					{
						title.text = "LOADING..";
					}
					else if (timer < displayTime + (4f * interval))
					{
						title.text = "LOADING...";
					}
					else
					{
						title.text = "LOADING";
						timer = displayTime;
					}
				}
				else if (!Application.HasProLicense())
				{
					if (!loading)
					{
						LoadScene();
					}
				}
			}
			else if (!fade && fader.alpha > 0f)
			{
				fader.alpha = 1 - (Time.realtimeSinceStartup - initialTime) * speed * 1.1f;
			}
			else if (!fade && fader.alpha <= 0f)
			{
				fade = true;
				loading = false;
				transition = false;
				fader.alpha = 0f;
				window.alpha = 0f;
			}
			
			yield return 0;
		}
	}
	
	void OnLevelWasLoaded()
	{
		timer = 0f;
		fade = false;
		window.alpha = 0f;
		currentTime = 0f;
		initialTime = Time.realtimeSinceStartup;
	}
	
	void LoadScene()
	{
		loading = true;
		
		if (Application.HasProLicense())
		{
			async = Application.LoadLevelAsync(nextScene);
		}
		else
		{
			Application.LoadLevel(nextScene);
		}
	}
	
	public void Load(string scene)
	{
		Debug.Log ("Loading: " + scene);
		timer = 0f;
		fade = true;
		nextScene = scene;
		transition = true;
		StartCoroutine("ChangeScenes");
	}
}
