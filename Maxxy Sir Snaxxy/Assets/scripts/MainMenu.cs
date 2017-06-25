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
	public enum LetterAnimation { FadeIn, FadeOut, FadeOutIn, PunchScale, DoublePunch, FlipLetters, Rotate, UnderLine, ChangeColour, Wave, Stretch };
	public enum ImageAnimation { FadeIn, FadeOut, FadeOutIn, Rotate, ShrinkExpand, SwapImage, UnifyImage, PunchScale };
	public enum SequenceType { None, Forward, Backward, GroupedOrdered, GroupedUnordered, sGroupedUnordered, Single, sOdd, Odd, sEven, Even };
	public List<TextMeshProUGUI> titleLetters;
	public List<TextMeshProUGUI> maxxy;
	public List<TextMeshProUGUI> sir;
	public List<TextMeshProUGUI> snaxxy;
	public List<Image> cBorder;
	public Color[] colours;
	public Consumable[] consumables;
	private Coroutine cor;
	private Coroutine borderCor;
	private Coroutine recordsCor;
	public bool inHighScoreMenu;
	public Button backButton;

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		Init(2.3f);
	}
	public void Init(float delay)
	{
		foreach (var t in titleLetters)
		{
			t.gameObject.SetActive(true);
			t.color = Color.clear;
			t.fontStyle = FontStyles.Bold;
		}
		MakeTextAnimate(true, titleLetters, true);
		foreach (var b in buttons)
		{
			b.image.color = GetRandomColour();
			b.transform.localScale = new Vector3(1.0f,0.0f,1.0f);
			b.transform.DOScaleY(1.0f,1.0f).SetEase(Ease.OutElastic, 2.0f,1.0f).SetDelay(delay).OnComplete(() => {
				
			});
		}
		backButton.transform.localScale = new Vector3(1.0f,0.0f,1.0f);

		if (!inHighScoreMenu)
		{
			foreach(var bc in cBorder)
			{
				bc.color = Color.clear;
			}
			MakeBorderAnimate(true);
		}
		else
		{
			inHighScoreMenu = false;
		}
		

	}
	public void ActivateBackButton(float delay)
	{
		backButton.image.color = GetRandomColour();
		backButton.transform.DOScaleY(1.0f,1.0f).SetEase(Ease.OutElastic, 2.0f,1.0f).SetDelay(delay).OnComplete(() => {
				
		});
	}
	public Consumable GetRandomConsumable()
	{
		var rand = Random.Range(0, consumables.Length);
		return consumables[rand];
	}
	public Color GetRandomColour()
	{
		var index = Random.Range(0, colours.Length);

		return colours[index];
	}
	public IEnumerator SequentialImageAnimation(float delay, ImageAnimation at, SequenceType st)
	{
		//set a random sprite 
		Sprite s = GetRandomConsumable().sr.sprite;
		switch (st)
		{
		case SequenceType.None:
			yield return new WaitForSeconds(1.0f);
			break;
		case SequenceType.Forward:
			for (int i = 0; i < cBorder.Count; i++)
			{
				PlayImageAnimation(at, i, s, delay);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Backward:
			for (int i = cBorder.Count - 1; i >= 0; i--)
			{
				PlayImageAnimation(at, i, s, delay);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Single:
			var index = Random.Range(0,cBorder.Count);
			PlayImageAnimation(at, index, s, delay);
			break;
		case SequenceType.GroupedOrdered:
			for (int i = 0; i < cBorder.Count; i += 5)
			{
				PlayImageAnimation(at, i, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int j = 1; j < cBorder.Count; j += 5)
			{
				PlayImageAnimation(at, j, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int k = 2; k < cBorder.Count; k += 5)
			{
				PlayImageAnimation(at, k, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int l = 3; l < cBorder.Count; l += 5)
			{
				PlayImageAnimation(at, l, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int m = 4; m < cBorder.Count; m += 5)
			{
				PlayImageAnimation(at, m, s, delay);
			}
			break;
		case SequenceType.GroupedUnordered:
			for (int k = 2; k < cBorder.Count; k += 5)
			{
				PlayImageAnimation(at, k, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int i = 0; i < cBorder.Count; i += 5)
			{
				PlayImageAnimation(at, i, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int l = 3; l < cBorder.Count; l += 5)
			{
				PlayImageAnimation(at, l, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int j = 1; j < cBorder.Count; j += 5)
			{
				PlayImageAnimation(at, j, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int m = 4; m < cBorder.Count; m += 5)
			{
				PlayImageAnimation(at, m, s, delay);
			}
			break;
		case SequenceType.sGroupedUnordered:
			for (int k = 2; k < cBorder.Count; k += 5)
			{
				yield return new WaitForSeconds(delay);
				PlayImageAnimation(at, k, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int i = 0; i < cBorder.Count; i += 5)
			{
				yield return new WaitForSeconds(delay);
				PlayImageAnimation(at, i, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int l = 3; l < cBorder.Count; l += 5)
			{
				yield return new WaitForSeconds(delay);
				PlayImageAnimation(at, l, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int j = 1; j < cBorder.Count; j += 5)
			{
				yield return new WaitForSeconds(delay);
				PlayImageAnimation(at, j, s, delay);
			}
			yield return new WaitForSeconds(delay);
			for (int m = 4; m < cBorder.Count; m += 5)
			{
				yield return new WaitForSeconds(delay);
				PlayImageAnimation(at, m, s, delay);
			}
			break;
		case SequenceType.sEven:
			for (int i = 0; i < cBorder.Count; i += 2)
			{
				PlayImageAnimation(at, i, s, delay);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Even:
			for (int i = 0; i < cBorder.Count; i += 2)
			{
				PlayImageAnimation(at, i, s, delay);
			}
			break;
		case SequenceType.sOdd:
			for (int i = 1; i < cBorder.Count; i += 2)
			{
				PlayImageAnimation(at, i, s, delay);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Odd:
			for (int i = 1; i < cBorder.Count; i += 2)
			{
				PlayImageAnimation(at, i, s, delay);
			}
			break;
		}
		yield return new WaitForSeconds(2.0f);
		MakeBorderAnimate(false);
	}
	public void PlayImageAnimation(ImageAnimation at, int index, Sprite s, float delay)
	{
		switch (at)
		{
		case ImageAnimation.FadeIn:
			cBorder[index].DOColor(Color.white,delay);
			break;
		case ImageAnimation.FadeOut:
			cBorder[index].DOColor(Color.clear, delay);
			break;
		case ImageAnimation.FadeOutIn:
			cBorder[index].DOColor(Color.clear,delay).OnComplete(() => {
				cBorder[index].DOColor(Color.white,delay);
			});
			break;
		case ImageAnimation.Rotate:
			cBorder[index].transform.DORotate(new Vector3(0.0f,0.0f,360.0f), 1.0f, RotateMode.FastBeyond360);
			break;
		case ImageAnimation.ShrinkExpand:
			cBorder[index].transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => {
				cBorder[index].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutElastic,1.0f,0.25f);
			});	
			break;
		case ImageAnimation.SwapImage:
			cBorder[index].DOColor(Color.clear,delay).OnComplete(() => {
				cBorder[index].sprite = GetRandomConsumable().sr.sprite;
				cBorder[index].DOColor(Color.white,delay);
			});
			break;
		case ImageAnimation.UnifyImage:
			cBorder[index].DOColor(Color.clear,delay).OnComplete(() => {
				cBorder[index].sprite = s;
				cBorder[index].DOColor(Color.white,delay);
			});
			break;
		case ImageAnimation.PunchScale:
			cBorder[index].transform.DOScale(Vector3.one * 1.2f, delay).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic,1.0f,0.2f);
			break;
		}
	}
	public IEnumerator SequentialLetterAnimation(List<TextMeshProUGUI> l, float delay, LetterAnimation at, SequenceType st)
	{
		switch (st)
		{
		case SequenceType.None:
			yield return new WaitForSeconds(1.0f);
			break;
		case SequenceType.Forward:
			if (at == LetterAnimation.FadeIn)
				yield return new WaitForSeconds(1.5f);
			for (int i = 0; i < l.Count; i++)
			{
				PlayLetterAnimation(l, at, i);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Backward:
			for (int i = l.Count - 1; i >= 0; i--)
			{
				PlayLetterAnimation(l, at, i);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Single:
			var index = Random.Range(0,l.Count);
			PlayLetterAnimation(l,at,index);
			break;
		case SequenceType.GroupedOrdered:
			break;
		case SequenceType.GroupedUnordered:
			break;
		case SequenceType.sGroupedUnordered:
			break;
		case SequenceType.sEven:
			for (int i = 0; i < l.Count; i += 2)
			{
				PlayLetterAnimation(l, at, i);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Even:
			for (int i = 0; i < l.Count; i += 2)
			{
				PlayLetterAnimation(l, at, i);
			}
			break;
		case SequenceType.sOdd:
			for (int i = 1; i < l.Count; i += 2)
			{
				PlayLetterAnimation(l, at, i);
				yield return new WaitForSeconds(delay);
			}
			break;
		case SequenceType.Odd:
			for (int i = 1; i < l.Count; i += 2)
			{
				PlayLetterAnimation(l, at, i);
			}
			break;
		}
		yield return new WaitForSeconds(2.0f);
		if (inHighScoreMenu)
			MakeTextAnimate(false, l, true);
		else
			MakeTextAnimate(false,l,false);
	}
	public Tween PlayLetterAnimation(List<TextMeshProUGUI> l, LetterAnimation at, int index)
	{
		var oldColor = (l[index].color.a > 0) ? Color.white : Color.clear;
		var newColor = (oldColor.a > 0) ? Color.clear : Color.white;

		Tween t = null;
		switch (at)
		{
		case LetterAnimation.FadeIn:
			l[index].fontStyle = FontStyles.Bold;
			t = l[index].DOColor(Color.white, 0.1f);
			break;
		case LetterAnimation.FadeOut:
			t = l[index].DOColor(Color.clear,0.1f).OnComplete(() => {
				l[index].transform.DORotate(Vector3.zero, 0.0f);
				l[index].transform.DOScale(Vector3.one, 1.0f).OnComplete(() => {
					l[index].gameObject.SetActive(false);
				});
			});
			break;
		case LetterAnimation.FadeOutIn:
			l[index].DOColor(Color.clear,0.1f).OnComplete(() => {
				t = l[index].DOColor(Color.white,0.1f);
			});
			break;
		case LetterAnimation.PunchScale:
			t = l[index].transform.DOScale(Vector3.one * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic,1.0f,1.0f);
			break;
		case LetterAnimation.DoublePunch:
			t = l[index].transform.DOScale(Vector3.one * 1.2f, 0.1f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InBounce,1.0f,1.0f);
			break;
		case LetterAnimation.FlipLetters:
			var currentRot = l[index].transform.rotation.y;
			var newRot = (currentRot < 180.0f) ? 180.0f : 0.0f;
			t = l[index].transform.DORotate(new Vector3(0.0f,newRot,0.0f), 1.0f, RotateMode.Fast);
			break;
		case LetterAnimation.Rotate:
			t = l[index].transform.DORotate(new Vector3(0.0f,0.0f,360.0f),1.0f,RotateMode.FastBeyond360);
			break;
		case LetterAnimation.UnderLine:
			var newLine = (l[index].fontStyle == FontStyles.Underline) ? FontStyles.Bold : FontStyles.Underline;
			l[index].fontStyle = newLine;
			break;
		case LetterAnimation.ChangeColour:
			var dur = Random.value;
			while (dur < 0.5f)
				dur = Random.value;
			t = l[index].DOColor(GetRandomColour(), dur);
			break;
		case LetterAnimation.Wave:
			t = l[index].rectTransform.DOAnchorPosY(l[index].rectTransform.anchoredPosition.y - 20.0f, 0.5f).SetLoops(2,LoopType.Yoyo).SetEase(Ease.InBack,2.0f,0.5f);
			break;
		case LetterAnimation.Stretch:
			t = l[index].transform.DOScaleY(1.3f, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic, 2.0f,1.0f);
			break;
		}
		return t;
	}
	public void MakeTextAnimate(bool intro, List<TextMeshProUGUI> l, bool isTitle)
	{
		var animIndex = Random.Range(2, (int)LetterAnimation.Stretch + 1);
		var sequenceIndex = Random.Range(0, (int)SequenceType.Even + 1);
		var listType = Random.Range(0, 4);
		if (intro)
		{
			animIndex = 0;
			sequenceIndex = 1;
		}
			

		if (isTitle)
		{
			var list = new List<TextMeshProUGUI>();
			if (listType == 0 || intro)
				list = titleLetters;
			else if (listType == 1)
				list = maxxy;
			else if (listType == 2)
				list = sir;
			else
				list = snaxxy;

			StartCoroutine(SequentialLetterAnimation(list,0.05f,(LetterAnimation)animIndex, (SequenceType)sequenceIndex));
		}
		else
			StartCoroutine(SequentialLetterAnimation(l,0.05f,(LetterAnimation)animIndex, (SequenceType)sequenceIndex));

	}
	private void MakeBorderAnimate(bool intro)
	{
		var animIndex = Random.Range(2, (int)ImageAnimation.PunchScale + 1);
		var sequenceIndex = Random.Range(0, (int)SequenceType.Even + 1);
		var delay = 0.1f;
		if (intro)
		{
			animIndex = 0;
			sequenceIndex = 4;
			delay = 0.3f;
		}

		if (borderCor != null)
		{
			StopCoroutine(borderCor);
			borderCor = null;
		}

		borderCor = StartCoroutine(SequentialImageAnimation(delay,(ImageAnimation)animIndex, (SequenceType)sequenceIndex));
	}
	public void StartGame()
	{
		HideMenu(true);
		overlay.DOColor(Color.clear, 1.0f).SetDelay(1.0f).OnComplete(() => {
			gameObject.SetActive(false);
			borderCor = null;
			cor = null;
			recordsCor = null;
			Character.instance.Init();
			Layout.instance.Init();
			SequenceManager.instance.Init();
			UserInterface.instance.Init();
			GameManager.instance.Init();
		});
	}
	public void Settings()
	{
		HideMenu(false);
		ActivateBackButton(1.0f);
	}
	public void HighScores()
	{
		HideMenu(false);
		MakeTextAnimate(true, HighScoreMenu.instance.records, false);
		inHighScoreMenu = true;
		HighScoreMenu.instance.Init();
		ActivateBackButton(3.0f);
	}
	public void HideMenu(bool hideBorder)
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
		cor = StartCoroutine(SequentialLetterAnimation(titleLetters,0.05f,LetterAnimation.FadeOut, SequenceType.Backward));

		if (hideBorder)
		{
			if (borderCor != null)
			{
				StopCoroutine(borderCor);
				borderCor = null;
			}
			borderCor = StartCoroutine(SequentialImageAnimation(0.25f,ImageAnimation.FadeOut, SequenceType.GroupedUnordered));
		}
	}
	public void Back()
	{
		backButton.transform.DOScaleY(0.0f,1.0f).SetEase(Ease.InElastic, 1.0f,0.33f);
		if (inHighScoreMenu)
		{
			if (recordsCor != null)
			{
				StopCoroutine(recordsCor);
				recordsCor = null;
			}
			recordsCor = StartCoroutine(SequentialLetterAnimation(HighScoreMenu.instance.records,0.05f,LetterAnimation.FadeOut, SequenceType.Backward));
			HighScoreMenu.instance.Back();
		}
		Init(2.3f);
	}
	public void Quit()
	{
		Application.Quit();
	}
}
