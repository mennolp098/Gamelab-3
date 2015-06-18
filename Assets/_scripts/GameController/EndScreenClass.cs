using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScreenClass : MonoBehaviour {

	public Text winnerText;
	public Sprite humansWinGraph;
	public Sprite zombiesWinGraph;


	public void ChangeText(string winningTeam, string winningPlayerUserN = ""){

		if(winningTeam == GameMode.TEAMONE)
		{
			SetBackground(humansWinGraph);
			winnerText.text = "Winning Team: " + winningTeam + "\n";
			if (winningPlayerUserN != "") 
			{
				winnerText.text += " Winning players: " + winningPlayerUserN;
			}
		} 
		else if(winningTeam == GameMode.TEAMTWO)
		{
			SetBackground(zombiesWinGraph);
			winnerText.text = "";
		}
	}
	private void SetBackground(Sprite background)
	{
		GetComponent<Image>().sprite = background;
	}
	void Update()
	{
		if(Input.anyKeyDown)
		{
			Network.Disconnect();
			Application.LoadLevel(0);
		}
		if(Input.GetMouseButtonDown(0))
		{
			Network.Disconnect();
			Application.LoadLevel(0);
		}
	}
}
