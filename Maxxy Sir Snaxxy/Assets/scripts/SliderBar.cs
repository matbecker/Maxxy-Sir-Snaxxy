using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SliderBar : MonoBehaviour {

	public Transform[] nodes;
	public int currentIndex;
	public Consumable consumable;
	public int screenIndex;
	public bool consumableAlive;
	// Use this for initialization
	void Start () 
	{
		currentIndex = (Random.value > 0.5f) ? 0 : 1;
		transform.position = nodes[currentIndex].position;
		GoToNextNode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void GoToNextNode()
	{
		//flip the index
		currentIndex = (currentIndex == 0) ? 1 : 0;
		transform.DOMove(nodes[currentIndex].position,4.0f).SetEase(Ease.Linear).OnComplete(() => {
			var delay = Random.Range(1.0f,3.0f);
			SpawnConsumable(delay);
			GoToNextNode();
		});
	}
	public void SpawnConsumable(float delay)
	{
		if (!consumableAlive)
		{
			transform.DOScale(Vector3.one,delay).OnComplete(() => {
				var layout = Layout.instance.layouts[screenIndex];
				consumable = Instantiate(MainMenu.instance.GetRandomConsumable(), transform.position,Quaternion.Euler(0.0f,0.0f,layout.rotation)) as Consumable;
				consumable.menuSpeed = layout.fallingSpeed;
				consumableAlive = true;
			});
		}
	}
}
