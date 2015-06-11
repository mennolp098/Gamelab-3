
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IWeapon {
	private NetworkView _networkView;
	private float _ammo;
	private float _maxAmmo;
	private float _reloadTime;
	private float _shootCooldown;
	private float _currentShootCooldown;
	private float _range;
	private float _damage;
	private bool _isReloading = false;
	void Start()
	{
		_networkView = GetComponent<NetworkView>();
	}
	public virtual void PullTrigger()
	{
		if(_ammo != 0 && _currentShootCooldown <= Time.time)
		{
			Debug.Log("Shooting!");
			_networkView.RPC("Shoot", RPCMode.Server);
			_currentShootCooldown = Time.time + _shootCooldown;
		} 
		else 
		{
			Reload();
		}
	}
	[RPC]
	public virtual void Shoot()
	{
		_ammo--;
		//if(Network.isServer)
		//{
			Debug.Log("PANG!");
			Vector3 direction = transform.TransformDirection(Vector3.forward);
			int range = 10;
			RaycastHit hit;
			if (Physics.Raycast(transform.position, direction, out hit, range))
			{
				if(hit.transform.tag == Tags.Player)
				{
					//TODO: do dmg
					hit.transform.GetComponent<Health>().AddSubHealth(-damage);
					Debug.Log("AUWO! ---> "  + hit.transform.name);
				}
			}
		//}
	}
	public virtual void Reload()
	{
		if(!_isReloading)
		{
			_isReloading = true;
			Invoke ("AddAmmo", _reloadTime);
		}
	}
	protected virtual void AddAmmo()
	{
		ammo = maxAmmo;
		_isReloading = false;
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
	public float range{
		set{
			_range = value;
		}
		get {
			return _range;
		}
	}
	public float damage{
		set{
			_damage = value;
		}
		get {
			return _damage;
		}
	}
	public float maxAmmo{
		set{
			_maxAmmo = value;
		}
		get {
			return _maxAmmo;
		}
	}
}
