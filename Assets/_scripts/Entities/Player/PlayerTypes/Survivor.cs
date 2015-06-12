using UnityEngine;
using System.Collections;

public class Survivor : PlayerType {
	private Gun _gunComponent;

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
	private void Update()
	{
		if(_networkView.isMine)
		{
			if(Input.GetMouseButton(0))
			{
				if(_gunComponent == null && GetComponent<Gun>())
				{
					_gunComponent = GetComponent<Gun>();
				}
				if(_gunComponent != null)
				{
					_gunComponent.PullTrigger();
				}
			}
		}
	}
	public void BecomeZombie()
	{
		GetComponent<NetworkView>().RPC("NetworkBecomeZombie",RPCMode.All);
	}
	[RPC]
	private void NetworkBecomeZombie()
	{
		gameObject.AddComponent<Zombie> (); //<-- check met de component niet met de tag. Tag is en blijft "Player" voor het systeem
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

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == Tags.Gun)
		{
			_networkView.RPC("PickupGun", RPCMode.All);
		}
	}

	[RPC]
	protected override void PlayAnimationNetwork (string animation)
	{
		base.PlayAnimationNetwork (animation);
		_animator.speed = 1;
		string animationToPlay = animation;
		if (animationToPlay == PlayerType.RUN_ANIM) {
			_animator.speed = 2;
			animationToPlay = PlayerType.WALK_ANIM;
		}
		if (GetComponent<Gun> () != null) {
			animationToPlay = "Gun" + animationToPlay;
		}

		_animator.Play (animationToPlay);
	}
	private void Shooting()
	{
		_networkView.RPC("PlayAnimationNetwork", RPCMode.All, "Shoot");
	}
}
