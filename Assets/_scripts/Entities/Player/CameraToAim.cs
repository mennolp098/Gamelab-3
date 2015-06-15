using UnityEngine;
using System.Collections;

public class CameraToAim : MonoBehaviour {
	private NetworkView _networkView;

	private GameObject _camera;
	private float _lookSpeed = 1.2f;
	//private float _minDistanceToActivate = 1f;
	private float _maxLookDistance = 1f;
	private Vector3 _focusPoint;

	void Awake(){
		_camera = GameObject.FindGameObjectWithTag (Tags.Cam);
		_networkView = GetComponent<NetworkView>();
	}

	// Update is called once per frame
	void Update () {
		if(_networkView.isMine)
		{
			CameraMoveToMouse ();
			RotateToMouse ();
		}
	}
	private void CameraMoveToMouse(){
		_focusPoint = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, _camera.transform.position.z);
		
		Vector2 axisDistance = new Vector2(_focusPoint.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x,_focusPoint.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector3 cameraTargetPosition;
		
		if (axisDistance.magnitude <= _maxLookDistance) {
			cameraTargetPosition = new Vector3 (_focusPoint.x -axisDistance.x,
			                                    _focusPoint.y -axisDistance.y,
			                                    _focusPoint.z);
		} else {
			Vector2 maxDistance = axisDistance.normalized;
			
			maxDistance *= -_maxLookDistance;
			
			cameraTargetPosition = new Vector3 (_focusPoint.x + maxDistance.x,
			                                    _focusPoint.y + maxDistance.y,
			                                    _focusPoint.z);
		}
		float thisToCamDistance = (new Vector2(_camera.transform.position.x - transform.position.x,_camera.transform.position.y - transform.position.y).magnitude);
		cameraTargetPosition.x = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, _focusPoint.x -_maxLookDistance,_focusPoint.x +_maxLookDistance);
		cameraTargetPosition.y = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, _focusPoint.y -_maxLookDistance,_focusPoint.y+_maxLookDistance);
		_camera.transform.position = Vector3.Lerp (_camera.transform.position, cameraTargetPosition, _lookSpeed * Time.deltaTime * (thisToCamDistance + 1));
	}
	private void RotateToMouse(){
		Vector3 mousePosition = Input.mousePosition;
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
	  	transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}
	private void Shooting()
	{
		StartCoroutine("ShakeCamera");
	}
	private IEnumerator ShakeCamera()
	{
		int shakeAmount = 2;
		for (int i = 0; i < shakeAmount; i++) 
		{
			Vector3 camPos = _camera.transform.position;
			camPos.x += Random.Range(-0.5f,0.5f);
			camPos.y += Random.Range(-0.5f,0.5f);
			_camera.transform.position = camPos;
			yield return new WaitForSeconds(0.01f);
		}
	}
}
