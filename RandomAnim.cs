using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnim : MonoBehaviour
{
	Animator _anim{
		get { return GetComponent<Animator>(); }
	}

	// Start is called before the first frame update
	void Start()
	{
		AnimatorStateInfo state = _anim.GetCurrentAnimatorStateInfo(0);
		_anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
	}

}
