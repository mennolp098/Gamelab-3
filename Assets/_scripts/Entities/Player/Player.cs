using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public string usernameText;
	public GameObject usernameCanvas;
	public Text usernameDisplayText;
	private UserInfo _myUserInfo;

	// Player stats
	public float healthPoints;

	public float walkSpeed;
	public float runSpeed;
	public float condition;
	public float maxStamina;
	public float respawnTime = 3;

	protected NetworkView _networkView;
	protected Health _healhComponent;
	protected Movement _movementComponent;

	private Image _bloodScreen;
	private ParticleSystem _particleSystem;
	// Use this for initialization
	protected virtual void Awake()
	{
		_networkView = GetComponent<NetworkView>();
		_healhComponent = gameObject.AddComponent<Health> ();
		_movementComponent = gameObject.AddComponent<Movement> ();
		_networkView.observed = _movementComponent;

		_healhComponent.HealthLostEvent += OnPlayerHit;
		_healhComponent.NoHealthLeftEvent += OnPlayerDeath;

		_particleSystem = GetComponent<ParticleSystem>();
	}

	void Start()
	{
		usernameCanvas.GetComponent<RectTransform>().SetParent(null);
		if(_networkView.isMine)
		{
			_bloodScreen = GameObject.Find("BloodScreen").GetComponent<Image>();
			_myUserInfo = GameObject.FindGameObjectWithTag(Tags.Connector).GetComponent<UserInfo>();
			_networkView.RPC("ShowMyUsername", RPCMode.All, _myUserInfo.username);
		}
	}
	void Update()
	{
		usernameCanvas.GetComponent<RectTransform>().position = this.transform.position + new Vector3(0,1.5f,0);
	}
	private void PlayerStatsChanged(){

		_healhComponent.SetHealth (healthPoints);
		_movementComponent.SetMovementStats (walkSpeed,runSpeed,condition,maxStamina);
	}

	protected virtual void OnPlayerHit(float value){
		//BroadcastMessage ("PlayAnimation", PlayerType.HIT_ANIM);
		_networkView.RPC("BloodSplatter", RPCMode.All);
		if(_networkView.isMine)
		{
			SeeBlood();

			float[] shakeParameters = new float[3];
			float shakeAmount = 2;
			float shakeIntensity = 0.5f;
			float shakeSpeed = 0.1f;
			
			shakeParameters[0] = shakeAmount;
			shakeParameters[1] = shakeIntensity;
			shakeParameters[2] = shakeSpeed;
			
			SendMessage("Shake", shakeParameters);
		}
	}
	[RPC]
	private void BloodSplatter()
	{
		_particleSystem.Play();
	}

	private void SeeBlood()
	{
		Color newColor = _bloodScreen.color;
		newColor.a = 0.5f;
		_bloodScreen.color = newColor;
		Invoke("RemoveBlood", 0.1f);
	}
	private void RemoveBlood()
	{
		Color newColor = _bloodScreen.color;
		newColor.a = 0;
		_bloodScreen.color = newColor;
	}
	protected virtual void OnPlayerDeath(GameObject playerDied){
		BroadcastMessage ("PlayAnimation", PlayerType.DEATH_ANIM);

		_networkView.RPC("NetworkPlayerDeath", RPCMode.All);

	}

	[RPC]
	private void NetworkPlayerDeath()
	{
		//destroy all components that make a player
		Destroy(GetComponent<Movement>());
		Destroy(GetComponent<Rigidbody2D>());
		Destroy(GetComponent<BoxCollider2D>());

		//respawn zombie if hideandseek
		if(GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<HideAndSeekGameMode>() && GetComponent<Zombie>())
		{
			Invoke("Respawn", respawnTime);
		}
	}

	private void Respawn()
	{
		gameObject.AddComponent<Movement>();
		gameObject.AddComponent<Rigidbody2D>();
		gameObject.AddComponent<BoxCollider2D>();
	}
	
	// Network functions
	[RPC]
	private void ShowMyUsername(string username)
	{
		usernameText = username;
		this.name = username;
		usernameDisplayText.text = username;
		Color newColor = usernameDisplayText.color;
		newColor.a = 0;
		usernameDisplayText.color = newColor;
	}

	//mouse fuctions for username
	private void OnMouseOver()
	{
		Color newColor = usernameDisplayText.color;
		if(newColor.a != 1f)
		{
			newColor.a += 0.05f;
		}
		usernameDisplayText.color = newColor;
	}
	private void OnMouseExit()
	{
		Color newColor = usernameDisplayText.color;
		if(newColor.a != 0f)
		{
			newColor.a = 0f;
		}
		usernameDisplayText.color = newColor;
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == Tags.GrayRoom && _networkView.isMine)
		{
			other.GetComponent<FadeInOut>().Fade(0, 0.05f);
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == Tags.GrayRoom & _networkView.isMine)
		{
			other.GetComponent<FadeInOut>().Fade(1, 0.05f);
		}
	}
}
