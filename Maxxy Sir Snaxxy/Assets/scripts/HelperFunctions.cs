using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions {

	public static GameManager.ColourType SetColourType(int index)
	{
		var colour = GameManager.ColourType.Purple;
		switch (index) 
		{
		case 0:
			colour = GameManager.ColourType.Purple;
			break;
		case 1:
			colour = GameManager.ColourType.Blue;
			break;
		case 2:
			colour = GameManager.ColourType.Green;
			break;
		case 3:
			colour = GameManager.ColourType.Yellow;
			break;
		case 4:
			colour = GameManager.ColourType.Orange;
			break;
		case 5:
			colour = GameManager.ColourType.Red;
			break;
		default:
			break;
		}
		return colour;
	}
	public static bool CompareColour(GameManager.ColourType a, GameManager.ColourType b)
	{
		if (a == b)
			return true;
		else
			return false;
	}
}
