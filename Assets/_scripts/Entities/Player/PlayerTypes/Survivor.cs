﻿using UnityEngine;
using System.Collections;

public class Survivor : PlayerType {
	private Gun _gunComponent;

	public const string TURN_ZOMBIE_ANIM = "TurnZombie";

	public delegate void GameObjectInfoDelegate(GameObject info);
	public event GameObjectInfoDelegate SurvivorBecameZombieEvent;

	protected override void ChangePlayerStats (){
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load("Sprites/PlayerArt/Survivor") as Sprite;// <---- voor de correcte sprite (oud idee).
		//TODO De animator moet verandert worden als je in een zombie verandert niet de sprite van de animator..
		//GetComponent<Collider2D> ().bounds.size = new Vector3 (2, 4, 1); <---- voor de correcte box collision.

		_animator.runtimeAnimatorController = Resources.Load("Art/Animators/SurviverAnimator") as RuntimeAnimatorController;
		_player.healthPoints = 50; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 3;
		_player.runSpeed = 5;
		_player.condition = 1;
		_player.maxStamina = 10;
		_currentStatus = "Survivor";

		base.ChangePlayerStats ();
	}

	//TODO Animatie function die becomeZombie aanroept
	public void BecomeZombie(){
		BroadcastMessage ("PlayAnimation",TURN_ZOMBIE_ANIM);
		lockAnim ();
	}

	private void Update()
	{
		if(_networkView.isMine)
		{
			if(Input.GetMouseButton(0))
			{
				if(_gunComponent == null && GetComponent<Gun>())
				{
					_gunComponent = GetComponent<Gun>();
					_gunComponent.PullTrigger();
				}
				else if(_gunComponent != null)
				{
					_gunComponent.PullTrigger();
				}
			}
		}
	}
	public void BecomeZombieCallNetwork()
	{
		if(_networkView.isMine)
		{
			float[] shakeParameters = new float[3];
			float shakeAmount = 2;
			float shakeIntensity = 0.5f;
			float shakeSpeed = 0.1f;
			
			shakeParameters[0] = shakeAmount;
			shakeParameters[1] = shakeIntensity;
			shakeParameters[2] = shakeSpeed;
			
			SendMessage("Shake", shakeParameters);

			_networkView.RPC ("NetworkBecomeZombie", RPCMode.All);
		}
	}
	[RPC]
	private void NetworkBecomeZombie()
	{
		gameObject.AddComponent<Zombie> (); //<-- check met de component niet met de tag. Tag is en blijft "Player" voor het systeem
		if (SurvivorBecameZombieEvent != null) {
			SurvivorBecameZombieEvent(this.gameObject);
		}
		Destroy (this);
	}

	[RPC]
	private void PickupGun()
	{
		if(GetComponent<Pistol>() == null)
		{
			gameObject.AddComponent<Pistol>();
		}
	}
	[RPC]
	private void PickupGoldenGun()
	{
		if(GetComponent<GoldenGun>() == null)
		{
			gameObject.AddComponent<GoldenGun>();
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == Tags.Gun)
		{
			_networkView.RPC("PickupGun", RPCMode.All);
			if(Network.isServer)
				Network.Destroy(other.gameObject);
		}
		else if(other.transform.tag == Tags.GoldenGun)
		{
			_networkView.RPC("PickupGoldenGun", RPCMode.All);
			if(Network.isServer)
				Network.Destroy(other.gameObject);
		}
	}
	public override string ConvertAnimationName (string animName, bool trueName)
	{
		string animNameToReturn = base.ConvertAnimationName (animName, trueName);
		
		if (GetComponent<Gun> () != null) {
			if(animName != TURN_ZOMBIE_ANIM){
				animNameToReturn = "Gun" + animNameToReturn;
			}
		}
		return animNameToReturn;
	}
	private void Shooting()
	{
		BroadcastMessage("PlayAnimation", ATTACK_ANIM);
		lockAnim ();
	}
}
