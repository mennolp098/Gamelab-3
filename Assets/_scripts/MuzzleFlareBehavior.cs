using UnityEngine;
using System.Collections;

public class MuzzleFlareBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("DestroyMe", 0.1f);
	}
	void DestroyMe()
	{
		if(Network.isServer)
			Network.Destroy(this.gameObject);
	}
}