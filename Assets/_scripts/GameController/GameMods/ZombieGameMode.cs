using UnityEngine;
using System.Collections;

public class ZombieGameMode : GameMode {
	private GameObject[] gunSpawnPoints = new GameObject[4]; //Change length if there are more spawnpoints!!
	private GameObject gunPrefab;
	protected override void Start()
	{
		base.Start();
		_gameModeName = "Survival";
		gunPrefab = Resources.Load("Prefabs/gunPrefab", typeof(GameObject)) as GameObject;
		for (int i = 0; i < gunSpawnPoints.Length; i++) {
			gunSpawnPoints[i] = GameObject.Find("GunSpawnPoint" + i);
		}
	}

	public override void StartGameMode ()
	{
		base.StartGameMode ();
		if(Network.isServer)
		{
			_allPlayers[(int)Random.Range(0,_allPlayers.Count)].GetComponent<Survivor>().BecomeZombie();
			foreach(GameObject player in _allPlayers)
			{
				player.GetComponent<Health>().NoHealthLeftEvent += CheckPlayers;
			}
		}
	}
	protected override void EndTimer ()
	{
		base.EndTimer ();
		if(Network.isServer)
			SpawnGuns();
	}
	private void SpawnGuns()
	{
		for (int i = 0; i < gunSpawnPoints.Length; i++) 
		{
			Network.Instantiate(gunPrefab,gunSpawnPoints[i].transform.position,Quaternion.identity, 1);
		}
	}
	private void CheckPlayers (GameObject playerDied)
	{
		if(playerDied.GetComponent<Survivor>())
		{
			_allSurvivors.Remove(playerDied);
			if(_allSurvivors.Count == 0)
			{
				ZombiesWon();
			}
		} 
		else if(playerDied.GetComponent<Zombie>())
		{
			_allZombies.Remove(playerDied);
			if(_allZombies.Count == 0)
			{
				SurvivorsWon();
			}
		}
	}
}
