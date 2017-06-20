using System.Collections;
using System;
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
	public float fatness;
	public bool collected;
	public bool lastItem;
	public bool move;
	public string[] gameoverMessages;

	// Use this for initialization
	void Start () 
	{
		transform.DORotate(new Vector3(0.0f,0.0f, Layout.instance.GetCurrentScreen().rotation),0.0f, RotateMode.Fast);
		originalValue = value;
		sr.gameObject.transform.DOScale (Vector3.one * 1.2f, pulseDuration).SetLoops (-1, LoopType.Yoyo);

		if (GameManager.instance.gameover)
			VoidItem();
	}
	void Update()
	{
		if (!UserInterface.instance.intermission)
		{
			var vel = rb.velocity;
			vel.Normalize();
			vel += SequenceManager.instance.sequenceSpeed;
			rb.velocity = vel;
		}
	}
	public void VoidItem()
	{
		rb.constraints = RigidbodyConstraints.FreezeAll;
		NotCollected();
	}
	public void Collected()
	{
		collected = true;
		GameManager.instance.CheckForNewWave(this);

		transform.DOScale (Vector3.zero, 0.5f).OnComplete(() => 
		{
			//remove from consumable list
			mySequence.sequenceConsumables.Remove (this);

			Destroy (gameObject);
		});
		Debug.Log("you got the " + type);
	}
	public void NotCollected()
	{
		GameManager.instance.CheckForNewWave(this);
		if (bc != null)
			bc.enabled = false;

		transform.DOScale (Vector3.zero, 1.0f).OnComplete(() => 
		{
			mySequence.sequenceConsumables.Remove(this);
			Destroy(gameObject);
		});
	}
}
