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
	public void HideMenu(float delay)
	{
		foreach (Button b in buttons)
		{
			b.transform.DOScaleY(0.0f,0.5f).SetEase(Ease.InElastic, 1.0f,0.25f);
		}

		overlay.DOColor (new Color (0, 0, 0, 0), 1.0f).SetDelay(delay).OnComplete(() => {
			overlay.gameObject.SetActive (false);
			foreach (Button b in buttons) {
				b.gameObject.SetActive (false);
			}
			buttonPanel.transform.localScale = Vector3.zero;
			buttonPanel.gameObject.SetActive (false);
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
			buttonPanel.transform.localScale = Vector3.one;
			foreach (Button b in buttons)
			{
				b.transform.DOScaleY(1.0f,1.0f).SetEase(Ease.OutElastic, 2.0f,1.0f);
			}
			UserInterface.instance.DisplayLastWave();
		});
	}

	public void Retry()
	{
		HideMenu (1.0f);
		Layout.instance.Init();
		Character.instance.Reset();
		GameManager.instance.Reset();
		UserInterface.instance.ResetUI();
		SequenceManager.instance.Reset();
		UserInterface.instance.PlayWaveIntermission(1.0f);
	}
	public void Menu()
	{
		GameManager.instance.inGame = false;
		SequenceManager.instance.DeleteSequences();
		MainMenu.instance.gameObject.SetActive(true);
		HideMenu(0.5f);

		MainMenu.instance.overlay.DOColor(Color.black,1.0f).OnComplete(() => {
			MainMenu.instance.Init();
			UserInterface.instance.HideUI();
		});
	}
	public void Quit()
	{
		Application.Quit();
	}
}
