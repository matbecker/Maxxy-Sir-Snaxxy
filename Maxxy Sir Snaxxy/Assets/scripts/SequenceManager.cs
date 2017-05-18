using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour {

	public static SequenceManager instance;
	public Consumable[] consumables;
	public List<Sequence> queuedSequences;
	public int sequenceMin;
	public int sequenceMax;
	public Sequence sequenceObj;
	public float sequenceSpeed;
	// Use this for initialization

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	void Start () 
	{
		var s = InstantiateSequence();
		queuedSequences.Add (s);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public Consumable GetRandomConsumable()
	{
		var rand = Random.Range (0, consumables.Length);

		return consumables [rand];
	}
	public void IncreaseDifficulty()
	{
		sequenceSpeed *= 1.1f;
		sequenceMin += 5;
		sequenceMax += 5;

	}
	public Sequence InstantiateSequence()
	{
		return Instantiate (sequenceObj) as Sequence;
	}
	public void Reset()
	{
		sequenceMin = 5;
		sequenceMax = 10;
		sequenceSpeed = 100;

		foreach (Sequence s in queuedSequences) {
			s.isPlaying = true;
		}
	}
	public Sequence GetCurrentSequence()
	{
		return queuedSequences [0];
	}
	public void ClearSequences()
	{
		foreach (Sequence s in queuedSequences) {
			s.EndSequence ();
			s.isPlaying = false;
		}
	}

}
