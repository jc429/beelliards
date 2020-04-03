using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DataManager
{
	static string dataPath = System.IO.Path.Combine(Application.persistentDataPath, "game_data");
	static string settingsFile = "settings.cfg";
	static string scoreFile = "scores.cfg";

	/***** Increment this with every change to the cfg formats *****/
	const int currentFormat = 2;

	public static void SaveSettingData(){
		CheckDirectoryValid(dataPath);
		string path = Path.Combine(dataPath, settingsFile);
		Debug.Log("Saving to: " + path);
		using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) {
			writer.Write(currentFormat);
			
			if(currentFormat >= 1){
				writer.Write(GameSettings.InvertControls);
			}
			if(currentFormat >= 2){
				writer.Write(GameSettings.GetInputStyleInt());
			}
		}
	}


	public static void LoadSettingData(){
		CheckDirectoryValid(dataPath);
		string path = Path.Combine(dataPath, settingsFile);
		Debug.Log("Loading from: " + path);
		FileStream f = null;
		if (!File.Exists(path)) {
			Debug.LogError("File does not exist: " + path);
			return;
		}
		else{
			f = File.OpenRead(path);
		}
		using (BinaryReader reader = new BinaryReader(f)) {
			try{
				int[] scores = new int[ScoreController.NumScores];
				int headerID = reader.ReadInt32();

				if(headerID <= 0 || headerID > currentFormat){
					Debug.Log("Invalid Header, Aborting. " + headerID);
				}

				if(headerID >= 1){
					bool invControls = reader.ReadBoolean();
					GameSettings.SetControlsInverted(invControls);
				}

			}
			catch(IOException e){
				Debug.Log(e);
				f.Close();
				return;
			}
		}

	}	
	
	public static void SaveScoreData(){
		CheckDirectoryValid(dataPath);
		string path = Path.Combine(dataPath, scoreFile);
		Debug.Log("Saving to: " + path);
		using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) {
			writer.Write(currentFormat);
			
			int[] scores = ScoreController.GetHighScores();

			if(currentFormat >= 1){
				for(int i = 0; i < ScoreController.NumScores; i++){
					writer.Write(scores[i]);
				}
			}
		}
	}


	public static void LoadScoreData(){
		CheckDirectoryValid(dataPath);
		string path = Path.Combine(dataPath, scoreFile);
		Debug.Log("Loading from: " + path);
		FileStream f = null;
		if (!File.Exists(path)) {
			Debug.LogError("File does not exist: " + path);
			return;
		}
		else{
			f = File.OpenRead(path);
		}
		using (BinaryReader reader = new BinaryReader(f)) {
			try{
				int[] scores = new int[ScoreController.NumScores];
				int headerID = reader.ReadInt32();

				if(headerID <= 0 || headerID > currentFormat){
					Debug.Log("Invalid Header, Aborting. " + headerID);
				}

				if(headerID >= 1){
					for(int i = 0; i < ScoreController.NumScores; i++){
						scores[i] = reader.ReadInt32();
						if(reader.BaseStream.Position == reader.BaseStream.Length){
							break;
						}
					}
				}

				ScoreController.SetHighScores(scores);
			}
			catch(IOException e){
				Debug.Log(e);
				return;
			}
		}
	}

	static bool CheckDirectoryValid(string path){
		try{
			if (!Directory.Exists(path)){
				Directory.CreateDirectory(path);
			}
			return true;
		}
		catch(IOException e){
			Debug.Log(e.Message);	
			return false;
		}
	}

	static FileStream CheckFileValid(string path, string fileName){
		if(!CheckDirectoryValid(path)){
			return null;
		}
		try{
			return File.Open(path, FileMode.OpenOrCreate);
		}
		catch(IOException e){
			Debug.Log(e.Message);
			return null;
		}
	}
}
