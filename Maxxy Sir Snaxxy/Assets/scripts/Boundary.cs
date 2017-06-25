using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boundary : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		var consumable = other.gameObject.GetComponent<Consumable>();

		if (consumable != null)
		{
			var type = consumable.type;

			switch (type)
			{
			case Consumable.Type.Fruit:
				consumable.transform.DOScale(Vector3.zero, 1.0f).OnComplete(() => {
					if (!consumable.collected && !GameManager.instance.gameover) {
						GameManager.instance.Strike(consumable);
						Character.instance.DisplayInGameFailText(UserInterface.instance.inGameDropText, UserInterface.instance.inGameDropText.Length);
						consumable.NotCollected();
//						Debug.Log("you dropped the delicious " + consumable.name);
					}
				});
				break;
			case Consumable.Type.Clover: case Consumable.Type.Cracker: case Consumable.Type.Dud:
			case Consumable.Type.Treat:  case Consumable.Type.Vegetable:
				consumable.NotCollected();
				break;
			}
		}
	}
}
