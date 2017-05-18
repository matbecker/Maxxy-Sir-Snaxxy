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
			transform.DOScale (Vector3.zero, 0.5f).OnComplete(() => 
			{
				//remove from consumable list
				mySequence.sequenceConsumables.Remove (this);
				//destroy gameobject
				Destroy (gameObject);
			});
		}
	}
}
