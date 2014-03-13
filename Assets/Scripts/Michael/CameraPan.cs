using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour 
{
	public Transform target;
	public float transitionDuration = 1.5f;

	private Transform cameraTransform;

	void Start() 
	{
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
	}
	
	void OnTriggerEnter2D()
	{
		//cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

		StartCoroutine(Transition());
	}

	IEnumerator Transition()
	{
		float t = 0.0f;
		Vector3 startingPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().position;
		while (t < 1.0f)
		{
			t += Time.deltaTime * (Time.timeScale / transitionDuration);
			
			cameraTransform.position = Vector3.Lerp(startingPos, target.position, t);
			yield return 0;
		}
	}
}
