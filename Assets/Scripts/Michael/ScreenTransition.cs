using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour 
{
	public Transform newCameraLocation;
	public Transform newPlayerLocation;
	public GameObject curtain;

	private GameObject player;
	private Transform camTransform;

	private bool fadeOut = false;
	private bool ready = false;
	private bool fadeIn = false;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		camTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
	}

	void Update()
	{
		if(fadeOut)
		{
			curtain.GetComponent<UIPanel>().alpha += Time.deltaTime / .5f;
			if(curtain.GetComponent<UIPanel>().alpha == 1) 
			{
				ready = true;
				fadeOut = false;
			}
		}

		if(ready)
		{
			camTransform.position = newCameraLocation.position;
			player.transform.position = newPlayerLocation.position;
			player.GetComponent<MouseInput>().futurePosition = newPlayerLocation.position;
			ready = false;
			fadeIn = true;
		}

		if(fadeIn)
		{
			curtain.GetComponent<UIPanel>().alpha -= Time.deltaTime / .5f;
			if(curtain.GetComponent<UIPanel>().alpha == 0) 
			{
				fadeIn = false;
				player.GetComponent<MouseInput>().enabled = true;

			}
		}
	}

	void OnTriggerEnter()
	{
		fadeOut = true;
		player.GetComponent<MouseInput>().enabled = false;
	}
}
