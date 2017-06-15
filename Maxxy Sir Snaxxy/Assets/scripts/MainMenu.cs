using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainMenu : MonoBehaviour {

	public enum AnimationType { ToggleFade, PunchScale };
	public List<TextMeshProUGUI> titleLetters;
	public List<TextMeshProUGUI> maxxy;
	public List <TextMeshProUGUI> snaxxy;
	public List<Tween> animations;
	private Coroutine cor;
	// Use this for initialization
	void Start () 
	{
//		foreach (var l in titleLetters)
//		{
//			l.color = Color.clear;
//
//		}
		cor = StartCoroutine(SequentialLetterAnimation(titleLetters,0.05f,AnimationType.PunchScale));
	}
	public IEnumerator SequentialLetterAnimation(List<TextMeshProUGUI> l, float delay, AnimationType at)
	{
		for (int i = 0; i < l.Count; i++)
		{
			switch (at)
			{
			case AnimationType.ToggleFade:
				l[i].DOColor(Color.white,0.1f);
				break;
			case AnimationType.PunchScale:
				l[i].transform.DOScale(Vector3.one * 1.1f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic,1.0f,1.0f);
				break;
			}
			yield return new WaitForSeconds(delay);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
