using UnityEngine;
using System.Collections;

public class Zombie : PlayerType {
	private float _attackRange = 0.5f;
	private float _attackRadius = 0.75f;
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
	void Update()
	{
		if(_networkView.isMine)
		{
			AttackInput();
		}
	}
	private void AttackInput()
	{
		if(Input.GetMouseButton(0))
		{
			Attack ();
		}
	}
	private void Attack()
	{
		_networkView.RPC("CreateCollision", RPCMode.Server);
		BroadcastMessage("PlayAnimation", ATTACK_ANIM);
	}
	[RPC]
	private void CreateCollision()
	{
		Vector2 pos = new Vector2(this.transform.position.x,this.transform.position.y);
		pos += new Vector2(transform.up.x,transform.up.y) * _attackRange;
		Collider2D[] cols = Physics2D.OverlapCircleAll(pos,_attackRadius);
		foreach(Collider2D col in cols)
		{
			if(col.transform.tag == Tags.Player && col.GetComponent<Survivor>())
			{
				col.GetComponent<Survivor>().BecomeZombie();
				break;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(this.transform.position + this.transform.up * _attackRange, _attackRadius);
	}

}
