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
	public bool portraitView;
	// Use this for initialization
	void Start () 
	{
		portraitView = true;
		Dissappear();
	}
	public void Init()
	{
		portraitView = true;
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

		if (portraitView)
		{
			//make the background appear
			backgroundImage.transform.DOScaleX(1.0f,1.0f).SetEase(Ease.OutBack, 0.5f,1.0f).OnComplete(() => {
				isVisible = true;
			});
		}
		else
		{
			backgroundImage.transform.DOScaleY(0.3333f,1.0f).SetEase(Ease.OutBack,0.5f,1.0f).OnComplete(() => {
				isVisible = true;
			});
		}
	}
	public void Dissappear()
	{
		if (portraitView)
		{
			backgroundImage.transform.DOScaleX(0.0f,1.0f).SetEase(Ease.InBack, 0.5f,1.0f).OnComplete(() => {
				backgroundImage.transform.localScale = new Vector3(0.0f,1.0f,1.0f);
				isVisible = false;
			});
		}
		else
		{
			backgroundImage.transform.DOScaleY(0.0f,1.0f).SetEase(Ease.InBack, 0.5f,1.0f).OnComplete(() => {
				backgroundImage.transform.localScale = new Vector3(0.0f,1.0f,1.0f);
				isVisible = false;
			});
		}
	}
}
