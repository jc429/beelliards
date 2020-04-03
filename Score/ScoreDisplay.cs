using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
	TextMeshProUGUI _text{
		get { return GetComponent<TextMeshProUGUI>(); }
	}
	
	void Update(){
		int score = ScoreController.CurrentScore;

		_text.text = score.ToString("N0");

	}	

}
