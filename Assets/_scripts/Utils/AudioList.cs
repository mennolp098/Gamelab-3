using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioList : MonoBehaviour 
{
	//Music
	[SerializeField]private AudioClip shootSound;
	[SerializeField]private AudioClip zombieRawrSound;
	[SerializeField]private AudioClip zombieMassiveRawrSound;


	private List<AudioClip> AudioL = new List<AudioClip> ();

	public void Lists()
	{
		//Gun sounds
		AudioL.Add (shootSound);

		//Zombie sounds
		AudioL.Add (zombieRawrSound);
		AudioL.Add (zombieMassiveRawrSound);
	}
	public AudioClip PlayAudio(string audioname)
	{
		AudioClip newSound = null;
		foreach(AudioClip sound in AudioL)
		{
			if(sound.name == audioname)
			{
				newSound = sound;
				break;
			}
		}
		if(newSound == null)
		{
			Debug.LogWarning("no sound has been found, add it in the list if exists.");
			return null;
		}
		return newSound;
	}
	void Awake()
	{
		Lists ();
	}


}
