using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Layout : MonoBehaviour {

	public static Layout instance;
	public Canvas canvas;
	public enum ScreenState { Left, Right, Top, Bottom };
	public Transform[] bounds;

	[System.Serializable]
	public class Screen
	{
		public Transform startPoint;
		public ScreenState state;
		public Node[] nodes;
		public Transform[] columns;
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
		if (Input.deviceOrientation == DeviceOrientation.Portrait)
		{
			if (currentLayout == ScreenState.Bottom)
			{
				return;
			}
			else
			{
				currentLayout = ScreenState.Bottom;
				SetLayout();
				return;
			}
		}
		else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
		{
			if (currentLayout == ScreenState.Right)
			{
				return;
			}
			else
			{
				currentLayout = ScreenState.Right;
				SetLayout();
				return;
			}
		}
		else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
		{
			if (currentLayout == ScreenState.Top)
			{
				return;
			}
			else
			{
				currentLayout = ScreenState.Top;
				SetLayout();
				return;
			}
		}
		else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
		{
			if (currentLayout == ScreenState.Left)
			{
				return;
			}
			else
			{
				currentLayout = ScreenState.Left;
				SetLayout();
				return;
			}
		}
	}
	public Node GetNextNode()
	{
		//increase the node index
		var newIndex = Character.instance.currentNode.index + 1;
		//stay in bounds of array
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

	public Screen GetCurrentScreen()
	{
		return layouts [screenIndex];
	}
	public Rect GetBounds()
	{
		Rect r = new Rect(bounds[3].transform.position.x, bounds[0].transform.position.y, bounds[1].transform.position.x, bounds[2].transform.position.y);
		return r;
			
	}
	public void AdjustBounds()
	{
		foreach (Transform b in bounds)
		{
			b.gameObject.SetActive(false);
		}
		switch(currentLayout)
		{
		case ScreenState.Bottom:
			bounds[0].gameObject.SetActive(true);
			break;
		case ScreenState.Right:
			bounds[1].gameObject.SetActive(true);
			break;
		case ScreenState.Top:
			bounds[2].gameObject.SetActive(true);
			break;
		case ScreenState.Left:
			bounds[3].gameObject.SetActive(true);
			break;
		}
	}
	public void SetLayout()
	{
		AdjustBounds();

		switch (currentLayout)
		{
		case ScreenState.Bottom:
			screenIndex = 0;
			Character.instance.SetFallingSpeed(0.0f,-1.0f);
			Character.instance.transform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
			break;
		case ScreenState.Right:
			screenIndex = 1;
			Character.instance.SetFallingSpeed(1.0f,0.0f);
			Character.instance.transform.DORotate(new Vector3(0.0f,0.0f,90.0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
			break;
		case ScreenState.Top:
			screenIndex = 2;
			Character.instance.SetFallingSpeed(0.0f,1.0f);
			Character.instance.transform.DORotate(new Vector3(0.0f,0.0f,180.0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
			break;
		case ScreenState.Left:
			screenIndex = 3;
			Character.instance.SetFallingSpeed(-1.0f,0.0f);
			Character.instance.transform.DORotate(new Vector3(0.0f,0.0f,270.0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutBack,1.0f,0.5f);
			break;
		}
		foreach (Node n in GetCurrentScreen().nodes)
		{
			n.gameObject.SetActive(true);
		}
		SequenceManager.instance.AdjustSequenceSpeed(0.0f);
		Character.instance.transform.DOMove(GetCurrentScreen().startPoint.position, 0.5f);

	}
	// Use this for initialization
	void Start () 
	{
		foreach (Screen s in layouts)
		{
			foreach (Node n in s.nodes)
			{
				n.gameObject.SetActive(false);
			}
		}
		SetLayout();
	}
}
