using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerType : MonoBehaviour {

	public const string IDLE_ANIM = "Idle";
	public const string WALK_ANIM = "Walk";
	public const string RUN_ANIM = "Run";
	public const string ATTACK_ANIM = "Attack";
	public const string DEATH_ANIM = "Death";
	public const string HIT_ANIM = "Hit";

	protected string _currentStatus = "Survivor";
	protected Text _uiStatus;
	
	protected Player _player;
	protected Animator _animator;
	protected NetworkView _networkView;

	private bool _animationLocked = false;

	// Use this for initialization
	protected virtual void Start () {
		_player = GetComponent<Player> ();
		_animator = GetComponent<Animator> ();
		_networkView = GetComponent<NetworkView> ();
		if (GameObject.Find ("StatusText") != null) {
			_uiStatus = GameObject.Find ("StatusText").GetComponent<Text> ();
		}
		ChangePlayerStats ();
	}

	protected virtual void ChangePlayerStats () {
		if(_networkView.isMine)
		{
			_uiStatus.text = _currentStatus;
		}
		SendMessage ("PlayerStatsChanged");
	}


	private void lockAnim(){
		_animationLocked = true;
	}
	private void unlockAnim(){
		_animationLocked = false;
	}

	[RPC]
	private void PlayAnimation(string animation){
		if (!_animationLocked && _animator.GetCurrentAnimatorStateInfo (0).IsName(animation) == false) {
			_networkView.RPC ("PlayAnimationNetwork", RPCMode.All, animation);
		}
	}
	
	[RPC]
	protected virtual void PlayAnimationNetwork(string animation){
		
	}
}
