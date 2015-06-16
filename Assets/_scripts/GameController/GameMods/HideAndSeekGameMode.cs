using UnityEngine;
using System.Collections;

public class HideAndSeekGameMode : GameMode {
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
