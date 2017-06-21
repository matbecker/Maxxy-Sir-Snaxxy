using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class UserInterface : MonoBehaviour {

	public static UserInterface instance;
	public TextMeshProUGUI score;
	public TextMeshProUGUI combo;
	public TextMeshProUGUI multiplier;
	public TextMeshProUGUI failText;
	public TextMeshProUGUI waveText;
	public List<TextMeshProUGUI> ingameTextObjects;
	public string[] inGameFailText;
	public string[] inGameDropText;
	public Image SizeOMeter;
	public Image sizeOMeterBackground;
	public Image arrow;
	public string[] skinnyText;
	public string[] fatText;
	private Tween colorTween;
	private Coroutine counter;
	private Tween multiplierTween;
	public bool intermission;

	public int waveInt = 1;
	void Awake()
	{
		if (!instance)
			instance = this;

		ResetUI();
		score.transform.localScale = Vector3.zero;
	}
	// Use this for initialization
	void Start () 
	{
		arrow.color = Color.clear;
	}

	public void Init()
	{
		ResetUI();
		score.transform.localScale = Vector3.zero;
		counter = null;
		score.transform.DOScale(Vector3.one, 1.0f).OnComplete(() => {
			PlayWaveIntermission(0.0f);
			BounceScore();
		});
	}

	public void BumpText(TextMeshProUGUI t)
	{
		t.rectTransform.DOScale (new Vector3 (t.rectTransform.localScale.x * 1.1f, t.rectTransform.localScale.x * 1.1f, t.rectTransform.localScale.z), 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
		{
			t.rectTransform.localScale = Vector3.one;		
		});
	}
	public Tween PlayRandomWaveFinish()
	{
		Tween t = null;
		int index = Random.Range(0,5);
		switch(index)
		{
		case 0:
			waveText.transform.DOScaleX(1.2f, 0.5f).SetEase(Ease.OutElastic, 1.0f, 0.5f);
			t =	waveText.transform.DOScale(Vector3.zero,1.0f);
			break;
		case 1:
			waveText.transform.DORotate(new Vector3(-360.0f,0.0f,waveText.rectTransform.rotation.eulerAngles.z),1.0f, RotateMode.FastBeyond360);
			t = waveText.transform.DOScaleY(0.0f,1.0f).SetEase(Ease.InBack,1.0f,1.0f).SetDelay(1.0f);
			break;
		case 2:
			waveText.transform.DORotate(new Vector3(0.0f,180.0f,waveText.rectTransform.rotation.eulerAngles.z), 1.0f, RotateMode.Fast);
			t = waveText.DOColor(Color.clear,1.0f);
			break;
		case 3:
			waveText.transform.DOScaleY(1.4f, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutElastic,1.0f,1.0f);
			t = waveText.rectTransform.DOAnchorPosX(300.0f,1.0f).SetEase(Ease.InOutElastic,1.0f,1.0f).SetDelay(0.25f);
			break;
		case 4:
			waveText.transform.DOScaleX(1.4f, 0.3f).SetEase(Ease.OutBounce,1.0f,1.0f);
			waveText.transform.DOScale(Vector3.zero,1.0f).SetDelay(0.3f);
			t = waveText.rectTransform.DOAnchorPosY(400.0f,1.0f).SetEase(Ease.OutElastic,1.0f,1.0f).SetDelay(0.3f);
			break;
		}
		return t;
	}
	public Tween PlayRandomWaveIntro()
	{
		waveText.text = "Wave" + waveInt;
		waveText.transform.localRotation = Quaternion.identity;
		Tween t = null;
		int index = Random.Range(0,5);

		switch (index)
		{
		case 0:
			waveText.transform.localScale = Vector3.zero;
			waveText.color = Color.white;
			waveText.rectTransform.anchoredPosition = Vector2.zero;
			t = waveText.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack,1.0f,1.0f);
			break;
		case 1:
			waveText.transform.localScale = Vector3.one;
			waveText.color = Color.clear;
			waveText.rectTransform.anchoredPosition = Vector2.zero;
			waveText.DOColor(Color.white, 1.0f);
			t = waveText.transform.DOScale(Vector3.one * 1.1f,0.3f).SetLoops(2,LoopType.Yoyo).SetDelay(1.0f).SetEase(Ease.OutCirc,1.0f,0.3f);
			break;
		case 2:
			waveText.transform.localScale = new Vector3(1.0f,0.0f,1.0f);
			waveText.color = Color.white;
			waveText.rectTransform.anchoredPosition = Vector2.zero;
			t = waveText.transform.DOScaleY(1.0f, 1.0f).SetEase(Ease.OutBounce,0.5f,1.0f);
			break;
		case 3:
			waveText.transform.localScale = Vector3.one;
			waveText.color = Color.white;
			waveText.rectTransform.anchoredPosition = new Vector2(-300.0f,0.0f);
			t = waveText.rectTransform.DOAnchorPosX(0.0f, 1.0f,false).SetEase(Ease.OutElastic,1.0f,1.0f);
			break;
		case 4:
			waveText.transform.localScale = Vector3.zero;
			waveText.color = Color.white;
			waveText.rectTransform.anchoredPosition = new Vector2(0.0f,-400.0f);
			waveText.transform.DOScale(Vector3.one,0.75f);
			t = waveText.rectTransform.DOAnchorPosY(0.0f, 1.0f,false).SetEase(Ease.InOutBack,1.0f,1.0f);
			break;
		}
		return t;

	}
	public void PlayWaveIntermission(float delay)
	{
		waveText.rectTransform.DORotate(new Vector3(0.0f,0.0f, Layout.instance.GetCurrentScreen().rotation),0.0f, RotateMode.Fast);
		arrow.DOColor(Color.white, 0.5f).SetDelay(0.25f);
		arrow.rectTransform.DOAnchorPosY(-80.0f,0.5f).SetLoops(10,LoopType.Yoyo).SetEase(Ease.InOutBack,1.0f,0.25f);
		intermission = true;
		PlayRandomWaveIntro().SetDelay(delay).OnComplete(() => {
			arrow.DOColor(Color.clear,0.5f).SetDelay(0.5f);
			PlayRandomWaveFinish().OnComplete(() => {
				SequenceManager.instance.GetCurrentSequence().SpawnSequence();
				intermission = false;
			});
		});
	}
	public void AdjustSizeOMeter()
	{
		var c = Character.instance;

		if (c.currentSize > c.minSize + 0.25f || c.currentSize < c.maxSize - 0.25f)
		{
			if (colorTween != null)
			{
				colorTween.Kill(true);
				colorTween = SizeOMeter.DOColor(Color.black, 0.2f); 
				colorTween = null;
			}
		}
		//if i am bigger than average
		if (c.currentSize > c.midSize) {
			//flip the scale of the meter if its upside down
			if (SizeOMeter.transform.localScale.y == -1)
				SizeOMeter.transform.localScale = Vector3.one;

			//if i am bigger than i am aloud to be
			if (c.currentSize >= c.maxSize)
				SizeOMeter.transform.DOScaleY (1.0f, 1.0f); //full size
			else {
				var diff = c.currentSize - c.midSize; // get the percentage of the bar that needs to be filled
				SizeOMeter.transform.DOScaleY (diff, 1.0f); //fill that shit
			}
		//if i am smaller than average
		} else if (c.currentSize <= c.midSize) {
			//flip the scale of the meter if its upside down
			if (SizeOMeter.transform.localScale.y == 1)
				SizeOMeter.transform.localScale = new Vector3(1,-1,1);

			//if i am smaller than i am aloud to be
			if (c.currentSize <= c.minSize)
				SizeOMeter.transform.DOScaleY (-1.0f, 1.0f); //full negative size
			else {
				var d = c.midSize - c.currentSize;
				SizeOMeter.transform.DOScaleY (d * -1, 1.0f); //negate the result 
			}
		} else {
			SizeOMeter.transform.localScale = new Vector3 (1, 0, 1);
		}
		if (c.currentSize <= c.minSize + 0.25f || c.currentSize >= c.maxSize - 0.25f)
		{
			//flash the bar if im close to losing because of my weight
			 colorTween = SizeOMeter.DOColor(Color.red, 0.2f).SetLoops(-1, LoopType.Yoyo); 
		}
	}
	public void UpdateMultiplier(GameManager.Multiplier m, float mSize)
	{
		if (m == GameManager.Multiplier.None)
		{
			multiplier.text = "";
		}
		else
		{
			//set the multiplier text
			multiplier.SetText(m.ToString());
		}

		//set the new size
		if (multiplierTween != null)
		{
			multiplierTween.Kill(false);
			multiplierTween = null;
		}
		multiplierTween = multiplier.rectTransform.DOScale (Vector3.one * mSize, 1.0f);
		multiplierTween.OnComplete(() => {
			multiplierTween.Kill(true);
			multiplierTween = null;
		});
	}
	public IEnumerator ScoreCounter()
	{
		for (int i = int.Parse(score.text); i <= Character.instance.score; i++)
		{
			score.text = i.ToString();

			var consumableValue = Character.instance.GetLastConsumable().value;

			//var delay = 0.1f;

			yield return new WaitForSeconds(0.001f);
		}
		StopCoroutine(ScoreCounter());
		counter = null;
	}
	public void MakeTextDance()
	{
		combo.text = Character.instance.combo.ToString ();

		if (counter == null)
		{
			counter = StartCoroutine(ScoreCounter());
		}
		BumpText (combo);
	}
	public void DisplayLastWave()
	{
		waveText.rectTransform.anchoredPosition = new Vector2(0.0f,-240.0f);
		waveText.transform.localRotation = Quaternion.identity;
		waveText.transform.localScale = Vector3.one;
		waveText.color = Color.clear;
		waveText.DOColor(Color.white, 2.0f);
	}
	public void ResetUI()
	{
		combo.text = "";
		combo.DOColor(Color.clear,0.0f);
		score.text = "00";
		multiplier.text = "";
		SizeOMeter.transform.localScale = new Vector3 (1, 0, 1);
		sizeOMeterBackground.color = new Color(255.0f,255.0f,255.0f, 0.5f);
		failText.DOColor(Color.clear,0.0f);
		waveInt = 1;
		waveText.text = "";
		colorTween.Kill(true);
		colorTween = SizeOMeter.DOColor(Color.black, 0.2f); 
		colorTween = null;

	}
	public Tween DisplayFailMessage(string message)
	{
		failText.text = message;
		return failText.DOColor(Color.white, 1.0f);
	}
	public string GetSizeRelatedFailText(bool skinny)
	{
		if (skinny)
		{
			var sRand = Random.Range(0,skinnyText.Length);

			return skinnyText[sRand];
		}
		else
		{
			var fRand = Random.Range(0,fatText.Length);

			return fatText[fRand];
		}
	}
	public void BounceScore()
	{
		if (!GameManager.instance.inGame)
			return;
		
		score.transform.DOScaleY(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic, 0.5f,0.5f).OnComplete(() => {
			score.transform.DOScaleX(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic, 0.5f,0.5f).OnComplete(() => {
				score.transform.localScale = Vector3.one;
				BounceScore();
			});
		});
	}
	public void GameOver()
	{
		multiplier.transform.DOScale(Vector3.zero,1.0f);
		combo.DOColor(Color.clear,1.0f);
		sizeOMeterBackground.DOColor(Color.clear,1.0f);
	}
	public void HideUI()
	{
		waveText.DOColor(Color.clear,1.0f);
		failText.DOColor(Color.clear, 1.0f);
		score.transform.DOScale(Vector3.zero, 1.0f);
		multiplier.transform.DOScale(Vector3.zero, 1.0f);
	}
}
