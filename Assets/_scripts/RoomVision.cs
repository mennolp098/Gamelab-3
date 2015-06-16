using UnityEngine;
using System.Collections;

public class RoomVision : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == Tags.Player)
		{
			GetComponent<FadeInOut>().Fade(0, 0.05f);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == Tags.Player)
		{
			GetComponent<FadeInOut>().Fade(1, 0.05f);
		}
	}
}
