using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public int index;
	public BackgroundQuadrant bq;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		var max = other.GetComponentInChildren<Character>();

		if (max != null)
		{
			if (bq.isVisible)
			{
				GameManager.instance.SetGamePlayVariables(bq.colour); //compare against the background quad
				return;
			}
			else
			{
				GameManager.instance.SetGamePlayVariables(GameManager.instance.currentColour); //compare against the whole background
			}
		}
	}
}
