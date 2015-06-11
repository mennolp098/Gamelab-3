using UnityEngine;
using System.Collections;

public class ZombieGamemMode : GameMode {
	public GameObject[] gunSpawnPoints = new GameObject[0];
	public GameObject gunPrefab;

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
			SpawnGuns();
	}
	private void SpawnGuns()
	{
		for (int i = 0; i < gunSpawnPoints.Length; i++) 
		{
			Network.Instantiate(gunPrefab,gunSpawnPoints[i].transform.position,Quaternion.identity, 1);
		}
	}
}
