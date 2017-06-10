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
			var type = consumable.type;

			switch (type)
			{
			case Consumable.Type.Vegetable:
				consumable.NotCollected();
				break;
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		var consumable = other.gameObject.GetComponent<Consumable>();

		if (consumable != null)
		{
			var type = consumable.type;

			switch (type)
			{
			case Consumable.Type.Fruit:
				GameManager.instance.Strike(consumable);
				Character.instance.DisplayInGameFailText(UserInterface.instance.inGameDropText, UserInterface.instance.inGameDropText.Length);
				//TODO only give the player strike if the fruit hits the bottom of the collider
				Debug.Log("you dropped the delicious " + consumable.name);
				break;
			}
			consumable.NotCollected();
		}
	}
}
