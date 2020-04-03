using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
  public ParticleSystem particles; 
	
	void OnTriggerEnter(Collider other){
		other.GetComponent<Bee>().Terminate();
		if(other.GetComponent<Bee>() == GameController.instance.playerBee){
			GameController.instance.OpenRespawnMenu();
		}
		else{
			ScoreController.AddGoalScore(transform.position + new Vector3(0,1,0));
			particles.Play();
		}
	}
}
