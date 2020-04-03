using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
	Rigidbody _rigidbody{
		get{ return GetComponent<Rigidbody>(); }
	}
	AudioSource _audioSource{
		get { return GetComponent<AudioSource>(); }
	}

	public TargetArrow targetArrow;

	bool isActive;

	public bool startActive;

	public GameObject selectCanvas;
	public Score scorePrefab;

	static bool activatedBee;
	public bool expire = false;

	int baseScore = 10;
	float multiplier = 1;
	const float multiTime = 3f;
	Timer multiTimer = new Timer(multiTime);

	Bee lastBeeHit;

	// Start is called before the first frame update
	void Start()
	{
		GameController.AddBee(this);
		if(startActive){
			Activate();
		}
		SetRandomRotation();
	}

	void Update(){
		if(multiTimer.IsActive){
			multiTimer.AdvanceTimer(Time.deltaTime);
			if(multiTimer.IsFinished){
				ResetMultiplier();
			}
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if(_rigidbody.velocity.y > 0){
			Vector3 v = _rigidbody.velocity;
			v.y *= 0.85f;
			_rigidbody.velocity = v;
		}
		if(expire){
			Deactivate();
			Destroy(this.gameObject);
		}
	}

	public void Activate(){
		GameController.ActivateBee(this);
		isActive = true;
		selectCanvas.SetActive(true);
	}

	public void Deactivate(){
		GameController.DeactivateBee(this);
		isActive = false;
		selectCanvas.SetActive(false);
		expire = true;
	}

	public void ApplyForce(Vector3 dir, float force){
		_rigidbody.velocity = Vector3.zero;
		Vector3 forceVec = dir.normalized * force;
		_rigidbody.AddForce(forceVec, ForceMode.Impulse);
		Bounce(force);
	}

	public void HitFromPosition(Vector3 pos, float force){
		
		Vector3 dir;
		if(GameSettings.InvertControls){
			dir = pos - transform.position;
		} 
		else{
			dir = transform.position - pos;
		}
		//Debug.Log("D" + dir);
		ApplyForce(dir, force);
	}

	void SetRandomRotation(){
		int r = Random.Range(0,360);
		transform.rotation = Quaternion.Euler(0, r, 0);
	}

	public void Terminate(){
		Deactivate();
	}

	public void Bounce(float magnitude){
		

		if(GameController.bounceTimer.IsActive){
			return;
		}
		if(expire){
			return;
		}
	//	Debug.Log(magnitude);
		AudioClip c = _audioSource.clip;
		_audioSource.volume = Mathf.Lerp(0.4f, 1f, (magnitude / GameController.instance.maxHitForce));
		_audioSource.pitch = Random.Range(0.8f, 1.0f);
		_audioSource.PlayOneShot(c);
		GameController.bounceTimer.Start();

		
	}

	void OnCollisionEnter(Collision c){
		
		if(c.transform.tag.Equals("Bee") || c.transform.tag.Equals("Wall")){
			Bounce(c.relativeVelocity.magnitude);
			Bee b = c.gameObject.GetComponent<Bee>();

			if(b != null){
				if(b.lastBeeHit == this){
					return;
				}

				int p = Mathf.FloorToInt((float)baseScore * multiplier * b.multiplier);		/// SCORE
				Score scoreSpawn = Instantiate(scorePrefab);
				scoreSpawn.transform.position = c.GetContact(0).point;
				//scoreSpawn.transform.position = transform.position;
				scoreSpawn.SetPoints(p);
				ScoreController.AddScore(p);
				AddMultiplier();

				lastBeeHit = b;
				b.lastBeeHit = this;
			}
		}
	}

	void AddMultiplier(){
		multiplier += 0.1f;
		multiplier = Mathf.Clamp(multiplier, 0, 10f);
		multiTimer.Reset();
		multiTimer.Start();
	}

	void ResetMultiplier(){
		multiplier = 1;
		multiTimer.Reset();
	}
}
