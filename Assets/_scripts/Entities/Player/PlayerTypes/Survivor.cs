using UnityEngine;
using System.Collections;

public class Survivor : PlayerType {

	protected override void ChangePlayerStats (){
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load("Sprites/PlayerArt/Survivor") as Sprite;// <---- voor de correcte sprite (oud idee).
		//TODO De animator moet verandert worden als je in een zombie verandert niet de sprite van de animator..
		//GetComponent<Collider2D> ().bounds.size = new Vector3 (2, 4, 1); <---- voor de correcte box collision.

		_animator.runtimeAnimatorController = playerTypeAnimatorController;
		_player.healthPoints = 50; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 3;
		_player.runSpeed = 5;
		_player.condition = 1;
		_player.maxStamina = 10;

		base.ChangePlayerStats ();
	}

	public void BecomeZombie()
	{
		GetComponent<NetworkView>().RPC("NetworkBecomeZombie",RPCMode.All);
	}
	[RPC]
	private void NetworkBecomeZombie()
	{
		//TODO: Change sprite;
		//TODO: Add Zombie Collision;
		//this.transform.tag = Tags.Zombie;

		gameObject.AddComponent<Zombie> (); //<-- check met de component niet met de tag. Tag is en blijft "Player" voor het systeem
		Destroy (this);
	}
	[RPC]
	protected override void PlayAnimation (string animation)
	{
		base.PlayAnimation (animation);
		_animator.speed = 1;
		string animationToPlay = animation;
		if (animationToPlay == PlayerType.RUN_ANIM) {
			_animator.speed = 2;
			animationToPlay = PlayerType.WALK_ANIM;
		}
		if (GetComponent<Shoot> () != null) {
			animationToPlay = "Gun" + animationToPlay;
		}

		_animator.Play (animationToPlay);
	}

}
