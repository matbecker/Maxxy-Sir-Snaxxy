using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour {

	public GameManager.ColourType colour = GameManager.ColourType.Purple;

	//components
	[SerializeField] Rigidbody rb;
	[SerializeField] SphereCollider col;
	[SerializeField] Material mat;
	[SerializeField] Color[] colours;
	[SerializeField] TextMeshProUGUI consumableValueText;
	[SerializeField] Transform consumableValueContainer;

	//stats
	public float currentSize;
	public float minSize;
	public float maxSize;
	public float midSize
	{
		get
		{
			return (maxSize + minSize) / 2;
		}
	}
	[SerializeField] Vector2 bounceForce;
	public float moveduration;
	public int score;

	private Vector3 pos; 
	private bool bouncing;
	private float delay;
	public int combo;

	public int currentNodeIndex;
	public int colourIndex;

	public static Character instance;

	void Awake()
	{
		if (!instance)
			instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		currentSize = midSize;
		transform.localScale = Vector3.one * currentSize;
		colourIndex = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		pos = Camera.main.WorldToViewportPoint(transform.position);

		if (Layout.instance.currentLayout == Layout.ScreenState.Bottom) 
		{
			if (bouncing) 
			{
				delay += Time.deltaTime;
				if (delay > 0.1f) 
				{
					bouncing = false;
				}
			}
		}
	}
	private void OnCollisionEnter(Collision other)
	{
		var boundary = other.gameObject.GetComponent<Boundary> ();

		if (boundary != null && transform.position.y > other.transform.position.y)
		{
			Bounce ();
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		var node = other.gameObject.GetComponent<Node> ();

		if (node != null) 
		{
			currentNodeIndex = node.index;
		}

		var consumable = other.gameObject.GetComponent<Consumable>();

		if (consumable != null)
		{
			var type = consumable.type;
			switch (type) 
			{
			case Consumable.Type.Fruit:
				IncreaseCombo();
				//increase score
				score += consumable.value;
				transform.DOScale (transform.localScale * 1.1f, 0.05f).SetLoops(2,LoopType.Yoyo).OnComplete(() =>
				{
					transform.localScale = Vector3.one * currentSize;	
				});
				break;
			case Consumable.Type.Vegetable:
				score += consumable.value;
				combo = 0;
				//increase strikes
				GameManager.instance.Strike();
				break;
			case Consumable.Type.Cracker:
				IncreaseCombo();
				currentSize -= 0.2f;
				Resize ();
				break;
			case Consumable.Type.Treat:
				IncreaseCombo();
				score += consumable.value;
				currentSize += consumable.fatness;
				Resize ();
				break;
			case Consumable.Type.Dud:
				return;
				break;
			}
			DisplayConsumableValue(consumable);
			UserInterface.instance.MakeTextDance();
			GameManager.instance.SetMultiplier ();
		}

	}
	private void Bounce()
	{
		if (!bouncing) 
		{
			delay = 0.0f;
			rb.velocity = Vector3.zero;
			rb.AddForce(bounceForce);
			bouncing = true;
		}
	}
	public Color NextColour()
	{
		colourIndex++;

		if (colourIndex > colours.Length - 1)
			colourIndex = 0;

		colour = HelperFunctions.SetColourType (colourIndex);

		GameManager.instance.SetMultiplier ();
		
		return colours [colourIndex];
	}
	public void Resize()
	{
		if (currentSize > maxSize || currentSize < minSize) 
		{
			GameManager.instance.GameOver ();
			return;
		} 
		else 
		{
			transform.DOScale (Vector3.one * currentSize, 1.0f).SetEase (Ease.OutBounce,1.0f,1.0f);
		}
		AdjustStats ();
	}
	public void AdjustStats()
	{
		if (currentSize <= 0.25f) {
			moveduration = 0.1f;
			bounceForce.y = 300.0f;
		} else if (currentSize >= 0.25f && currentSize < 0.50f) {
			moveduration = 0.15f;
			bounceForce.y = 280.0f;
		} else if (currentSize >= 0.5f && currentSize < 0.75f) {
			moveduration = 0.2f;
			bounceForce.y = 260.0f;
		} else if (currentSize >= 0.75f && currentSize < 1.0f) {
			moveduration = 0.225f;
			bounceForce.y = 260.0f;
		} else if (currentSize >= 1.0f && currentSize < 1.25f) {
			moveduration = 0.25f;
			bounceForce.y = 240.0f;
		} else if (currentSize >= 1.25f && currentSize < 1.50f) {
			moveduration = 0.275f;
			bounceForce.y = 220.0f;
		} else if (currentSize >= 1.5f && currentSize < 1.75f) {
			moveduration = 0.3f;
			bounceForce.y = 200.0f;
		} else if (currentSize >= 1.75f && currentSize < 2.0f) {
			moveduration = 0.35f;
			bounceForce.y = 175.0f;
		} else if (currentSize >= 2.0f && currentSize < 2.25f) {
			moveduration = 0.4f;
			bounceForce.y = 150.0f;
		} else if (currentSize >= 2.25f && currentSize < 2.5f) {
			moveduration = 0.45f;
			bounceForce.y = 125.0f;
		} else if (currentSize >= 2.5f && currentSize < 2.75f) {
			moveduration = 0.5f;
			bounceForce.y = 100.0f;
		}
		UserInterface.instance.AdjustSizeOMeter (this);
	}
	public void GameOver()
	{
		bounceForce = Vector2.zero;
	}
	public void Reset()
	{
		score = 0;
		bounceForce = new Vector2(0.0f,200.0f);
		currentSize = midSize;
		transform.localScale = Vector3.one * currentSize;
	}
	public void ResetCombo()
	{
		combo = 0;
		UserInterface.instance.combo.DOColor(Color.clear,0.5f);
	}
	public void IncreaseCombo()
	{
		combo++;
		if (combo == 1)
		{
			UserInterface.instance.combo.DOColor(Color.white,0.5f);
		}
	}
	public void DisplayConsumableValue(Consumable c)
	{
		consumableValueText.text = c.value.ToString();
		consumableValueContainer.transform.DOScale(Vector3.one, 0.5f).OnComplete(() => {
			consumableValueContainer.transform.DOScale(Vector3.zero, 0.5f).SetDelay(0.5f);
		});
	}

}