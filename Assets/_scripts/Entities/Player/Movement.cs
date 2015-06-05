using UnityEngine;
using System.Collections;

public class Movement : MoveableNetworkEntity {
	private float _runSpeed;
	private float _walkSpeed;
	private float _stamina;
	private float _maxStamina;
	private float _currentRegenCooldown;
	private float _regenCooldown;
	private float _regenSpeed;
	private float _playerCondition;

	protected override void Start ()
	{
		base.Start ();
		_walkSpeed = 5;
		_runSpeed = 10;
		_speed = _walkSpeed;
		_runSpeed = 10;
		_maxStamina = 10;
		_regenSpeed = 0.1f;
		_stamina = _maxStamina;
		_currentRegenCooldown = 0;
		_regenCooldown = 2.5f;
		_playerCondition = 0.1f;
	}
	protected override void MovementInput ()
	{
		base.MovementInput ();
		//allemaal uit me hoofd code dus probably slecht
		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis = Input.GetAxis("Vertical");
		_rigidBody.velocity = new Vector3(horizontalAxis,verticalAxis,0) * _speed * Time.deltaTime;

		//Check running
		if(Input.GetKey(KeyCode.LeftShift))
		{
			Run();
		} 
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			StopRunning();
		} 
		else 
		{
			RegenStamina();
		}
	}
	private void Run()
	{
		//Running with the condition the player has.
		if(_stamina != 0)
		{
			_speed = _runSpeed;
			_stamina -= _playerCondition;
		}
		else
		{
			StopRunning();
		}
		//cooldown before stamina regens.
		//regencooldown = wait time
		_currentRegenCooldown = Time.time + _regenCooldown;
	}
	//change his speed back to walk speed
	private void StopRunning()
	{
		_speed = _walkSpeed;
	}
	private void RegenStamina()
	{
		//update stamina if currentregencooldown is lower then Time.time
		if(_currentRegenCooldown <= Time.time && _stamina != _maxStamina)
			_stamina += _regenSpeed;
	}
}
