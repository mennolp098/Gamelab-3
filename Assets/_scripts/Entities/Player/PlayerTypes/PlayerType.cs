﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerType : MonoBehaviour {

	public const string IDLE_ANIM = "Idle";
	public const string WALK_ANIM = "Walk";
	public const string RUN_ANIM = "Run";
	public const string ATTACK_ANIM = "Attack";

	protected string _currentStatus = "Survivor";
	protected Text _uiStatus;

	protected Player _player;
	protected Animator _animator;
	public RuntimeAnimatorController playerTypeAnimatorController;
	protected NetworkView _networkView;

	// Use this for initialization
	protected virtual void Start () {
		_player = GetComponent<Player> ();
		_animator = GetComponent<Animator> ();
		_networkView = GetComponent<NetworkView>();
		_uiStatus = GameObject.Find("StatusText").GetComponent<Text>();
		ChangePlayerStats ();
	}

	protected virtual void ChangePlayerStats () {
		_uiStatus.text = _currentStatus;
		SendMessage ("PlayerStatsChanged");
	}



	private void PlayAnimationNetwork(string animation){
		GetComponent<NetworkView>().RPC("ShowMyUsername", RPCMode.All, animation);
	}

	[RPC]
	protected virtual void PlayAnimation(string animation){

	}

}
