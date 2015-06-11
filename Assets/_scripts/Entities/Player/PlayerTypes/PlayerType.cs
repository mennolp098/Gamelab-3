using UnityEngine;
using System.Collections;

public class PlayerType : MonoBehaviour {

	public const string IDLE_ANIM = "Idle";
	public const string WALK_ANIM = "Walk";
	public const string RUN_ANIM = "Run";
	public const string ATTACK_ANIM = "Attack";

	protected Player _player;
	protected Animator _animator;
	public RuntimeAnimatorController playerTypeAnimatorController;
	protected NetworkView _networkView;

	// Use this for initialization
	protected virtual void Start () {
		_player = GetComponent<Player> ();
		_animator = GetComponent<Animator> ();
		_networkView = GetComponent<NetworkView>();
		ChangePlayerStats ();
	}

	protected virtual void ChangePlayerStats () {
		SendMessage ("PlayerStatsChanged");
	}



	private void PlayAnimationNetwork(string animation){
		GetComponent<NetworkView>().RPC("ShowMyUsername", RPCMode.All, animation);
	}

	[RPC]
	protected virtual void PlayAnimation(string animation){

	}

}
