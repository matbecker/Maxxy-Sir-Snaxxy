using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HighScoreMenu : MonoBehaviour {

	public static HighScoreMenu instance;
	public List<TextMeshProUGUI> records;
	public Coroutine recordsCor;

	[System.Serializable]
	public class Column
	{
		public Button labelButton;
		public Image columnImage;
		public TextMeshProUGUI labelText;
		public TextMeshProUGUI columnText;
		public List<string> columnEntires;
		public bool visible;
	}
	public List<Column> columns;
	public Column currentColumn;
	// Use this for initialization
	void Awake()
	{
		if (!instance)
			instance = this;

		foreach (var c in columns)
		{
			c.labelText.color = Color.clear;
			c.labelButton.image.color = Color.clear;
			c.columnImage.transform.localScale = new Vector3(1.0f,0.0f,0.0f);
			c.visible = false;
		}
	}
	void Start () 
	{
		recordsCor = null;
	}
	public void Init()
	{
		foreach (var l in records)
		{
			l.gameObject.SetActive(true);
		}
		foreach (var c in columns)
		{
			if (c.labelButton.GetComponent<RectTransform>().anchoredPosition.x == 500.0f)
				currentColumn = c;
			
			if (c == currentColumn)
				ToggleColumn(currentColumn.columnImage).SetDelay(2.0f);

			c.labelButton.image.DOColor(MainMenu.instance.GetRandomColour(), 1.0f).SetDelay(1.0f).OnComplete(() => {
				var labelColour = c.labelButton.image.color;
				var ColumnColour = new Color(labelColour.r, labelColour.g, labelColour.b, 0.5f);
				c.columnImage.color = ColumnColour;
				c.labelText.DOColor(Color.white,1.0f);
			});

		}
	}
	public Tween ToggleColumn(Image img)
	{
		if (img.transform.localScale.y < 1.0f)
		{
			return img.transform.DOScaleY(1.0f,1.0f).SetEase(Ease.OutBounce,2.0f,1.5f);
		}
		else
		{
			return img.transform.DOScaleY(0.0f,1.0f).SetEase(Ease.InElastic,1.0f,1.0f);
		}
	}
	public void Back()
	{
		foreach (var c in columns)
		{
			c.columnImage.transform.DOScaleY(0.0f,1.0f).SetEase(Ease.InElastic,1.0f,1.0f).OnComplete(() => {
				c.labelButton.image.DOColor(Color.clear,1.0f);
				c.labelText.DOColor(Color.clear,1.0f);
			});
		}
	}
	// Update is called once per frame
	void Update () 
	{
		var aP = gameObject.GetComponent<RectTransform>().anchoredPosition;
		if (MainMenu.instance.inHighScoreMenu)
		{
			if (aP.x > -400.0f && aP.x < 400.0f)
			{
				var touch = Input.GetTouch(0);

				//if (Input.GetTouch().phase == TouchPhase.Moved)
				if (touch.phase == TouchPhase.Moved)
				{
					var pos = gameObject.GetComponent<RectTransform>().anchoredPosition;
					gameObject.GetComponent<RectTransform>().DOAnchorPosX(pos.x + 200.0f,1.0f).SetEase(Ease.OutElastic, 1.0f,1.0f);
				}
			}
		}

	}
}
