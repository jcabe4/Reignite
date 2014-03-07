using UnityEngine;
using System.Collections;

public class TutorialPanels : MonoBehaviour 
{
	public UIPanel[] panels;
	public bool showTutorial;
	
	public static TutorialPanels Instance
	{
		get
		{
			return instance;
		}
	}

	private int panelIndex = 0;
	
	private static TutorialPanels instance;
	private RhythmGame rhythm;

	void Start()
	{
		instance = this;
		rhythm = GameObject.FindGameObjectWithTag("GameController").GetComponent<RhythmGame>();

		if(rhythm.song == "WarmUp")
		{
			showTutorial = true;
		}
		else 
		{
			showTutorial = false;
		}

		if(!showTutorial)
		{
			for(int i = 0; i < panels.Length; i++)
			{
				panels[i].alpha = 0f;
			}

			rhythm.BeginGame();

			Destroy(gameObject);
		}
		else
		{
			DisplayPanel(0);
		}
	}

	void DisplayPanel (int panelIndex)
	{
		if(showTutorial)
		{
			for(int i = 0; i < panels.Length; i++) 
			{

				if(i == panelIndex) 
				{
					panels[i].alpha = 1f;
				}
				else
				{
					panels[i].alpha = 0f;
				}
			}

		}
	}

	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Space))
		{
			nextPanel();
		}
		if(Input.GetKeyDown (KeyCode.Return))
		{
			for(int i = 0; i < panels.Length; i++) 
			{
				
				if(i == panelIndex) 
				{
					panels[i].alpha = 0f;
				}
			}
			rhythm.BeginGame();
			Destroy(gameObject);
		}
	}

	public void nextPanel()
	{
		if(panelIndex < panels.Length) 
		{
			panelIndex++;
			DisplayPanel (panelIndex);
		}
		else
		{
			panels[panelIndex - 1].alpha = 0f;
			rhythm.BeginGame();
		}

	}
}
