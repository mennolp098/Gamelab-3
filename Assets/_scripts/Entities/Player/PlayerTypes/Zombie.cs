using UnityEngine;
using System.Collections;

public class Zombie : PlayerType {

	protected override void ChangePlayerStats () {
		base.ChangePlayerStats ();

		//TODO GetComponent<SpriteRenderer> ().sprite = Resources.Load (Zombiesprite); <----

		_player.healthPoints = 200; // not sure if 200 but at least you can see it must be changed here <3
		_player.walkSpeed = 2;
		_player.runSpeed = 3;
	}
}
