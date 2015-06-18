using UnityEngine;
using System.Collections;

public class ButtonGameMode : MonoBehaviour {
	public GameMenu gameMenu;
	public int id;
	void OnMouseOver()
	{
		gameMenu.HoverGameMode(id);
	}
	void OnMouseExit()
	{
		gameMenu.HoverOutGameMode(id);
	}
}
