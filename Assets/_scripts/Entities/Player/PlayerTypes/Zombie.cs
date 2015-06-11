using UnityEngine;
using System.Collections;

public class Zombie : PlayerType {

	protected override void ChangePlayerStats () {
		//TODO GetComponent<SpriteRenderer> ().sprite = Resources.Load (Zombiesprite); <----
		_animator.runtimeAnimatorController = playerTypeAnimatorController;
		_player.healthPoints = 200; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 2;
		_player.runSpeed = 3;
		_player.condition = 5;
		_player.maxStamina = 5;
		_currentStatus = "Zombie";

		base.ChangePlayerStats ();
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
		_animator.Play (animationToPlay);
	}
}
