using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	public const string TEAMONE = "Survivors";
	public const string TEAMTWO = "Zombies";

	protected List<GameObject> _allPlayers = new List<GameObject>();
	protected NetworkView _networkView;
	protected string _gameModeName;
	protected bool _gameEnded = false;
	protected GameObject _timer;

	protected virtual void Awake () {

	}
	protected virtual void Start(){
		_networkView = GetComponent<NetworkView>();
	}
	protected virtual void Update()
	{
	}
	protected virtual void EndTimer () {
		//TODO if timer hits end then give all survivers a gun.
	}

	[RPC]
	protected virtual void EndGame(string gameMode, string teamwinner, string winners)
	{
		_timer.GetComponent<Timer> ().PauseTimer ();

		Invoke ("ShowEndScreen", 3f);
	}

	private void ShowEndScreen(){
		GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>().ShowEndscreen();
	}

	public virtual void StartGameMode(){

		_timer = GameObject.Find ("Timer");
		_timer.GetComponent<Timer> ().TimerEndedEvent += EndTimer;

		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.Player);
		foreach(GameObject player in players)
		{
			_allPlayers.Add(player);
		}
		_timer.GetComponent<Timer> ().StartTimer ();
	}
}
