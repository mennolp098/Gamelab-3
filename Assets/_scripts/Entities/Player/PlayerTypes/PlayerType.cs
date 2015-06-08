using UnityEngine;
using System.Collections;

public class PlayerType : MonoBehaviour {

	protected Player _player;

	// Use this for initialization
	void Awake () {
		_player = GetComponent<Player> ();
	}

	void Start(){
		ChangePlayerStats ();
	}
	
	protected virtual void ChangePlayerStats () {

	}
}
