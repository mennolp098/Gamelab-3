using UnityEngine;
using System.Collections;

public class ZombieGameMode : GameMode {
	protected override void Start()
	{
		base.Start();
		_gameModeName = "Survival";
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

		//every client and the server will give the survivor a pistol component
		foreach (GameObject survivor in _allSurvivors) 
		{
			survivor.AddComponent<Pistol>();
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
