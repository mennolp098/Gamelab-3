using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IWeapon {
	private float _ammo;
	private float _reloadTime;
	private float _currentReloadTime;
	private float _shootCooldown;
	private float _currentShootCooldown;
	public virtual void Shoot()
	{

	}
	public virtual void Reload()
	{

	}
	public float ammo
	{
		set{
			_ammo = value;
		}
		get{
			return _ammo;
		}
	}
	public float reloadTime
	{
		set{
			_reloadTime = value;
		}
		get{
			return _reloadTime;
		}
	}
	public float currentReloadTime
	{
		set{
			_currentReloadTime = value;
		}
		get{
			return _currentReloadTime;
		}
	}
	public float shootCooldown
	{
		set{
			_shootCooldown = value;
		}
		get{
			return _shootCooldown;
		}
	}
	public float currentShootCooldown
	{
		set{
			_currentShootCooldown = value;
		}
		get{
			return _currentShootCooldown;
		}
	}
}
