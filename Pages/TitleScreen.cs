using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
  void Awake(){
		GameController.titleScreen = this.gameObject;
	}
  void Start(){
		GameController.titleScreen = this.gameObject;
	}

	public void StartGame(){
		GameController.instance.StartGame();
	}

	public void GoToCredits(){
		GameController.instance.GoToCredits();
	}
	public void GoToScores(){
		GameController.instance.GoToScores();
	}
	public void OpenSettingsMenu(){
		GameController.instance.OpenSettingsMenu();
	}
}
