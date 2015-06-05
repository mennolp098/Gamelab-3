using UnityEngine;
using System.Collections;

public class MouseView : MonoBehaviour {
	private float _lookRange;
	private float _lookRadius;
	private float _lookSpeed;
	private GameObject _camera;
	private NetworkView _networkView;
	// Use this for initialization
	void Start () {
		_camera = GameObject.FindGameObjectWithTag(Tags.Cam);
		_networkView = this.GetComponent<NetworkView>();
		_lookRange = 100;
		_lookRadius = 20;
		_lookSpeed = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if(_networkView.isMine)
		{
			//Aim script works probably
			Vector3 mousePosition = Input.mousePosition;
			Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
			Vector3 dir = mousePosition - pos;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			//Ik weet niet of dit goed is tho
			if(Vector3.Distance(pos,transform.position) > _lookRadius)
			{
				Vector3 newPos = this.transform.position + dir.normalized * _lookRange;
				newPos.z = _camera.transform.position.z;
				_camera.transform.position = Vector3.Lerp(_camera.transform.position, newPos, _lookSpeed * Time.deltaTime);
			} 
			else if(_camera.transform.position.x != this.transform.position.x || _camera.transform.position.y != this.transform.position.y)
			{
				Vector3 newPos = this.transform.position;
				newPos.z = _camera.transform.position.z;
				_camera.transform.position = Vector3.Lerp(_camera.transform.position,newPos, _lookSpeed * Time.deltaTime);
			}
		}
	}
}
