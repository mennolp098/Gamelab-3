
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IWeapon {
	private NetworkView _networkView;
	private float _ammo;
	private float _reloadTime;
	private float _shootCooldown;
	private float _currentShootCooldown;
	private float _range;
	void Start()
	{
		_networkView = GetComponent<NetworkView>();
	}
	public virtual void PullTrigger()
	{
		if(_ammo != 0)
		{
			if(_currentShootCooldown < Time.time)
			{
				_networkView.RPC("Shoot", RPCMode.Server);
				_currentShootCooldown = Time.time + _shootCooldown;
			}
		} 
		else 
		{
			Reload();
		}
	}
	[RPC]
	public virtual void Shoot()
	{
		if(Network.isServer)
		{
			Vector3 direction = transform.TransformDirection(Vector3.forward);
			int range = 10;
			RaycastHit hit;
			if (Physics.Raycast(transform.position, direction, out hit, range))
			{
				if(hit.transform.tag == Tags.Player)
				{
					//TODO: do dmg
				}
			}
		}
	}
	public virtual void Reload()
	{
		Invoke ("AddAmmo", _reloadTime);
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
}
