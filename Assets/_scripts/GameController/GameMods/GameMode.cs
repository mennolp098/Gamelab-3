using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	public const string TEAMONE = "Survivors";
	public const string TEAMTWO = "Zombies";
	public const string SURVIVAL = "Survival";
	public const string HIDEANDSEEK = "Hide And Seek";

	protected List<GameObject> _allPlayers = new List<GameObject>();
	protected List<GameObject> _allZombies = new List<GameObject>();
	protected List<GameObject> _allSurvivors = new List<GameObject>();
	protected GameObject[] _gunSpawnPoints = new GameObject[4]; //Change length if there are more spawnpoints!!
	protected NetworkView _networkView;
	protected string _gameModeName;
	protected bool _gameEnded = false;
	protected GameObject _timer;

	private GameController _gameController;

	protected virtual void Awake () {
		_gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
	}
	protected virtual void Start(){
		_networkView = GetComponent<NetworkView>();
		for (int i = 0; i < _gunSpawnPoints.Length; i++) {
			_gunSpawnPoints[i] = GameObject.Find("GunSpawnPoint" + i);
		}
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

		_gameController.SetEndScreen(gameMode, teamwinner, winners);

		Invoke ("ShowEndScreen", 3f);
	}

	private void ShowEndScreen(){
		_gameController.ShowEndscreen();
	}

	public virtual void StartGameMode(){

		_timer = GameObject.Find ("Timer");
		_timer.GetComponent<Timer> ().TimerEndedEvent += EndTimer;

		GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.Player);
		foreach(GameObject player in players)
		{
			_allPlayers.Add(player);
			_allSurvivors.Add(player);
			player.GetComponent<Survivor>().SurvivorBecameZombieEvent += BecameZombie;
		}
		_timer.GetComponent<Timer> ().StartTimer ();
	}

	protected virtual void BecameZombie(GameObject player)
	{
		_allSurvivors.Remove(player);
		_allZombies.Add(player);

		if(Network.isServer)
			CheckZombieWin();
	}
	protected virtual void CheckZombieWin()
	{
		if(_allSurvivors.Count == 0)
		{
			ZombiesWon();
		}
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
