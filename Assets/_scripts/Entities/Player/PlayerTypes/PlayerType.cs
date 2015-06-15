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


	public void lockAnim(){
		_animationLocked = true;
	}
	public void unlockAnim(){
		_animationLocked = false;
	}

	public bool GetAnimLockStage(){
		return _animationLocked;
	}

	private void PlayAnimation(string animation){
		if (!_animationLocked && _animator.GetCurrentAnimatorStateInfo (0).IsName(ConvertAnimationName(animation)) == false) {
			_networkView.RPC ("PlayAnimationNetwork", RPCMode.All, ConvertAnimationName(animation,true));
			Debug.Log (ConvertAnimationName (animation));
		}
	}

	public virtual string ConvertAnimationName(string animName, bool trueName = false){
		string animNameToReturn = animName;

		if (animNameToReturn == RUN_ANIM && !trueName) {
			animNameToReturn = WALK_ANIM;
		}

		return animNameToReturn;
	}

	[RPC]
	protected virtual void PlayAnimationNetwork(string animation){

		_animator.speed = 1;
		string animationToPlay = animation;

		if (animationToPlay == PlayerType.RUN_ANIM) {
			_animator.speed = 2;
			animationToPlay = PlayerType.WALK_ANIM;
		}
		_animator.Play(animationToPlay);
		Debug.Log (animationToPlay);
	}
}
