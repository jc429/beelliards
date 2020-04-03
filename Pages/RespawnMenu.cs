using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RespawnMenu : MonoBehaviour
{

	public TextMeshProUGUI penaltyText;

  void Awake(){
		GameController.respawnMenu = this.gameObject;
	}
  void Start(){
		GameController.respawnMenu = this.gameObject;
	}

	public void GoToTitle(){
		ScoreController.CheckCurrentRun();
		GameController.instance.CloseRespawnMenu();
		GameController.ResetGame();
	}

	public void Respawn(){
		GameController.instance.RespawnPlayer();
		GameController.instance.CloseRespawnMenu();
	}

	void Update(){
		penaltyText.text = "Penalty: " + ScoreController.GetRespawnPenalty().ToString("N0");
	}
}
