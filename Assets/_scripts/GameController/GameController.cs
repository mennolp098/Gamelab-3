using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject endScreen;
	public GameObject countDown;
	private Text _countDownText;
	private int _counter = 30;
	void CountDown()
	{
		if(_countDownText == null)
		{
			_countDownText = countDown.GetComponentInChildren<Text>();
		}
		if(_counter != 0)
		{
			_counter--;
			_countDownText.text = _counter.ToString();
			Invoke("CountDown", 1f);
		} else {
			countDown.SetActive(false);
			StartGameMode();
		}
	}

	public void BackToMenu(){
		Application.LoadLevel ("Menu");
	}

	public void SetEndScreen(string gameMod, string winningTeam, string winnerUsername = ""){
		endScreen.GetComponent<EndScreenClass> ().ChangeText (winningTeam, winnerUsername);
	}

	public void ShowEndscreen(){
		endScreen.SetActive (true);
	}

	public void StartGame(){
		CountDown();
	}
	private void StartGameMode()
	{
		GetComponent<GameMode> ().StartGameMode();
	}
}
