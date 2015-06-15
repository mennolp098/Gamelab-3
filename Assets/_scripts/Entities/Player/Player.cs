using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public string usernameText;
	private UserInfo _myUserInfo;

	// Player stats
	public float healthPoints;

	public float walkSpeed;
	public float runSpeed;
	public float condition;
	public float maxStamina;

	protected NetworkView _networkView;
	protected Health _healhComponent;
	protected Movement _movementComponent;
	
	// Use this for initialization
	protected virtual void Awake()
	{
		_networkView = GetComponent<NetworkView>();
		_healhComponent = gameObject.AddComponent<Health> ();
		_movementComponent = gameObject.AddComponent<Movement> ();
		_networkView.observed = _movementComponent;

		_healhComponent.HealthLostEvent += OnPlayerHit;
		_healhComponent.NoHealthLeftEvent += OnPlayerDeath;
	}

	private void PlayerStatsChanged(){

		_healhComponent.SetHealth (healthPoints);
		_movementComponent.SetMovementStats (walkSpeed,runSpeed,condition,maxStamina);
	}

	protected virtual void OnPlayerHit(float value){
		BroadcastMessage ("PlayAnimation", PlayerType.HIT_ANIM);
	}

	protected virtual void OnPlayerDeath(){
		BroadcastMessage ("PlayAnimation", PlayerType.DEATH_ANIM);
	}

	void Start()
	{
		if(_networkView.isMine)
		{

			_myUserInfo = GameObject.FindGameObjectWithTag(Tags.Connector).GetComponent<UserInfo>();
			_networkView.RPC("ShowMyUsername", RPCMode.All, _myUserInfo.username);
			Debug.Log(_networkView.viewID);
		}
	}

	// Network functions

	[RPC]
	private void ShowMyUsername(string username)
	{
		usernameText = username;
		this.name = username;
	}
}
