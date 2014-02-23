using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour 
{
	public Transform newCameraLocation;
	public Transform newPlayerLocation;
	public GameObject curtain;
	public string requiredItem;

	private GameObject player;
	private Transform camTransform;
	private Inv inv;

	private bool fadeOut = false;
	private bool ready = false;
	private bool fadeIn = false;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		camTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		inv = player.GetComponent<Inv>();
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
		if(requiredItem == "")
		{
			fadeOut = true;
			player.GetComponent<MouseInput>().enabled = false;
		}
		else 
		{
			if(inv.items.Contains(requiredItem))
			{
				fadeOut = true;
				player.GetComponent<MouseInput>().enabled = false;
			}
		}
	}
}
