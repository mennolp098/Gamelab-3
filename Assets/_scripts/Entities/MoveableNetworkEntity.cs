using UnityEngine;
using System.Collections;

public class MoveableNetworkEntity : MonoBehaviour {
	protected float _speed;
	protected float _normalSpeed;
	protected float _objectSpeed;
	protected Rigidbody2D _rigidBody;
	protected bool _isGrounded;
	protected NetworkView _networkView;

	private float _lastSynchronizationTime = 0f;
	private float _syncDelay = 0f;
	private float _syncTime = 0f;
	private Vector3 _syncStartPosition = Vector3.zero;
	private Quaternion _syncStartRotation = Quaternion.identity;
	private Vector3 _syncEndPosition = Vector3.zero;
	private Quaternion _syncEndRotation = Quaternion.identity;
	private Animator _animator;

	protected virtual void Awake()
	{
		_networkView = this.GetComponent<NetworkView>();
		_rigidBody = this.GetComponent<Rigidbody2D>();
		if(this.GetComponent<Animator>())
		{
			_animator = this.GetComponent<Animator>();
		} else if(this.GetComponentInChildren<Animator>())
		{
			_animator = this.GetComponentInChildren<Animator>();
		}
	}
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;;
		if (stream.isWriting) //write new values into stream for other players.
		{
			syncPosition = _rigidBody.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = _rigidBody.velocity;
			stream.Serialize(ref syncVelocity);
			
			syncRotation = transform.rotation;
			stream.Serialize(ref syncRotation);
		}
		else //stream values for other players so they can sync the variables.
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncRotation);

			//calculate delay in ms.
			_syncTime = 0f;
			_syncDelay = Time.time - _lastSynchronizationTime;
			_lastSynchronizationTime = Time.time;

			//calculate end position for current entity.
			_syncEndPosition = syncPosition + syncVelocity * _syncDelay;
			_syncStartPosition = _rigidBody.position;

			//check new rotation.
			_syncEndRotation = syncRotation;
			_syncStartRotation = transform.rotation;
		}
	}
	// Use this for initialization
	protected virtual void Start () {
		
	}
	
	protected virtual void Update()
	{
		if (_networkView.isMine)
		{
			MovementInput();
		}
		else
		{
			SyncedMovement();
		}
	}
	protected virtual void MovementInput()
	{
	}
	//sync movement for other players.
	private void SyncedMovement()
	{
		_syncTime += Time.deltaTime;
		_rigidBody.position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);

		transform.rotation = Quaternion.Slerp(_syncStartRotation, _syncEndRotation, _syncTime / _syncDelay);
	}
	[RPC]
	private void ChangeSpeed(float speed)
	{
		_speed = speed;
	}
	public void AddSpeed(float strenght,float duration = 0)
	{
		_speed += strenght;

		_networkView.RPC("ChangeSpeed", RPCMode.Others,_speed);

		if(duration != 0)
			Invoke("ResetSpeed",duration);
	}
	private void ResetSpeed()
	{
		_speed = _normalSpeed;
		_networkView.RPC("ChangeSpeed", RPCMode.Others,_speed);
	}
	public Vector3 syncStartPosition
	{
		get{
			return _syncStartPosition;
		}
		set{
			_syncStartPosition = value;
		}
	}
	public virtual void DestroyNetworkObject(){
		Network.RemoveRPCs(this._networkView.owner);
		Network.Destroy (this.gameObject);
	}
	[RPC]
	protected void SetAnimation(string animName)
	{
		_animator.Play(animName);
	}
	[RPC]
	protected void SetScale(Vector3 newScale)
	{
		this.transform.localScale = newScale;
	}
}
