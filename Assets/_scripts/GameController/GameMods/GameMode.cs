using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	public const string TEAMONE = "Survivors";
	public const string TEAMTWO = "Zombies";

	protected List<GameObject> _allPlayers = new List<GameObject>();
	protected List<GameObject> _allZombies = new List<GameObject>();
	protected List<GameObject> _allSurvivors = new List<GameObject>();
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

	protected virtual void BecameZombie(GameObject player)
	{
		_allSurvivors.Remove(player);
		_allZombies.Add(player);
	}
	protected virtual void ZombiesWon()
	{
		string winners = "";
		foreach(GameObject player in _allPlayers)
		{
			if(player.GetComponent<Zombie>())
			{
				winners += player.GetComponent<Player>().usernameText + " ";
			}
		}
		_networkView.RPC("EndGame", RPCMode.All, _gameModeName, TEAMTWO, winners);
	}
	protected virtual void SurvivorsWon()
	{
		string winners = "";
		foreach(GameObject player in _allPlayers)
		{
			if(player.GetComponent<Survivor>())
			{
				winners += player.GetComponent<Player>().usernameText + " ";
			}
		}
		_networkView.RPC("EndGame", RPCMode.All, _gameModeName, TEAMONE, winners);
	}
}
