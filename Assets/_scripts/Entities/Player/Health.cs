using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public delegate void FloatInfoDelegate(float value);
	public delegate void EmptyInfoDelegate ();

	public event FloatInfoDelegate HealthAddedEvent;
	public event FloatInfoDelegate HealthLostEvent;
	public event EmptyInfoDelegate NoHealthLeftEvent;

	private float _maxHealth;
	private float _currentHealth;

	private NetworkView _networkView;

	void Awake()
	{
		_networkView = GetComponent<NetworkView>();
	}

	public void SetHealth(float amountMaxHealth,float currentHealth = float.NaN){
		_maxHealth = amountMaxHealth;
		if (float.IsNaN (currentHealth)) {
			_currentHealth = amountMaxHealth;
		} else {
			_currentHealth = currentHealth;
		}
		_networkView.RPC("UpdateHealth", RPCMode.Others, _currentHealth);
	}

	public void AddSubHealth(float amount){
		_currentHealth += amount;
		if (amount > 0) {
			if(HealthAddedEvent != null){
				HealthAddedEvent(amount); //parameter amount because the total can be returned with a get total and get max.
			}
		} else if (amount < 0) {
			if(HealthLostEvent != null){
				HealthLostEvent(amount);
			}
			if (_currentHealth <= 0) {
				_currentHealth = 0;
				if(NoHealthLeftEvent != null){
					NoHealthLeftEvent();
				}
			}
		}
		_networkView.RPC("UpdateHealth", RPCMode.Others, _currentHealth);
	}

	[RPC]
	private void UpdateHealth(float newHealth)
	{
		_currentHealth = newHealth;
		Debug.Log("UpdatingHealth");
	}

	public float currentHealth{
		get{return _currentHealth;}
	}

	public float maxHealth{
		get{return _maxHealth;}
	}
}
