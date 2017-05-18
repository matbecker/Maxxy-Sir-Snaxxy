﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UserInterface : MonoBehaviour {

	public static UserInterface instance;
//	public Text score;
	public TextMeshProUGUI score;
	public TextMeshProUGUI combo;
	public TextMeshProUGUI multiplier;
	public Image SizeOMeter;

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	// Use this for initialization
	void Start () {
		ResetUI();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void BumpText(TextMeshProUGUI t)
	{
		t.rectTransform.DOScale (new Vector3 (t.rectTransform.localScale.x * 1.1f, t.rectTransform.localScale.x * 1.1f, t.rectTransform.localScale.z), 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
		{
			t.rectTransform.localScale = Vector3.one;		
		});
	}
	public void AdjustSizeOMeter(Character c)
	{
		if (c.currentSize > c.midSize) {
			if (SizeOMeter.transform.localScale.y == -1)
				SizeOMeter.transform.localScale = Vector3.one;

			if (c.currentSize == c.maxSize)
				SizeOMeter.transform.DOScaleY (1.0f, 1.0f);
			else {
				var diff = c.currentSize - c.midSize;
				SizeOMeter.transform.DOScaleY (diff / c.midSize, 1.0f);
			}
		} else if (c.currentSize <= c.midSize) {
			if (SizeOMeter.transform.localScale.y == 1)
				SizeOMeter.transform.localScale = new Vector3(1,-1,1);

			if (c.currentSize == c.minSize)
				SizeOMeter.transform.DOScaleY (-1.0f, 1.0f);
			else {
				var d = c.midSize - c.currentSize;
				SizeOMeter.transform.DOScaleY (-1 * (d / c.midSize), 1.0f);
			}
		} else {
			SizeOMeter.transform.localScale = new Vector3 (1, 0, 1);
		}
	}
	public void MakeTextDance()
	{
		combo.text = Character.instance.combo.ToString ();
		score.text = Character.instance.score.ToString();
		BumpText (combo);
		BumpText (score);
	}
	public void ShowGameOverScreen()
	{
		GameoverScreen.instance.ShowMenu ();
	}
	public void HideGameOverScreen()
	{
		GameoverScreen.instance.HideMenu ();
	}
	public void ResetUI()
	{
		combo.text = "";
		combo.DOColor(Color.clear,0.0f);
		score.text = "00";
		multiplier.text = "";
		SizeOMeter.transform.localScale = new Vector3 (1, 0, 1);
	}
}