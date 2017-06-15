using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
using System;

public class GameManager : MonoBehaviour {

	public enum ColourType { Blank, Purple, Blue, Green, Yellow, Orange, Red };
	public ColourType currentColour;

	public enum Multiplier { None, x2, x3, x4, x5, x6, x8, x10 };
	public Multiplier currentMultiplier;

	public static GameManager instance;
	public BackgroundQuadrant[] backgroundQuads;
	public Color[] backgroundColours;
	public int colourIndex;

	public int strikes;
	public int maxStrikes;


	public bool gameover
	{
		get
		{
			return strikes >= maxStrikes;
		}
	}
	public bool inGame = false;
	public Dictionary<string, int> highscoreDict;

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	public void Init()
	{
		var colourIndex = UnityEngine.Random.Range (0, backgroundColours.Length);
		Camera.main.DOColor (backgroundColours [colourIndex], 0.0f);
		currentColour = HelperFunctions.SetColourType (colourIndex);
		SetGamePlayVariables(currentColour);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (inGame)
		{
			int rand = UnityEngine.Random.Range (0, 1000);

			if (rand == 1) 
			{
				var colourIndex = UnityEngine.Random.Range (0, backgroundColours.Length);
				Camera.main.DOColor (backgroundColours [colourIndex], 2.0f);

				currentColour = HelperFunctions.SetColourType (colourIndex);
			}
		}

	}
	public void SetGamePlayVariables(ColourType col)
	{
		var c = Character.instance;

		if (gameover || c.combo < 10)
		{
			SetMultiplierVariables(Multiplier.None, 0.0f, col);
			return;
		}
		else
		{
			if (c.combo >=  10 && c.combo < 25)
			{
				SetMultiplierVariables(Multiplier.x2, 1.0f, col);
			}
			else if (c.combo >= 25 && c.combo < 50)
			{
				SetMultiplierVariables(Multiplier.x3, 1.5f, col);
			}
			else if (c.combo >= 50 && c.combo < 100)
			{
				SetMultiplierVariables(Multiplier.x4, 2.0f, col);
			}
			else 
			{
				SetMultiplierVariables(Multiplier.x5, 2.5f, col);
			}
		}
	}
	public void SetMultiplierVariables(Multiplier m, float mSize, ColourType col)
	{
		if (HelperFunctions.CompareColour (Character.instance.colour, col))
		{
			mSize += 0.75f;
			switch (m)
			{
			case Multiplier.None:
				m = Multiplier.x2;
//				Debug.Log("x2");
				break;
			case Multiplier.x2:
				m = Multiplier.x4;
//				Debug.Log("x4");
				break;
			case Multiplier.x3:
				m = Multiplier.x6;
//				Debug.Log("x6");
				break;
			case Multiplier.x4:
				m = Multiplier.x8;
//				Debug.Log("x8");
				break;
			case Multiplier.x5:
				m = Multiplier.x10;
//				Debug.Log("x10");
				break;
			}
		}
		else
			Debug.Log(m.ToString());
		//set the current multiplier
		currentMultiplier = m;
		//update the UI Multiplier
		UserInterface.instance.UpdateMultiplier(currentMultiplier, mSize);
		//set consumable values
		SequenceManager.instance.GetCurrentSequence ().SetConsumableValues (m);
	}
	public void SetBackgroundImages()
	{
		foreach (BackgroundQuadrant bq in backgroundQuads)
		{
			var rand = UnityEngine.Random.value;
			if (rand > 0.5f)
			{
				if (bq.isVisible)
					bq.Dissappear();
				else
					bq.Appear();
			}
		}
	}
	public void Strike(Consumable consumable) 
	{
		strikes++;
		Character.instance.ResetCombo();
		if (gameover)
		{
			//get random index for message list
			var messageIndex = UnityEngine.Random.Range(0,consumable.gameoverMessages.Length);
			//display error message
			UserInterface.instance.DisplayFailMessage(consumable.gameoverMessages[messageIndex]).OnComplete(() => 
			{ 
				GameOver(); 
				return;
			});

		}
	}
	public void DecreaseStrikes()
	{
		strikes--;

		if (strikes < 0)
			strikes = 0;
	}
	public void GameOver()
	{
		GameoverScreen.instance.ShowMenu();
		SequenceManager.instance.ClearSequences ();
		Character.instance.ResetCombo();
		SetGamePlayVariables(ColourType.Blank);
	}
	public void Reset()
	{
		strikes = 0;
		foreach (BackgroundQuadrant bq in backgroundQuads)
		{
			bq.Dissappear();
		}
	}
	public void CheckForNewWave(Consumable c)
	{
		if (!gameover)
		{
			if (c.lastItem && !UserInterface.instance.intermission)
			{
				UserInterface.instance.PlayWaveIntermission();
				Debug.Log("new wave started by " + c.name);

			}
		}	
	}
}
