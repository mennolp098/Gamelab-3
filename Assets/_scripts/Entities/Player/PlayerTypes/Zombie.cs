using UnityEngine;
using System.Collections;

public class Zombie : PlayerType {

	protected override void ChangePlayerStats () {
		//TODO GetComponent<SpriteRenderer> ().sprite = Resources.Load (Zombiesprite); <----
		_animator.runtimeAnimatorController = Resources.Load("Art/Animators/ZombieAnimator") as RuntimeAnimatorController;
		_player.healthPoints = 200; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 2;
		_player.runSpeed = 3;
		_player.condition = 5;
		_player.maxStamina = 5;
		_currentStatus = "Zombie";

		base.ChangePlayerStats ();
	}

	//Zombie collision
	void OnColliderEnter2D(Collider2D other)
	{
		if(other.transform.tag == Tags.Player && other.GetComponent<Survivor>())
		{
			other.GetComponent<Survivor>().BecomeZombie();
		}
	}
}
