using UnityEngine;
using System.Collections;

public class HideAndSeekGameMode : GameMode {
	protected override void Start()
	{
		base.Start();
		_gameModeName = "Hide And Seek";
	}
	public override void StartGameMode ()
	{
		base.StartGameMode ();
		if(Network.isServer)
		{
			_allPlayers[(int)Random.Range(0,_allPlayers.Count)].GetComponent<Survivor>().BecomeZombie();
		}
	}
	protected override void EndTimer ()
	{
		base.EndTimer ();
		if(Network.isServer)
		{
			CheckSurvivors();
		}
	}
	private void CheckSurvivors()
	{
		if(_allSurvivors.Count != 0)
		{
			SurvivorsWon();
		} 
		else 
		{
			ZombiesWon();
		}
	}
}
