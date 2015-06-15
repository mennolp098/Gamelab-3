using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour {
	private GameObject _camera;
	void Awake()
	{
		_camera = GameObject.FindGameObjectWithTag(Tags.Cam);
	}
	private void Shake(float[] shakeParameters)
	{
		StartCoroutine(StartShaking(shakeParameters));
	}
	private IEnumerator StartShaking(float[] shakeParameters)
	{
		int shakeAmount = (int)shakeParameters[0];
		float shakeIntensity = shakeParameters[1];
		float shakeSpeed = shakeParameters[2];
		for (int i = 0; i < shakeAmount; i++) 
		{
			Vector3 camPos = _camera.transform.position;
			camPos.x += Random.Range(-shakeIntensity,shakeIntensity);
			camPos.y += Random.Range(-shakeIntensity,shakeIntensity);
			_camera.transform.position = camPos;
			yield return new WaitForSeconds(shakeSpeed);
		}
	}
}
