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
	public Vector3 sequenceSpeed;

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
		AdjustSequenceSpeed(0.25f);
		sequenceMin += 5;
		sequenceMax += 5;

	}

	public Sequence InstantiateSequence()
	{
		return Instantiate (sequenceObj) as Sequence;
	}

	public void AdjustSequenceSpeed(float amount)
	{
		switch (Layout.instance.currentLayout)
		{
		case Layout.ScreenState.Bottom:
			sequenceSpeed = new Vector3(0.0f,sequenceSpeed.y - amount, 0.0f);
			break;
		case Layout.ScreenState.Right:
			sequenceSpeed = new Vector3(sequenceSpeed.x + amount, 0.0f,0.0f);
			break;
		case Layout.ScreenState.Top:
			sequenceSpeed = new Vector3(0.0f,sequenceSpeed.y + amount, 0.0f);
			break;
		case Layout.ScreenState.Left:
			sequenceSpeed = new Vector3(sequenceSpeed.x - amount, 0.0f, 0.0f);
			break;
		}
	}

	public void Reset()
	{
		sequenceMin = 5;
		sequenceMax = 10;
		sequenceSpeed = new Vector3(0.0f,-1.5f,0.0f);

		foreach (Sequence s in queuedSequences) {
			s.isPlaying = true;
			s.sequenceNumber = 0;
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
