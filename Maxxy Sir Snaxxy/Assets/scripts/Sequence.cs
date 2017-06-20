using System.Collections;
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

	}
	public void SpawnSequence()
	{
		if (!GameManager.instance.gameover)
		{
			if (!isPlaying)
				isPlaying = true;
			SequenceManager.instance.SetRandomWave();

			var consumableAmount = Random.Range (SequenceManager.instance.sequenceMin, SequenceManager.instance.sequenceMax);
			var oldCol = 3;
			for (int i = 0; i < consumableAmount; i++) 
			{
				//pick a random column
				var col = Random.Range(0,Layout.instance.GetCurrentScreen().columns.Length);

				Consumable consumable = null;

				switch(Layout.instance.currentLayout)
				{
				case Layout.ScreenState.Bottom:
					//instantiate a consumable in a radom column with an increasing y value
					consumable = Instantiate(SequenceManager.instance.GetRandomConsumable(), new Vector3(Layout.instance.GetCurrentScreen().columns[col].position.x, Layout.instance.GetBounds().y + (i * 1.5f), 0.0f), Quaternion.identity) as Consumable; 
					break;
				case Layout.ScreenState.Right:
					//while (col != oldCol + 1 || col != oldCol - 1)
					//col = Random.Range(0,Layout.instance.GetCurrentScreen().columns.Length);
					//instantiate a consumable in a radom column with an decreasing x value
					consumable = Instantiate(SequenceManager.instance.GetRandomConsumable(), new Vector3(Layout.instance.GetBounds().x - (i * 1.0f), Layout.instance.GetCurrentScreen().columns[col].position.y, 0.0f), Quaternion.identity) as Consumable; 
					break;
				case Layout.ScreenState.Top:
					//instantiate a consumable in a radom column with an decreasing y value
					consumable = Instantiate(SequenceManager.instance.GetRandomConsumable(), new Vector3(Layout.instance.GetCurrentScreen().columns[col].position.x, Layout.instance.GetBounds().y - (i * 1.5f), 0.0f), Quaternion.identity) as Consumable; 
					break;
				case Layout.ScreenState.Left:
					//instantiate a consumable in a radom column with an increasing x value
					consumable = Instantiate(SequenceManager.instance.GetRandomConsumable(), new Vector3(Layout.instance.GetBounds().x + (i * 1.0f), Layout.instance.GetCurrentScreen().columns[col].position.y, 0.0f), Quaternion.identity) as Consumable; 
					break;
				}
					
			
				sequenceConsumables.Add (consumable);
				consumable.mySequence = this;

				if (i == consumableAmount - 1) //last item in the list
					consumable.lastItem = true;
				
				//save the old column
				oldCol = col;
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
		SequenceManager.instance.AdjustSequenceSpeed(0.0f);
		foreach (Consumable c in sequenceConsumables) {
			c.VoidItem();
			Destroy(c, 2.0f);	
		}
		sequenceConsumables.Clear ();
	}
}
