using UnityEngine;
using System.Collections;

public class Pistol : Gun {
	void Awake()
	{
		ammo = 7;
		reloadTime = 2;
		shootCooldown = 0.5f;
		currentShootCooldown = 0;
	}
}
