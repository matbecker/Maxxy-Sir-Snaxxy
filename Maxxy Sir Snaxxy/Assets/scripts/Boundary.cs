using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boundary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter(Collider other)
	{
		var consumable = other.gameObject.GetComponent<Consumable>();

		if (consumable != null)
		{
			if (consumable.type == Consumable.Type.Fruit)
			{
				GameManager.instance.Strike();
			}
		}
	}
}
