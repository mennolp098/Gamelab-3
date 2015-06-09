using UnityEngine;
using System.Collections;

public class MouseView : MonoBehaviour {
	public Texture2D mouseTexture;

	private float _lookRange;
	private float _lookSpeed;
	private Vector3 _normalCamPosition;
	private Vector2 _hotSpot;
	private GameObject _camera;
	private NetworkView _networkView;
	// Use this for initialization
	void Start () {
		_camera = GameObject.FindGameObjectWithTag(Tags.Cam);
		_networkView = this.GetComponent<NetworkView>();
		_lookRange = 2.5f;
		_lookSpeed = 2;
		_normalCamPosition = _camera.transform.position;
		_hotSpot = new Vector2(43,43); // texture width and height = 86 so devided by 2 = 43
		Cursor.SetCursor(mouseTexture,_hotSpot,CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		if(_networkView.isMine)
		{
			RotateToMouse();
			MoveToMouse();
		}
	}
	private void RotateToMouse()
	{
		Vector3 mousePosition = Input.mousePosition + new Vector3(_hotSpot.x,_hotSpot.y,0);
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	private void MoveToMouse()
	{
		Vector3 cameraPosition = _normalCamPosition;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _hotSpot.x,Input.mousePosition.y + _hotSpot.y,10f)), Vector2.zero);
		if(hit.collider != null)
		{
			Vector2 hitPoint = hit.point;
			cameraPosition.x = Mathf.Clamp(hitPoint.x, this.transform.position.x-_lookRange,this.transform.position.x+_lookRange);
			cameraPosition.y = Mathf.Clamp(hitPoint.y, this.transform.position.y-_lookRange,this.transform.position.y+_lookRange);
			_camera.transform.position = Vector3.Lerp(_camera.transform.position, cameraPosition, _lookSpeed * Time.deltaTime);
		}
	}
}
