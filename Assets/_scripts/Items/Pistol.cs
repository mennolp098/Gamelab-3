using UnityEngine;
using System.Collections;

public class Pistol : Gun {
	void Awake()
	{
		ammo = 7;
		maxAmmo = 7;
		reloadTime = 10;
		shootCooldown = 0.5f;
		currentShootCooldown = 0;
		damage = 10;
	}
}
