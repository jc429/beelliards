using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
	const float resetTime = 2.0f;
	public Image[] rings;
	Timer resetTimer = new Timer(resetTime);
	public Gradient resetGradient;

	// Start is called before the first frame update
	void Start()
	{
		ResetResetTimer();
	}

	// Update is called once per frame
	void Update()
	{
		
		if((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Escape)) && GameController.GetGameState == GameState.Game){
			StartResetTimer();
		}

		if(resetTimer.IsActive){
			resetTimer.AdvanceTimer(Time.deltaTime);
			foreach(Image img in rings){
				img.fillAmount = resetTimer.CompletionPercentage * 1.05f;
				img.color = resetGradient.Evaluate(resetTimer.CompletionPercentage);
			}
			if(resetTimer.IsFinished){
				TriggerReset();
			}

			if(!Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.Escape)){
				ResetResetTimer();
			}
		}
	}

	public void StartResetTimer(){
		resetTimer.Start();
	}

	void ResetResetTimer(){
		resetTimer.Reset();
		foreach(Image img in rings){
			img.fillAmount = 0;
			img.color = resetGradient.Evaluate(0);
		}
	}

	public void TriggerReset(){
		ScoreController.CheckCurrentRun();
		GameController.ResetGame();
	}

}
