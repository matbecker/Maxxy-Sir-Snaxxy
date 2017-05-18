using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public enum ColourType { Purple, Blue, Green, Yellow, Orange, Red };
	public ColourType currentColour;

	public enum Multiplier { x2, x3, x4, x5, x6, x7, x8, x16 };
	public Multiplier currentMultiplier;

	public static GameManager instance;
	public Color[] backgroundColours;
	public int colourIndex;

	private Tween multiplierTween;

	public int strikes;
	public int maxStrikes;

	public bool gameover
	{
		get
		{
			return strikes >= maxStrikes;
		}
	}

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		var colourIndex = Random.Range (0, backgroundColours.Length);
		Camera.main.DOColor (backgroundColours [colourIndex], 0.0f);
		currentColour = HelperFunctions.SetColourType (colourIndex);
		SetMultiplier ();
		multiplierTween = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int rand = Random.Range (0, 1000);

		if (rand == 1) 
		{
			var colourIndex = Random.Range (0, backgroundColours.Length);
			Camera.main.DOColor (backgroundColours [colourIndex], 2.0f);

			currentColour = HelperFunctions.SetColourType (colourIndex);

			if (!HelperFunctions.CompareColour (Character.instance.colour, currentColour)) 
			{
				UserInterface.instance.multiplier.rectTransform.DOScale (Vector3.zero, 0.5f);
			}
		}
	}
	public void SetMultiplier()
	{
		var c = Character.instance;

		multiplierTween.Kill (false);
		multiplierTween = null;

		if (!HelperFunctions.CompareColour (Character.instance.colour, currentColour) && c.combo < 10) 
			multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (Vector3.zero, 0.5f);
		else if (HelperFunctions.CompareColour (Character.instance.colour, currentColour)) 
		{
			multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (Vector3.one, 0.5f);
			UserInterface.instance.multiplier.text = "x2";
			currentMultiplier = Multiplier.x2;
		}

		if (c.combo >= 10 && c.combo < 25) {
			if (HelperFunctions.CompareColour (Character.instance.colour, currentColour)) {
				UserInterface.instance.multiplier.text = "x4";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (1.75f, 1.75f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x4;
			} else {
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (Vector3.one, 0.5f);
				UserInterface.instance.multiplier.text = "x2";
				currentMultiplier = Multiplier.x2;
			}

		} else if (c.combo >= 25 && c.combo < 50) {
			if (HelperFunctions.CompareColour (Character.instance.colour, currentColour)) {
				UserInterface.instance.multiplier.text = "x6";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (2.25f, 2.25f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x6;
			} else {
				UserInterface.instance.multiplier.text = "x3";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (1.5f, 1.5f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x3;
			}
		} else if (c.combo >= 50 && c.combo < 100) {
			if (HelperFunctions.CompareColour (Character.instance.colour, currentColour)) {
				UserInterface.instance.multiplier.text = "x8";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (2.5f, 2.5f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x8;
			} else {
				UserInterface.instance.multiplier.text = "x4";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (1.75f, 1.75f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x4;
			}
		} else if (c.combo >= 100) {
			if (HelperFunctions.CompareColour (Character.instance.colour, currentColour)) {
				UserInterface.instance.multiplier.text = "x16";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (3.0f, 3.0f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x16;
			} else {
				UserInterface.instance.multiplier.text = "x8";
				multiplierTween = UserInterface.instance.multiplier.rectTransform.DOScale (new Vector3 (2.5f, 2.5f, 1.0f), 1.0f);
				currentMultiplier = Multiplier.x8;
			}
		}
		SequenceManager.instance.GetCurrentSequence ().SetConsumableValues ();
	}
	public void Strike()
	{
		strikes++;
		Character.instance.ResetCombo();
		SetMultiplier();

		if (gameover)
			GameOver();
	}
	public void GameOver()
	{
		UserInterface.instance.ShowGameOverScreen ();
		SequenceManager.instance.ClearSequences ();
		Character.instance.ResetCombo();
	}
}
