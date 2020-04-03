﻿using UnityEngine;

public class Timer {
	public float time;
	public float duration; 

	bool isFinished;
	public bool IsFinished{
		get { return isFinished; }
	}

	bool isActive;
	public bool IsActive{
		get { return isActive; }
	}
	
	public float CompletionPercentage{
		get {
			if(duration <= 0){
				return 1;
			}
			return time/duration;
		}
	}


	public Timer(){}
	public Timer(float length){
		duration = length;
	}

	public void SetActive(bool active){
		isActive = active;
	}

	public void Start(){
		Reset();
		isActive = true;
	}



	public void AdvanceTimer(float dTime){
		time += dTime;
		if(time >= duration){
			time = duration;
			Finish();
		}
	}

	public void SetDuration(float length){
		duration = length;
	}

	void Finish(){
		isFinished = true;
	}

	public void Reset(){
		time = 0;
		isFinished = false;
		isActive = false;
	}





}

