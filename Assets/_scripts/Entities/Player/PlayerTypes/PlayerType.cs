﻿using UnityEngine;
using System.Collections;

public class PlayerType : MonoBehaviour {

	protected Player _player;
	protected Animator _animator;
	public RuntimeAnimatorController playerTypeAnimatorController;

	// Use this for initialization
	protected virtual void Start () {
		_player = GetComponent<Player> ();
		_animator = GetComponent<Animator> ();
		ChangePlayerStats ();
	}

	protected virtual void ChangePlayerStats () {
		SendMessage ("PlayerStatsChanged");
	}
}