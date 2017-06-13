﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sequence : MonoBehaviour {

	public List<Consumable> sequenceConsumables;
	public int sequenceNumber = 0;
	public bool isPlaying;

	// Use this for initialization
	void Start () 
	{
		//SpawnSequence ();
	}
	public void SpawnSequence()
	{
		if (!isPlaying)
			isPlaying = true;

		var consumableAmount = Random.Range (SequenceManager.instance.sequenceMin, SequenceManager.instance.sequenceMax);

		for (int i = 0; i < consumableAmount; i++) 
		{
			//pick a random column
			var col = Random.Range(0,Layout.instance.GetCurrentScreen().columns.Length);
			//instantiate a consumable in a radom column with an increasing y value
			var consumable = Instantiate(SequenceManager.instance.GetRandomConsumable(), new Vector3(Layout.instance.GetCurrentScreen().columns[col].position.x, Layout.instance.GetBounds().height + (i * 1.5f), 0.0f), Quaternion.identity) as Consumable; 

			sequenceConsumables.Add (consumable);
			consumable.mySequence = this;
		}
		if (sequenceNumber > 0)
		{
			var rand = Random.value;
			if (rand > 0.5f)
				GameManager.instance.SetBackgroundImages();
		}
		sequenceNumber++;
		UserInterface.instance.waveInt++;
		if (sequenceNumber % 5 == 0) 
		{
			//increase the difficulty every 5 waves
			SequenceManager.instance.IncreaseDifficulty ();
			return;
		} 
	}

	public void SetConsumableValues(GameManager.Multiplier m)
	{
		foreach (Consumable c in sequenceConsumables) 
		{
			switch (m) 
			{
			case GameManager.Multiplier.None:
				c.value = c.originalValue;
				break;
			case GameManager.Multiplier.x2:
				c.value =  (c.originalValue * 2);
				break;
			case GameManager.Multiplier.x3:
				c.value = (c.originalValue * 3);
				break;
			case GameManager.Multiplier.x4:
				c.value = (c.originalValue * 4);
				break;
			case GameManager.Multiplier.x5:
				c.value = (c.originalValue * 5);
				break;
			case GameManager.Multiplier.x6:
				c.value = (c.originalValue * 6);
				break;
			case GameManager.Multiplier.x8:
				c.value = (c.originalValue * 8);
				break;
			case GameManager.Multiplier.x10:
				c.value = (c.originalValue * 10);
				break;
			}
		}
	}
	public void EndSequence()
	{
		SequenceManager.instance.AdjustSequenceSpeed(10.0f);
		foreach (Consumable c in sequenceConsumables) {
			c.GetComponent<BoxCollider>().enabled = false;
			Destroy(c, 2.0f);	
		}
		sequenceConsumables.Clear ();
	}
}
