using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout : MonoBehaviour {

	public static Layout instance;
	public Canvas canvas;
	public enum ScreenState { Left, Right, Top, Bottom };
	public Transform[] bounds;

	[System.Serializable]
	public class Screen
	{
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

		currentLayout = layouts [screenIndex].state;
	}
	public Node GetNextNode()
	{
		//increase the node index
		var newIndex = Character.instance.currentNodeIndex + 1;
		//stay in bounds of array
		if (newIndex >= GetCurrentScreen ().nodes.Length - 1)
			newIndex = GetCurrentScreen ().nodes.Length - 1;
		
		//return the new node
		return GetCurrentScreen ().nodes [newIndex];
	}
	public Node GetPrevNode()
	{
		var newIndex = Character.instance.currentNodeIndex - 1;

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
	// Use this for initialization
	void Start () 
	{

	}
}
