
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IWeapon {
	private GameObject _muzzleFlare;

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
		_muzzleFlare = Resources.Load("Prefabs/MuzzleFlare", typeof(GameObject)) as GameObject;
	}
	public virtual void PullTrigger()
	{
		if(_ammo != 0 && _currentShootCooldown <= Time.time)
		{
			SendMessage("Shooting");

			float[] shakeParameters = new float[3];
			float shakeAmount = 2;
			float shakeIntensity = 0.5f;
			float shakeSpeed = 0.1f;

			shakeParameters[0] = shakeAmount;
			shakeParameters[1] = shakeIntensity;
			shakeParameters[2] = shakeSpeed;

			SendMessage("Shake", shakeParameters);

			if(Network.isClient)
			{
				_networkView.RPC("Shoot", RPCMode.Server);
			} else {
				Shoot();
			}
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
		Vector3 muzzleFlarePos = this.transform.position;
		muzzleFlarePos.z = -1f;
		Network.Instantiate(_muzzleFlare,muzzleFlarePos,_muzzleFlare.transform.rotation, 1);
		//Debug.Log("PANG!");
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, range);
		Debug.DrawRay(transform.position,transform.up, Color.red, 1);
		foreach(RaycastHit2D hit in hits)
		{
			if(hit.transform.tag == Tags.Player && hit.transform != this.transform)
			{
				Debug.Log("Hitting a player: " + hit.transform.name);
				hit.transform.GetComponent<Health>().AddSubHealth(-damage);
				break;
			}
		}
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
