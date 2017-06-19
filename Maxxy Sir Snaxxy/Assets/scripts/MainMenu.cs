using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public static MainMenu instance;
	public Image overlay;
	public Image buttonPanel;
	public Button[] buttons;
	public enum AnimationType { FadeIn, FadeOut, FadeOutIn, PunchScale, DoublePunch, FlipLetters, Rotate, UnderLine, ChangeColour, Wave, Stretch };
	public enum SequenceType { None, Forward, Backward, Odd, Even };
	public List<TextMeshProUGUI> titleLetters;
	public List<TextMeshProUGUI> maxxy;
	public List<TextMeshProUGUI> sir;
	public List<TextMeshProUGUI> snaxxy;
	public Color[] colours;
	private Coroutine cor;
	// Use this for initialization
	void Start () 
	{
		if (!instance)
			instance = this;
		
		Init();
	}
	public void Init()
	{
		foreach (var t in titleLetters)
		{
			t.color = Color.clear;
		}
		MakeTextAnimate(true);
		foreach (var b in buttons)
		{
			b.transform.localScale = new Vector3(1.0f,0.0f,1.0f);
			b.transform.DOScaleY(1.0f,1.0f).SetEase(Ease.OutElastic, 2.0f,1.0f).SetDelay(1.0f);
		}
	}
	public IEnumerator SequentialLetterAnimation(List<TextMeshProUGUI> l, float delay, AnimationType at, SequenceType st)
	{
		
		Tween t = null;
		switch (st)
		{
		case SequenceType.None:
			yield return new WaitForSeconds(1.0f);
			break;
		case SequenceType.Forward:
			for (int i = 0; i < l.Count; i++)
			{
				PlayAnimation(l, at, i);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Backward:
			for (int i = l.Count - 1; i >= 0; i--)
			{
				PlayAnimation(l, at, i);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Even:
			for (int i = 0; i < l.Count; i += 2)
			{
				PlayAnimation(l, at, i);
			}
			break;
		case SequenceType.Odd:
			for (int i = 1; i < l.Count; i += 2)
			{
				PlayAnimation(l, at, i);
			}
			break;
		}
		yield return new WaitForSeconds(2.0f);
		MakeTextAnimate(false);
	}
	public Tween PlayAnimation(List<TextMeshProUGUI> l, AnimationType at, int index)
	{
		var oldColor = (l[index].color.a > 0) ? Color.white : Color.clear;
		var newColor = (oldColor.a > 0) ? Color.clear : Color.white;

		Tween t = null;
		switch (at)
		{
		case AnimationType.FadeIn:
			t = l[index].DOColor(Color.white, 0.1f);
			break;
		case AnimationType.FadeOut:
			t = l[index].DOColor(Color.clear,0.1f).OnComplete(() => {
				l[index].fontStyle = FontStyles.Bold;
				l[index].transform.DORotate(Vector3.zero, 0.0f);
			});
			break;
		case AnimationType.FadeOutIn:
			l[index].DOColor(Color.clear,0.1f).OnComplete(() => {
				t = l[index].DOColor(Color.white,0.1f);
			});
			break;
		case AnimationType.PunchScale:
			t = l[index].transform.DOScale(Vector3.one * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic,1.0f,1.0f);
			break;
		case AnimationType.DoublePunch:
			t = l[index].transform.DOScale(Vector3.one * 1.2f, 0.1f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InBounce,1.0f,1.0f);
			break;
		case AnimationType.FlipLetters:
			var currentRot = l[index].transform.rotation.y;
			var newRot = (currentRot < 180.0f) ? 180.0f : 0.0f;
			t = l[index].transform.DORotate(new Vector3(0.0f,newRot,0.0f), 1.0f, RotateMode.Fast);
			break;
		case AnimationType.Rotate:
			t = l[index].transform.DORotate(new Vector3(0.0f,0.0f,360.0f),1.0f,RotateMode.FastBeyond360);
			break;
		case AnimationType.UnderLine:
			var newLine = (l[index].fontStyle == FontStyles.Underline) ? FontStyles.Bold : FontStyles.Underline;
			l[index].fontStyle = newLine;
			break;
		case AnimationType.ChangeColour:
			var newColIndex = Random.Range(0, colours.Length);
			var dur = Random.value;
			while (dur < 0.5f)
				dur = Random.value;
			t = l[index].DOColor(colours[newColIndex], dur);
			break;
		case AnimationType.Wave:
			t = l[index].rectTransform.DOAnchorPosY(l[index].rectTransform.anchoredPosition.y - 20.0f, 0.5f).SetLoops(2,LoopType.Yoyo).SetEase(Ease.InBack,2.0f,0.5f);
			break;
		case AnimationType.Stretch:
			t = l[index].transform.DOScaleY(1.3f, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic, 2.0f,1.0f);
			break;
		}
		return t;
	}
	private void MakeTextAnimate(bool intro)
	{
		var animIndex = Random.Range(2, (int)AnimationType.Stretch + 1);
		var sequenceIndex = Random.Range(0, (int)SequenceType.Even + 1);
		var listType = Random.Range(0, 4);
		if (intro)
		{
			animIndex = 0;
			sequenceIndex = 1;
		}
			
		if (cor != null)
		{
			StopCoroutine(cor);
			cor = null;
		}
		var list = new List<TextMeshProUGUI>();
		if (listType == 0 || intro)
			list = titleLetters;
		else if (listType == 1)
			list = maxxy;
		else if (listType == 2)
			list = sir;
		else
			list = snaxxy;
		
		cor = StartCoroutine(SequentialLetterAnimation(list,0.05f,(AnimationType)animIndex, (SequenceType)sequenceIndex));
	}
	public void StartGame()
	{
		foreach (var b in buttons)
		{
			b.transform.DOScaleY(0.0f,1.0f).SetEase(Ease.InElastic, 1.0f,0.33f);
		}
		if (cor != null)
		{
			StopCoroutine(cor);
			cor = null;
		}
		cor = StartCoroutine(SequentialLetterAnimation(titleLetters,0.05f,AnimationType.FadeOut, SequenceType.Backward));
		overlay.DOColor(Color.clear, 1.0f).SetDelay(1.0f).OnComplete(() => {
			gameObject.SetActive(false);
			Character.instance.Init();
			Layout.instance.Init();
			SequenceManager.instance.Init();
			UserInterface.instance.Init();
			GameManager.instance.Init();
		});
	}
	public void Settings()
	{
		
	}
	public void Quit()
	{
		Application.Quit();
	}
}
