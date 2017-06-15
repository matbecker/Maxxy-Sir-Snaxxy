using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameoverScreen : MonoBehaviour {

	public Image overlay;
	public Image buttonPanel;
	public Button[] buttons;
	public static GameoverScreen instance;
	public Color backgroundColour;
	// Use this for initialization
	void Awake()
	{
		if (!instance)
			instance = this;
	}
	void Start () 
	{
		
	}
	public void HideMenu()
	{
		foreach (Button b in buttons)
		{
			b.transform.DOScaleY (0.0f, 1.0f).SetEase (Ease.InOutBounce, 1.0f, 0.5f);
		}
		buttonPanel.transform.DOScaleX(0.0f,1.0f).OnComplete(() => {
			overlay.DOColor (new Color (0, 0, 0, 0), 1.0f).OnComplete(() => {
				overlay.gameObject.SetActive (false);
				foreach (Button b in buttons) {
					b.gameObject.SetActive (false);
				}
				buttonPanel.gameObject.SetActive (false);
			});
		});

	}
	public void ShowMenu()
	{
		overlay.gameObject.SetActive (true);
		foreach (Button b in buttons) {
			b.gameObject.SetActive (true);
		}
		buttonPanel.gameObject.SetActive (true);
		overlay.DOColor (Color.black, 1.0f).OnComplete(() => {
			buttonPanel.transform.DOScaleX(1.0f,0.5f).OnComplete(() => {
				foreach (Button b in buttons)
				{
					b.transform.DOScaleY(1.0f,1.0f).SetEase(Ease.OutBounce,1.0f,1.0f);
				}
				UserInterface.instance.DisplayLastWave();
			});
		});
	}

	public void Retry()
	{
		HideMenu ();
		Character.instance.Reset();
		GameManager.instance.Reset();
		UserInterface.instance.ResetUI();
		SequenceManager.instance.Reset();
		UserInterface.instance.PlayWaveIntermission();
	}
}
