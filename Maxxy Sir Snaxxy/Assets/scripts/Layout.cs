using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Layout : MonoBehaviour {

	public static Layout instance;
	public Canvas canvas;
	public enum ScreenState { Left, Right, Top, Bottom };
	public BoxCollider[] colliders;

	[System.Serializable]
	public class Screen
	{
		public Transform startPoint;
		public ScreenState state;
		public Node[] nodes;
		public Transform[] columns;
		public Transform csp;
		public float rotation;
	}
	public Screen[] layouts;
	public int screenIndex;
	public ScreenState currentLayout;

	void Awake()
	{
		if (!instance)
			instance = this;

		screenIndex = 0;

	}
	void Update()
	{
//		if (GameManager.instance.inGame && UserInterface.instance.intermission)
//		{
//			var c = Character.instance;
//
//			if (Input.deviceOrientation == DeviceOrientation.Portrait)
//			{
//				if (currentLayout == ScreenState.Bottom)
//				{
//					return;
//				}
//				else
//				{
//					currentLayout = ScreenState.Bottom;
//					SetLayout();
//					return;
//				}
//			}
//			else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
//			{
//				if (currentLayout == ScreenState.Right)
//				{
//					return;
//				}
//				else
//				{
//					currentLayout = ScreenState.Right;
//					SetLayout();
//					return;
//				}
//			}
//			else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
//			{
//				if (currentLayout == ScreenState.Top)
//				{
//					return;
//				}
//				else
//				{
//					currentLayout = ScreenState.Top;
//					SetLayout();
//					return;
//				}
//			}
//			else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
//			{
//				if (currentLayout == ScreenState.Left)
//				{
//					return;
//				}
//				else
//				{
//					currentLayout = ScreenState.Left;
//					SetLayout();
//					return;
//				}
//			}
//		}
	}


	public Node GetNextNode()
	{
		//increase the node index
		var newIndex = Character.instance.currentNode.index + 1;
		//stay in colliders of array
		if (newIndex >= GetCurrentScreen ().nodes.Length - 1)
			newIndex = GetCurrentScreen ().nodes.Length - 1;
		
		//return the new node
		return GetCurrentScreen ().nodes [newIndex];
	}

	public Node GetPrevNode()
	{
		var newIndex = Character.instance.currentNode.index - 1;

		if (newIndex <= 0)
			newIndex = 0;

		return GetCurrentScreen ().nodes [newIndex];
	}
	public void PickRandomLayout(bool first)
	{
		var rand = Random.Range(0, 10000);

		if (rand < 5000 || first)
		{
			currentLayout = ScreenState.Bottom;
			screenIndex = 0;
		}
		else if (rand >= 5000 && rand < 7000)
		{
			currentLayout = ScreenState.Top;
			screenIndex = 2;
		}
		else if (rand >= 7000 && rand < 9000)
		{
			currentLayout = ScreenState.Right;
			screenIndex = 1;
		}
		else
		{
			currentLayout = ScreenState.Left;
			screenIndex = 3;
		}
	}

	public Screen GetCurrentScreen()
	{
		return layouts [screenIndex];
	}

	public Vector3 GetBounds()
	{
		return GetCurrentScreen().csp.position;
	}
	public void Adjustcolliders()
	{
		switch(currentLayout)
		{
		case ScreenState.Bottom:
			colliders[0].gameObject.SetActive(true);
			colliders[1].gameObject.SetActive(false);
			colliders[2].gameObject.SetActive(false);
			colliders[3].gameObject.SetActive(false);
			break;
		case ScreenState.Right:
			colliders[0].gameObject.SetActive(true);
			colliders[1].gameObject.SetActive(true);
			colliders[2].gameObject.SetActive(true);
			colliders[3].gameObject.SetActive(false);
			break;
		case ScreenState.Top:
			colliders[0].gameObject.SetActive(false);
			colliders[1].gameObject.SetActive(false);
			colliders[2].gameObject.SetActive(true);
			colliders[3].gameObject.SetActive(false);
			break;
		case ScreenState.Left:
			colliders[0].gameObject.SetActive(true);
			colliders[1].gameObject.SetActive(false);
			colliders[2].gameObject.SetActive(true);
			colliders[3].gameObject.SetActive(true);
			break;
		}
	}

	public void SetLayout(bool random)
	{
		if (random)
			PickRandomLayout(false);
		else
			PickRandomLayout(true);
		
		Adjustcolliders();
		var c = Character.instance;
		c.transform.DOScale(Vector3.zero,0.5f).OnComplete(() => {
			switch (currentLayout)
			{
			case ScreenState.Bottom:
				c.transform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
				c.SetFallingSpeed(0.0f,-1.0f);
				break;
			case ScreenState.Right:
				c.transform.DORotate(new Vector3(0.0f,0.0f,90.0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
				c.SetFallingSpeed(1.0f,0.0f);
				break;
			case ScreenState.Top:
				c.transform.DORotate(new Vector3(0.0f,0.0f,180.0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
				c.SetFallingSpeed(0.0f,1.0f);
				break;
			case ScreenState.Left:
				c.transform.DORotate(new Vector3(0.0f,0.0f,270.0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
				c.SetFallingSpeed(-1.0f,0.0f);
				break;
			}
			foreach (Node n in GetCurrentScreen().nodes)
			{
				n.gameObject.SetActive(true);
			}
			SequenceManager.instance.AdjustSequenceSpeed(0.0f);
			c.transform.DOMove(GetCurrentScreen().startPoint.position, 0.0f).OnComplete(() => {
				c.transform.DOScale(Vector3.one * c.currentSize, 0.5f).OnComplete(() => {
					c.rb.velocity = Vector3.zero;
					c.rb.constraints = (HelperFunctions.IsEven(screenIndex)) ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.FreezePositionX;
				});
			});
		});
	}

	// Use this for initialization
	void Start () 
	{
		foreach (Screen s in layouts)
		{
			foreach (Node n in s.nodes)
			{
				//disable all nodes
				n.gameObject.SetActive(false);
			}
		}
	}
	public void Init()
	{
		SetLayout(false);
	}
}
