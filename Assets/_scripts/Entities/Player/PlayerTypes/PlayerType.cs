using UnityEngine;
using System.Collections;

public class PlayerType : MonoBehaviour {

	protected Player _player;

	// Use this for initialization
	protected virtual void Awake () {
		_player = GetComponent<Player> ();
		ChangePlayerStats ();
	}

	protected virtual void ChangePlayerStats () {
		SendMessage ("PlayerStatsChanged");
	}
}
