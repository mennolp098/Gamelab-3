using UnityEngine;
using System.Collections;

public class ZombieGamemMode : GameMode {
	public override void StartGameMode ()
	{
		base.StartGameMode ();
		_allPlayers[Random.Range(0,_allPlayers.Count)].GetComponent<Survivor>().BecomeZombie();
	}
	protected override void EndTimer ()
	{
		base.EndTimer ();
		//TODO: make survivor component and uncomment function V
		/*
		foreach(Player player in _allPlayers)
		{

			if(player.GetComponent<Survivor>())
			{
				SurvivorComponent = player.GetComponent<Survivor>();
				SurvivorComponent.GetWeapon();
			}
		}
		*/
	}
}
