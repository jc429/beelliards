using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
      AudioController.sfxPlayer = this;
    }

    // Update is called once per frame
    public void Play()
    {
			GetComponent<AudioSource>().Play();
    }

		public void Stop(){
			GetComponent<AudioSource>().Stop();
		}
}
