using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Consumable : MonoBehaviour {

	public enum Type { Dud, Fruit, Vegetable, Treat, Cracker };
	public Type type;
	public Rigidbody rb;
	public SpriteRenderer sr;
	public Sequence mySequence;
	public int originalValue;
	public int value;
	public float pulseDuration;
	public float weight;
	public float fatness;
	private float timer;

	// Use this for initialization
	void Start () 
	{
		originalValue = value;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (timer > pulseDuration) 
		{
			sr.gameObject.transform.DOScale (new Vector3 (sr.transform.localScale.x * 1.2f, sr.transform.localScale.y * 1.2f, sr.transform.localScale.z), 0.1f).SetLoops (2, LoopType.Yoyo);
			timer = 0.0f;
		}
		if (transform.position.y < Layout.instance.GetBounds ().y) 
		{
			transform.DOScale (Vector3.zero, 1.0f).OnComplete(() => 
			{
					//if a good consumable has made this far my combo is over
					if (type == Type.Fruit)
					{
						Character.instance.combo = 0;
						UserInterface.instance.combo.DOColor(Color.clear, 0.5f);
						GameManager.instance.SetMultiplier();
						//increase strikes
						GameManager.instance.strikes++;
						if (GameManager.instance.CheckStrikes ()) 
							GameManager.instance.GameOver ();
					}
					mySequence.sequenceConsumables.Remove(this);
					Destroy(gameObject);
			});
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		var max = other.gameObject.GetComponentInChildren<Character> ();

		if (max != null) 
		{
			switch (type) 
			{
			case Type.Fruit:
				if (max.combo == 0)
					UserInterface.instance.combo.DOColor (Color.white, 0.5f);
				//increase score
				max.score += value;
				max.combo++;
				break;
			case Type.Vegetable:
				max.score += value;
				max.combo = 0;
				//increase strikes
				GameManager.instance.strikes++;
				if (GameManager.instance.CheckStrikes ()) 
					GameManager.instance.GameOver ();
				break;
			case Type.Cracker:
				max.combo++;
				max.currentSize -= 0.2f;
				max.Resize ();
				break;
			case Type.Treat:
				max.combo++;
				max.score += value;
				max.currentSize += fatness;
				max.Resize ();
				break;
			case Type.Dud:
				break;
			}
			UserInterface.instance.combo.text = max.combo.ToString ();
			UserInterface.instance.score.text = max.score.ToString();
			UserInterface.instance.BumpText (UserInterface.instance.combo);
			UserInterface.instance.BumpText (UserInterface.instance.score);
			GameManager.instance.SetMultiplier ();
			//remove from consumable list
			mySequence.sequenceConsumables.Remove (this);
			//destroy gameobject
			Destroy (gameObject);
		}
	}
}
