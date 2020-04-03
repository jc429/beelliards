using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveDisplay : MonoBehaviour
{
	TextMeshProUGUI _text{
		get { return GetComponent<TextMeshProUGUI>(); }
	}
	
	void Update(){
		_text.text = ""+(ScoreController.CurrentWave + 1);
	}	

}
