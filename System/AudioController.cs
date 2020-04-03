using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	AudioSource _audiosource{
		get { return GetComponent<AudioSource>(); }
	}

	const float basePitch = 0.9f;
	const float pitchStep = 0.025f;

	public AudioClip[] buzzes; 

	public AudioClip titleJingle;
	public AudioClip gameJingle;

	public static SFXplayer sfxPlayer;

	// Start is called before the first frame update
	void Awake()
	{
		GameController.audioController = this;
	}
	
	void Start(){
		GameController.audioController = this;
	}

	// Update is called once per frame
	void Update()
	{
			
	}

	public void PlayTitleJingle(){
		_audiosource.Stop();
		_audiosource.clip = (titleJingle);
		_audiosource.Play();
	}

	public void PlayGameJingle(){
		_audiosource.Stop();
		_audiosource.clip = (gameJingle);
		_audiosource.Play();
	}

	public void ResetPitch(){
		_audiosource.pitch = basePitch;
	}

	public void SetPitch(float pitchLevel){
		_audiosource.pitch = basePitch + (pitchLevel * pitchStep);
	}

	public void PlayStretchClip(){
		sfxPlayer.Play();
	}

	public void StopStretchClip(){
		sfxPlayer.Stop();
	}

	public void PlayBuzz(){


	}


	public void StopBuzz(){
		
	}
}
