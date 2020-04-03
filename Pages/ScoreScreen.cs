using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
	public TextMeshProUGUI scoreTable;

  void Awake(){
		GameController.scoreScreen = this.gameObject;
	}
  void Start(){
		GameController.scoreScreen = this.gameObject;
	}
	
	public void GoToTitle(){
		GameController.instance.GoToTitle();
	}

	void Update(){
		int[] scores = ScoreController.GetHighScores();
		string scoreStr = "";
		for(int i = 0; i < scores.Length; i++){
			scoreStr += scores[i].ToString("N0");
			scoreStr += "\n";
		}

		scoreTable.text = scoreStr;
	}

}
