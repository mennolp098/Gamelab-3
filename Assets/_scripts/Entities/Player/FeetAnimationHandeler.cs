using UnityEngine;
using System.Collections;

public class FeetAnimationHandeler : MonoBehaviour {


	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator> ();
	}
	
	void PlayAnimation(string animation){
		_animator.speed = 1;
		string animationToPlay;
		if (animation == PlayerType.RUN_ANIM || animation == PlayerType.WALK_ANIM) {
			if(animation == PlayerType.RUN_ANIM){
				_animator.speed = 2;
			}
			animationToPlay = "feetWalk";
		} else {
			animationToPlay = animation;
		}

		_animator.Play (animationToPlay);
	}
}
