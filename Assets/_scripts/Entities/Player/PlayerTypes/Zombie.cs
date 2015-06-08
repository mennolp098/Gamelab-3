using UnityEngine;
using System.Collections;

public class Zombie : PlayerType {

	protected override void ChangePlayerStats () {
		//TODO GetComponent<SpriteRenderer> ().sprite = Resources.Load (Zombiesprite); <----

		_player.healthPoints = 200; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 2;
		_player.runSpeed = 3;
		_player.condition = 5;
		_player.maxStamina = 5;


		base.ChangePlayerStats ();
	}
}
