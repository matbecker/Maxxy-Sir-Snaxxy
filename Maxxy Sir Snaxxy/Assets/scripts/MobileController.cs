using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MobileController : MonoBehaviour {

	[SerializeField] Character character;
	public Vector2 touchPos;
	public bool isMoving;
	public bool isFlipping;
	Tween moveTween;
	Tween spinTween;
	// Use this for initialization
	void Start () 
	{
		//maybe?
		Input.multiTouchEnabled = false;	
		moveTween = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0) && !GameManager.instance.gameover) 
		{
			if (isMoving)
				return;
			
			touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				
				if (hit.rigidbody != null && !isFlipping) 
				{
					if (hit.rigidbody.tag == "Player") 
					{
						character.GetComponentInChildren<MeshRenderer> ().material.DOColor (character.NextColour (), 0.75f);
						character.transform.DORotate (new Vector3 (360.0f, 0.0f, character.transform.rotation.eulerAngles.z), 0.3f, RotateMode.FastBeyond360).OnComplete (() => {
							isFlipping = false;			
						});
						isFlipping = true;
						return;
					} 
					else
						return;
				}
			}
				
			if (moveTween != null) 
			{
				moveTween.Kill (false);
				moveTween = null;
			}
			if (spinTween != null) 
			{
				spinTween.Kill (true);
				spinTween = null;
			}

			
			Vector3 newPos = Vector3.zero;

			if (Layout.instance.currentLayout == Layout.ScreenState.Bottom || Layout.instance.currentLayout == Layout.ScreenState.Top)
			{
				//var nodePos = Camera.main.ScreenToViewportPoint(Character.instance.currentNode.transform.position);
				if (touchPos.x < 0.5f) 
				{
					if (character.currentNode.index == 0) 
						return;

					newPos = Layout.instance.GetPrevNode ().transform.position;
				} 
				if (touchPos.x > 0.5f) 
				{
					if (character.currentNode.index == Layout.instance.GetCurrentScreen ().nodes.Length - 1)
						return;

					newPos = Layout.instance.GetNextNode ().transform.position;
				}
			}
			else
			{
				
			}
			isMoving = true;
			moveTween = character.transform.DOMoveX(newPos.x, character.moveduration).OnComplete(() => { 
				isMoving = false; 
				Character.instance.StopMoving();
			});
		}

	}
}
