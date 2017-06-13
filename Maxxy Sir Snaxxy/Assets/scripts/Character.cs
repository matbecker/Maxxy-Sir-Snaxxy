using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
	public float moveduration;
	public int score;

	private Vector3 pos; 
	private float delay;
	public int combo;

	public int colourIndex;

	public static Character instance;
	public List<Consumable> eatenConsumables;
	public Node currentNode;
	public Vector3 fallingSpeed;

	void Awake()
	{
		if (!instance)
			instance = this;

		eatenConsumables = new List<Consumable>();
	}
	// Use this for initialization
	void Start () 
	{
		currentSize = midSize;
		transform.localScale = Vector3.one * currentSize;
		colourIndex = 0;
		SetFallingSpeed(0.0f,-1.0f);
	}
	public void StopMoving()
	{
		rb.velocity = Vector3.zero;
		transform.position = new Vector3(currentNode.transform.position.x, transform.position.y, transform.position.z);

	}
	
	// Update is called once per frame
	void Update () 
	{
		pos = Camera.main.WorldToViewportPoint(transform.position);

		var vel = rb.velocity;
		vel.Normalize();
		vel += fallingSpeed;
		rb.velocity = vel;
	}
	public void SetFallingSpeed(float x, float y)
	{
		fallingSpeed = new Vector3(x,y,0.0f);
	}
	private void OnTriggerEnter(Collider other)
	{
		var node = other.gameObject.GetComponent<Node> ();

		if (node != null) 
		{
			SetCurrentNode(node);
		}

		var consumable = other.gameObject.GetComponent<Consumable>();

		if (consumable != null && !consumable.collected)
		{
			var type = consumable.type;
			var increaseMultiplier = false;
			switch (type) 
			{
			case Consumable.Type.Fruit:
				//get the old combo
				var oldCombo = ComboMultiplier();
				//increase the combo 
				IncreaseCombo();
				//if the combo returns a new int that means the multiplier needs to be increased
				increaseMultiplier = (oldCombo == ComboMultiplier()) ? false : true;
				//increase score
				score += consumable.value;
				transform.DOScale (transform.localScale * 1.1f, 0.05f).SetLoops(2,LoopType.Yoyo).OnComplete(() =>
				{
					transform.localScale = Vector3.one * currentSize;	
				});
				break;
			case Consumable.Type.Vegetable:
				score += consumable.value;
				//increase strikes
				GameManager.instance.Strike(consumable);
				Debug.Log("you ate the disgusting " + consumable.name);
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
			case Consumable.Type.Clover:
				GameManager.instance.DecreaseStrikes();
				IncreaseCombo();
				break;
			case Consumable.Type.Dud:
				return;
			}
			DisplayConsumableValue(consumable);
			eatenConsumables.Add(consumable);
			UserInterface.instance.MakeTextDance();
			var otherCol = (currentNode.bq.isVisible) ? currentNode.bq.colour : GameManager.instance.currentColour;

			if (increaseMultiplier)
			{
				GameManager.instance.SetGamePlayVariables (otherCol);
				increaseMultiplier = false;
			}
			consumable.Collected();
			var s = SequenceManager.instance.GetCurrentSequence();

			if (s.sequenceConsumables.Count < 1)
			{
				UserInterface.instance.PlayRandomWaveAnim().OnComplete(() => {
					UserInterface.instance.PlayRandomWaveFinish().OnComplete(() => {
						s.SpawnSequence();
					});
				});
			}
		}

	}
	public Color NextColour()
	{
		colourIndex++;

		if (colourIndex > colours.Length - 1)
			colourIndex = 0;

		colour = HelperFunctions.SetColourType (colourIndex);

		//if there is a background quad behind the node im currently at
		if (currentNode.bq.isVisible)
			GameManager.instance.SetGamePlayVariables(currentNode.bq.colour); //check against the background quad
		else
			GameManager.instance.SetGamePlayVariables (GameManager.instance.currentColour); //check against the whole background
		
		return colours [colourIndex];
	}
	public void Resize()
	{
		if (currentSize > maxSize) 
		{
			transform.DOScale(Vector3.one * 4.0f, 1.0f).SetEase(Ease.OutElastic, 1.0f,1.0f).OnComplete(() => 
			{
				UserInterface.instance.DisplayFailMessage(UserInterface.instance.GetSizeRelatedFailText(false));
				GameManager.instance.GameOver ();
				return;
			});
		} 
		else if (currentSize < minSize)
		{
			transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutElastic,1.0f,1.0f).OnComplete(() => 
			{ 
				UserInterface.instance.DisplayFailMessage(UserInterface.instance.GetSizeRelatedFailText(true));
				GameManager.instance.GameOver(); 
				return;
			});
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
		} else if (currentSize >= 0.25f && currentSize < 0.50f) {
			moveduration = 0.15f;
		} else if (currentSize >= 0.5f && currentSize < 0.75f) {
			moveduration = 0.2f;
		} else if (currentSize >= 0.75f && currentSize < 1.0f) {
			moveduration = 0.225f;
		} else if (currentSize >= 1.0f && currentSize < 1.25f) {
			moveduration = 0.25f;
		} else if (currentSize >= 1.25f && currentSize < 1.50f) {
			moveduration = 0.275f;
		} else if (currentSize >= 1.5f && currentSize < 1.75f) {
			moveduration = 0.3f;
		} else if (currentSize >= 1.75f && currentSize < 2.0f) {
			moveduration = 0.35f;
		} else if (currentSize >= 2.0f && currentSize < 2.25f) {
			moveduration = 0.4f;
		} else if (currentSize >= 2.25f && currentSize < 2.5f) {
			moveduration = 0.45f;
		} else if (currentSize >= 2.5f && currentSize < 2.75f) {
			moveduration = 0.5f;
		}
		UserInterface.instance.AdjustSizeOMeter ();
	}
	public void Reset()
	{
		score = 0;
		currentSize = midSize;
		transform.localScale = Vector3.one * currentSize;
		transform.position = Layout.instance.GetCurrentScreen().startPoint.position;
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
	public int ComboMultiplier()
	{
		if (combo < 10)
			return 1;
		else if (combo >= 10 && combo < 25)
			return 2;
		else if (combo >= 25 && combo < 50)
			return 3;
		else if (combo >= 50 && combo < 100)
			return 4;
		else
			return 5;
	}
	public void DisplayConsumableValue(Consumable c)
	{
		if (GameManager.instance.strikes < GameManager.instance.maxStrikes)
		{
			if (c.value != 0)
			{
				if (c.value > 0)
				{
					consumableValueText.text = c.value.ToString();

					consumableValueText.DOColor(Color.white,0.1f).SetDelay(0.1f).OnComplete(() => 
					{
						consumableValueText.DOColor(Color.clear,0.1f).SetDelay(0.1f);
					});
				}
				else 
				{
					DisplayInGameFailText(UserInterface.instance.inGameFailText, UserInterface.instance.inGameFailText.Length);
				}
			}
		}

	}
	public void DisplayInGameFailText(string[] textList, int range)
	{
		var rand = Random.Range(0,range);
		consumableValueText.text = textList[rand];

		consumableValueText.DOColor(Color.white,0.1f).SetDelay(0.1f).OnComplete(() => 
		{
			consumableValueText.DOColor(Color.clear,0.1f).SetDelay(0.1f);
		});
	}
	public Consumable GetLastConsumable()
	{
		return eatenConsumables.Last();
	}
	public void SetCurrentNode(Node n)
	{
		currentNode = n;
	}
}