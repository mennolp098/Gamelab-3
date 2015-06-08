using UnityEngine;
using System.Collections;

public class Survivor : PlayerType {

	protected override void ChangePlayerStats (){
		base.ChangePlayerStats ();

		_player.healthPoints = 50; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 3;
		_player.runSpeed = 4;
	}

	public void BecomeZombie()
	{
		GetComponent<NetworkView>().RPC("NetworkBecomeZombie",RPCMode.All);
	}
	[RPC]
	private void NetworkBecomeZombie()
	{
		//TODO: Change sprite;
		//TODO: Add Zombie Collision;
		//TODO: Change Health;
		//TODO  Add zombie component..
		//this.transform.tag = Tags.Zombie;
	}
}
