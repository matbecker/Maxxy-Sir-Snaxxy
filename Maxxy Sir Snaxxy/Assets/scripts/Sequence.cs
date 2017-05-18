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
		SpawnSequence ();
	}
	public void SpawnSequence()
	{
		if (!isPlaying)
			isPlaying = true;

		var rand = Random.Range (SequenceManager.instance.sequenceMin, SequenceManager.instance.sequenceMax);

		for (int i = 0; i < rand; i++) 
		{
			//pick a random column
			var col = Random.Range(0,Layout.instance.GetCurrentScreen().columns.Length);
			//instantiate a consumable in a radom column with an increasing y value
			var consumable = Instantiate(SequenceManager.instance.GetRandomConsumable(), new Vector3(Layout.instance.GetCurrentScreen().columns[col].position.x, Layout.instance.GetBounds().height + (i * 1.5f), 0.0f), Quaternion.identity) as Consumable; 

			sequenceConsumables.Add (consumable);
			consumable.mySequence = this;
		}
		sequenceNumber++;

		if (sequenceNumber % 5 == 0) 
		{
			//increase the difficulty every 5 waves
			SequenceManager.instance.IncreaseDifficulty ();
			return;
		} 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isPlaying) {
			return;
		}
		else{
			foreach (Consumable c in sequenceConsumables) 
			{
				var vel = c.rb.velocity;
				vel.Normalize ();
				c.rb.velocity = (vel * SequenceManager.instance.sequenceSpeed * Time.deltaTime);
			}
			if (sequenceConsumables.Count == 0) 
			{
				SpawnSequence ();
			}
		}

	}
	public void SetConsumableValues()
	{
		foreach (Consumable c in sequenceConsumables) 
		{
			switch (GameManager.instance.currentMultiplier) 
			{
			case GameManager.Multiplier.x2:
				c.value =  (c.originalValue * 2);
				break;
			case GameManager.Multiplier.x3:
				c.value = (c.originalValue * 3);
				break;
			case GameManager.Multiplier.x4:
				c.value = (c.originalValue * 4);
				break;
			case GameManager.Multiplier.x6:
				c.value = (c.originalValue * 6);
				break;
			case GameManager.Multiplier.x8:
				c.value = (c.originalValue * 8);
				break;
			case GameManager.Multiplier.x16:
				c.value = (c.originalValue * 16);
				break;
			default:
				c.value = c.originalValue;
				break;
			}
		}
	}
	public void EndSequence()
	{
		foreach (Consumable c in sequenceConsumables) {
			c.transform.DOScale (0, 1.0f).OnComplete (() => {
				Destroy(c);	
				Destroy(gameObject);
			});
		}
		sequenceConsumables.Clear ();
	}
}
