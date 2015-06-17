using UnityEngine;
using System.Collections;

public class HideAndSeekGameMode : GameMode {
	private GameObject _goldenGunPrefab;
	protected override void Start()
	{
		base.Start();
		_gameModeName = "Hide And Seek";
		_goldenGunPrefab = Resources.Load("Prefabs/goldenGunPrefab", typeof(GameObject)) as GameObject;
	}
	public override void StartGameMode ()
	{
		base.StartGameMode ();
		if(Network.isServer)
		{
			_allPlayers[(int)Random.Range(0,_allPlayers.Count)].GetComponent<Survivor>().BecomeZombie();
			Invoke("SpawnGoldenGun", Random.Range(1,60));
		}
	}
	private void SpawnGoldenGun()
	{
		Network.Instantiate(_goldenGunPrefab,_gunSpawnPoints[Random.Range(0,_gunSpawnPoints.Length)].transform.position, Quaternion.identity, 0);
		Invoke("SpawnGoldenGun", Random.Range(1,60));
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
