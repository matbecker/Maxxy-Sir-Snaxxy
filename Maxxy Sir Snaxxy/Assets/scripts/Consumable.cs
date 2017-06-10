using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Consumable : MonoBehaviour {

	public enum Type { Dud, Fruit, Vegetable, Treat, Cracker, Clover };
	public Type type;
	public Rigidbody rb;
	public SpriteRenderer sr;
	public Sequence mySequence;
	public BoxCollider bc;
	public int originalValue;
	public int value;
	public float pulseDuration;
	public float weight;
	public float fatness;
	public bool collected;
	public string[] gameoverMessages;

	// Use this for initialization
	void Start () 
	{
		originalValue = value;
		sr.gameObject.transform.DOScale (Vector3.one * 1.2f, pulseDuration).SetLoops (-1, LoopType.Yoyo);
	}
	void Update()
	{
		var vel = rb.velocity;
		vel.Normalize();
		vel += SequenceManager.instance.sequenceSpeed;
		rb.velocity = vel;
	}

	public void Collected()
	{
		collected = true;
		bc.enabled = false;

		transform.DOScale (Vector3.zero, 0.5f).OnComplete(() => 
		{
			//remove from consumable list
			mySequence.sequenceConsumables.Remove (this);
			//destroy gameobject
			Destroy (gameObject);
		});
	}
	public void NotCollected()
	{
		bc.enabled = false;

		transform.DOScale (Vector3.zero, 1.0f).OnComplete(() => 
		{
			mySequence.sequenceConsumables.Remove(this);
			Destroy(gameObject);
		});
	}
}
