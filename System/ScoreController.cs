using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreController 
{
  public static int NumScores = 5;

	static int[] highScores = new int[NumScores];
	public static int[] GetHighScores(){
		return highScores;
	}
	public static void SetHighScores(int[] scores){
		int scoreCount = Mathf.Min(scores.Length, NumScores);
		for(int i = 0; i < scoreCount; i++){
			highScores[i] = scores[i];
		}
	}

	static int score;
	public static int CurrentScore{
		get { return score; }
	}

	static int currentWave = 0;
	public static int CurrentWave{
		get { return currentWave; }
	}

	const int baseGoalScore = 100; 

	public static void ClearScore(){
		score = 0; 
	}

	public static void AddScore(int points){
		score += points;
	}

	public static void AddBeeScore(int baseBeeScore){
		int points = baseBeeScore;
		score += GetModifiedScore(points);
	}

	public static void AddGoalScore(Vector3 goalPos){
		int points = baseGoalScore;

		score += points;

		GameController.instance.SpawnScore(points, goalPos);
	}

	public static void AdvanceWave(){
		currentWave++;
		GameController.audioController.SetPitch(currentWave % 12);
	}

	public static void ResetWaves(){
		currentWave = 0;
		GameController.audioController.SetPitch(0);
	}

	public static int GetModifiedScore(int points){
		points *= (currentWave+1);
		return points;
	}

	public static void RespawnPenalty(){
		score = Mathf.FloorToInt(score / 2);	
	}

	public static int GetRespawnPenalty(){
		return Mathf.FloorToInt(score / 2);	
	}

	public static bool CheckHighScore(int score){
		int temp = 0;
		bool success = false;
		for(int i = 0; i < highScores.Length; i++){
			if(score > highScores[i]){
				success = true;
				temp = highScores[i];
				highScores[i] = score;
				score = temp;
			}
		}
		return success;
	}

	public static void CheckCurrentRun(){
		CheckHighScore(CurrentScore);
	}
	
}
