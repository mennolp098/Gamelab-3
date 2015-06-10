using UnityEngine;
using System.Collections;

public interface IWeapon {
	void Shoot();
	void PullTrigger();
	void Reload();
	float ammo{
		set;
		get;
	}
	float reloadTime{
		set;
		get;
	}
	float shootCooldown{
		set;
		get;
	}
	float currentShootCooldown{
		set;
		get;
	}
	float range{
		get;
		set;
	}
}
