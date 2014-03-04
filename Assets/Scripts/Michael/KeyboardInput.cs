/*****************************************************
 * Program: Reignite
 * Script: Michael Swedo.cs
 * Author: Michael Swedo, reference from Jonathan Cabe's RhythmInput script
 * Description: This script handles keyboard input,
 * primarily UI component handling and visibility.
 * ***************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class KeyboardInput : MonoBehaviour 
{
	public static Action<KeyCode> KeyPress;

	public KeyCode options;
	public KeyCode confirm;
	public UIPanel optionsPanel;
	
	public static KeyboardInput Instance
	{
		get
		{
			return instance;
		}
	}
	private int keys;
	private bool bLiftUp = false;

	private static KeyboardInput instance;
	
	public List<KeyCode> otherKeys = new List<KeyCode>();
	
	void Start()
	{
		keys = System.Enum.GetNames(typeof(KeyCode)).Length;
		instance = this;
	}
	
	void OnEnable()
	{
		KeyPress += CheckKeyDown;
		KeyPress += KeyCommand;
	}
	
	void OnDisable()
	{
		KeyPress -= CheckKeyDown;
		KeyPress -= KeyCommand;
	}
	
	void Update()
	{
		if (Input.anyKeyDown)
		{
			KeyPressed();
		}
		
		if (!Input.anyKey)
		{
			if (otherKeys.Count > 0)
			{
				otherKeys.Clear();
			}
			
			bLiftUp = true;
		}
		
		CheckKeyUp();
	}
	
	public bool GetLiftUp()
	{
		return bLiftUp;
	}
	
	private void KeyPressed()
	{
		for(int i = 0; i < keys; i++)
		{
			if (Input.GetKey((KeyCode)i))
			{
				if (KeyPress != null)
				{
					KeyPress((KeyCode)i);
				}
			}
		}
	}
	
	private void CheckKeyUp()
	{
		for (int i = 0; i < otherKeys.Count; i++)
		{
			if (Input.GetKeyUp(otherKeys[i]))
			{
				otherKeys.RemoveAt(i);
				break;
			}
		}
	}
	
	private void CheckKeyDown(KeyCode key)
	{
		bLiftUp = false;

		otherKeys.Add(key);

		if (!otherKeys.Contains(key) )
		{
			otherKeys.Add(key);
		}
	}

	private void KeyCommand(KeyCode key)
	{
		if (options == key && optionsPanel.alpha == 0f)
		{
			optionsPanel.alpha = 1f;
			gameObject.GetComponent<MouseInput>().enabled = false;
		}
		else if (options == key && optionsPanel.alpha != 0f)
		{
			optionsPanel.alpha = 0f;
			gameObject.GetComponent<MouseInput>().enabled = true;
		}
	}
}
