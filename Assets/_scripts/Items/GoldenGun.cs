using UnityEngine;
using System.Collections;

public class GoldenGun : Gun {
	void Awake()
	{
		ammo = 1;
		maxAmmo = 1;
		range = 100;
		reloadTime = 10;
		shootCooldown = 0.5f;
		currentShootCooldown = 0;
		damage = 1000;
	}
	public override void PullTrigger ()
	{
		base.PullTrigger ();
		_networkView.RPC("DestroyThis", RPCMode.All);
	}
}
