using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SequenceManager : MonoBehaviour {

	[System.Serializable]
	public class Wave 
	{
		public ConsumableWeights[] consumables;
		public int weight;
		private int totalWeight;

		public void CalcTotalWeight() 
		{
			totalWeight = consumables.Sum(cw => cw.weight);
		}

		public Consumable GetRandomConsumable()
		{
			var rand = Random.Range (0, totalWeight);

			var weight = 0;
			for(int i = 0; i < consumables.Length; i++) 
			{
				var cw = consumables[i];
				weight += cw.weight;
				if(weight > rand) 
				{
					return cw.prefab;
				}
			}

			return null;
		}
	}

	[System.Serializable]
	public class ConsumableWeights 
	{
		public Consumable prefab;
		public int weight;
	}

	public static SequenceManager instance;
	public Wave[] waves;
	public List<Sequence> queuedSequences;
	public int sequenceMin;
	public int sequenceMax;
	public Sequence sequenceObj;
	public Vector3 sequenceSpeed;

	private Wave currentWave;
	private int totalWaveWeight;

	// Use this for initialization

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	void Start () 
	{
		
	}
	public void Init()
	{
		var s = InstantiateSequence();
		queuedSequences.Add (s);
		totalWaveWeight = waves.Sum(w => w.weight);
		foreach(var wave in waves)
		{
			wave.CalcTotalWeight();
		}

		SetRandomWave();
	}

	public void SetRandomWave()
	{
		var rand = Random.Range (0, totalWaveWeight);

		var weight = 0;
		for(int i = 0; i < waves.Length; i++) {
			var cw = waves[i];
			weight += cw.weight;
			if(weight > rand) {
				currentWave = cw;
				return;
			}
		}

	}

	public Consumable GetRandomConsumable()
	{
		return currentWave.GetRandomConsumable();
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
	public bool SequenceOver()
	{
		return queuedSequences[0].sequenceConsumables.Count < 1;
	}
	public void ClearSequences()
	{
		foreach (Sequence s in queuedSequences) {
			s.EndSequence ();
			s.isPlaying = false;
		}
	}

}
