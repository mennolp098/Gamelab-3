using UnityEngine;
using System.Collections;

public class Movement : MoveableNetworkEntity {
	private float _runSpeed;
	private float _walkSpeed;
	private float _stamina;
	private float _maxStamina;
	private float _currentRegenCooldown = 0;
	private float _regenCooldown;
	private float _regenSpeed;
	private float _playerCondition;

	public void SetMovementStats (float walkSpeed,float runSpeed, float condition,float maxStamina = 10, float regenSpeed = 0.1f, float _regenCooldown = 2.5f)
	{
		_walkSpeed = walkSpeed;
		_runSpeed = runSpeed;
		_speed = _walkSpeed;

		_maxStamina = maxStamina;
		_stamina = _maxStamina;

		_playerCondition = condition; // the higher the condition the longer he can run.

		_regenSpeed = regenSpeed;
		_regenCooldown = _regenCooldown;

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
			_stamina -= 5 / _playerCondition;
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
