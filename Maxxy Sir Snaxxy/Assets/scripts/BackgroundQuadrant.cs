using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundQuadrant : MonoBehaviour {

	public GameManager.ColourType colour;
	public Color[] colours;
	public Image backgroundImage;

	public int colourIndex;
	public bool isVisible;
	// Use this for initialization
	void Start () 
	{
		Dissappear();
	}
	public void Init()
	{
		Dissappear();
	}
	public void Appear()
	{
		//random colour index
		while (colourIndex == GameManager.instance.colourIndex)
			colourIndex = Random.Range(0,colours.Length);
		
		//set the enum colour type
		colour = HelperFunctions.SetColourType(colourIndex);
		//set the background colour
		backgroundImage.color = colours[colourIndex];
		//make the background appear
		backgroundImage.transform.DOScaleX(1.0f,1.0f).SetEase(Ease.OutBack, 0.5f,1.0f);
		isVisible = true;
	}
	public void Dissappear()
	{
		backgroundImage.transform.DOScaleX(0.0f,1.0f).SetEase(Ease.InBack, 0.5f,1.0f).OnComplete(() => {
			backgroundImage.transform.localScale = new Vector3(0.0f,1.0f,1.0f);
			isVisible = false;
		});
	}
}
