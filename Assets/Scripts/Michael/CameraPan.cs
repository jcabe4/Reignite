using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour 
{
	public Transform cameraPos1;
	public Transform cameraPos2;

	public bool begin;
	private float dampTime = 0.2f;
	private Vector3 velocity = Vector3.zero;
	
	private Transform cameraTransform;

	void Start() 
	{
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
	}
	
	/*void Update() 
	{
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		if(begin)
		{
			if(cameraTransform.position == cameraPos1.position)
			{
				//cameraTransform.position = cameraPos2.position;
				Vector3.SmoothDamp(cameraTransform.position, cameraPos2.position, ref velocity, dampTime);
			}
		}
		//begin = false;
	}*/

	void OnTriggerEnter2D()
	{
		//begin = true;
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
		if(cameraTransform.position == cameraPos1.position)
		{
			cameraTransform.position = cameraPos2.position;
		}
	}
}
