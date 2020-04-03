using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
	public TextMeshPro tmp;

	const float riseSpeed = 0.4f;

	Timer lifeTimer = new Timer(0.8f);

	public Gradient colorOverLifeTime;

	// Start is called before the first frame update
	void Start()
	{
			
	}

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(Camera.main.transform, Vector3.back);

		lifeTimer.AdvanceTimer(Time.deltaTime);

		Vector3 v = tmp.transform.localPosition;
		v.y -= riseSpeed * Time.deltaTime;
		tmp.transform.localPosition = v; 

		Color c = colorOverLifeTime.Evaluate(lifeTimer.CompletionPercentage);
		tmp.color = c;

		if(lifeTimer.IsFinished){
			Destroy(this.gameObject);
		}
	}

	public void SetPoints(int p){
		p = ScoreController.GetModifiedScore(p);
		tmp.text = "" + p;
		GameController.scoreTicker.SpawnTick(p);
	}



}
