using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{
	Title,
	Game,
	Scores,
	Credits
}

public class GameController : MonoBehaviour
{

	public static GameController instance;	
	public static AudioController audioController;

	static List<Bee> beeList = new List<Bee>();
	static List<Bee> activeBees = new List<Bee>();
	static List<Bee> waveBees = new List<Bee>();

	public static ScoreTicker scoreTicker;

	public Bee playerBee;

	static GameState gameState;
	public static GameState GetGameState{
		get { return gameState; }
	}

	public static BeeSpawner beeSpawner;

	public static GameObject titleScreen;
	public static GameObject creditScreen;
	public static GameObject scoreScreen;
	public static GameObject respawnMenu;
	public static GameObject settingsMenu;
	
	public static BeeTrail beeTrail;

	public Score scorePrefab;


	[Range(5,25)]
	public float minHitForce = 5f, maxHitForce = 25f;
	bool charging;
	public bool IsCharging{
		get { return charging; }
	}
	const float maxChargeTime = 0.8f;
	static Timer chargeTimer = new Timer(maxChargeTime);
	public float ChargeProgress{
		get { return chargeTimer.CompletionPercentage; }
	}


	public static Timer bounceTimer = new Timer(0.2f);


	// Start is called before the first frame update
	void Awake()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else if(instance != this) {
			Destroy(this.gameObject);
		}
		gameState = GameState.Title;
	}

	void Start(){
		instance.GoToTitle();
		audioController.PlayTitleJingle();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(gameState == GameState.Title){
				Application.Quit();
			}
			else if(gameState != GameState.Game){
				instance.GoToTitle();
			}
		}


		if(GameSettings.DEBUG){
			if(Input.GetKeyDown(KeyCode.F10)){
				DataManager.SaveScoreData();
			}
			if(Input.GetKeyDown(KeyCode.F9)){
				DataManager.LoadScoreData();
			}
			if(Input.GetKeyDown(KeyCode.O) && gameState == GameState.Game){
				ClearWave();
			}
		}
		
		if(GameSettings.DEBUG && Input.GetKeyDown(KeyCode.RightAlt)){
			string time = System.DateTime.Now.ToString("yyyy'-'MM'-'dd'--'HH'-'mm'-'ss");
			string path = System.IO.Path.Combine(Application.persistentDataPath, "Pictures/screenshot " + time + ".png");
			ScreenCapture.CaptureScreenshot(path);
			Debug.Log("Screen capture saved! " + path);
		}

		if(bounceTimer.IsActive){
			bounceTimer.AdvanceTimer(Time.deltaTime);
			if(bounceTimer.IsFinished){
				bounceTimer.Reset();
			}
		}
		if(gameState == GameState.Game){
			ProcessInputs();
			if(waveBees.Count == 0){
				ClearWave();
			}
		}
		else if(gameState == GameState.Title){
		/*	if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
				StartGame();
			}*/
		}
		
	}

	void ProcessInputs(){
		if( Input.GetKeyDown(KeyCode.Space)){
			RespawnPlayer();
			return;
		}

		if(beeSpawner.spawnTimer.IsActive){
			return;
		}

		if(Input.GetMouseButton(0) && !charging){
			audioController.PlayStretchClip();
			charging = true;
			chargeTimer.Reset();
			chargeTimer.Start();
		}
		if(charging && Input.GetMouseButtonUp(0)){
			audioController.StopStretchClip();
			Vector3 mousePos = GetMousePos();
			//Debug.Log("M" + mousePos);
			charging = false;
			float force = Mathf.Lerp(minHitForce, maxHitForce, chargeTimer.CompletionPercentage);

			foreach(Bee b in activeBees){
				if(b.targetArrow != null){
					//b.targetArrow.Lock();
					b.targetArrow.ResetArrow();
				}
				b.HitFromPosition(mousePos, force);
			}

			chargeTimer.Reset();
		}

		if(charging){
			chargeTimer.AdvanceTimer(Time.deltaTime);
		}

		
	}

	public static Vector3 GetMousePos(){
		Vector3 mousePos = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		Plane plane = new Plane(Vector3.up, Vector3.zero);
		float dist;
		plane.Raycast(ray, out dist);
		return ray.GetPoint(dist);
	}

	static void ClearBees(){
		foreach(Bee b in beeList){
			b.expire = true;
		}
		beeList.Clear();
		instance.playerBee = null;
		activeBees.Clear();
		waveBees.Clear();
	}

	public static void AddBee(Bee b){
		if(!beeList.Contains(b)){
			beeList.Add(b);
		}
	}
	
	public static void RemoveBee(Bee b){
		if(beeList.Contains(b)){
			beeList.Remove(b);
		}
		if(activeBees.Contains(b)){
			activeBees.Remove(b);
		}
		if(waveBees.Contains(b)){
			waveBees.Remove(b);
		}
	}

	public static void ActivateBee(Bee b){
		if(!activeBees.Contains(b)){
			activeBees.Add(b);
		}
	}

	public static void DeactivateBee(Bee b){
		if(activeBees.Contains(b)){
			activeBees.Remove(b);
		}
		if(waveBees.Contains(b)){
			waveBees.Remove(b);
		}
	}

	static void ClearActiveBees(){
		activeBees.Clear();
	}

	public void StartGame(){
		titleScreen.SetActive(false);
		audioController = GetComponent<AudioController>();
		if(audioController != null){
			audioController.PlayGameJingle();
		}
		gameState = GameState.Game;
		SpawnPlayer();
		StartWave(0);
	}

	// spawn enemy bees
	static void StartWave(int wave){
		SpawnZone[] zones = SpawnWaves.GetWave(wave); 
		foreach(SpawnZone zone in zones){
			Bee[] bList = beeSpawner.SpawnBees(zone);
			foreach(Bee bee in bList){
				waveBees.Add(bee);
			}
		}
	}

	static void ClearWave(){
		if(waveBees.Count > 0){
			foreach(Bee b in waveBees){
				b.expire = true;
			}
		}
		waveBees.Clear();
		ScoreController.AdvanceWave();
		StartWave(ScoreController.CurrentWave);
	}

	public static void ResetGame(){
		audioController.PlayTitleJingle();
		ScoreController.ClearScore();
		ScoreController.ResetWaves();
		ClearBees();
		instance.charging = false;
	//	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		instance.GoToTitle();
	}

	public void GoToTitle(){
		gameState = GameState.Title;
		titleScreen.SetActive(true);
		scoreScreen.SetActive(false);
		creditScreen.SetActive(false);
		respawnMenu.SetActive(false);
		CloseSettingsMenu();
	}
	public void GoToCredits(){
		gameState = GameState.Credits;
		titleScreen.SetActive(false);
		scoreScreen.SetActive(false);
		creditScreen.SetActive(true);
		respawnMenu.SetActive(false);
		CloseSettingsMenu();
	}
	public void GoToScores(){
		gameState = GameState.Scores;
		titleScreen.SetActive(false);
		scoreScreen.SetActive(true);
		creditScreen.SetActive(false);
		respawnMenu.SetActive(false);
		CloseSettingsMenu();
	}
	
	public void OpenRespawnMenu(){
		Time.timeScale = 0;
		respawnMenu.SetActive(true);
	}

	public void CloseRespawnMenu(){
		Time.timeScale = 1;
		respawnMenu.SetActive(false);
	}

	public void OpenSettingsMenu(){
		Time.timeScale = 0;
		settingsMenu.SetActive(true);
	}

	public void CloseSettingsMenu(){
		Time.timeScale = 1;
		settingsMenu.SetActive(false);
	}

	void SpawnPlayer(){
		beeTrail.gameObject.SetActive(false);
		playerBee = beeSpawner.SpawnPlayerBee(Vector3.zero);
	}

	public void RespawnPlayer(){
		if(playerBee != null){
			playerBee.Terminate();
		}
		ScoreController.RespawnPenalty();
		playerBee = beeSpawner.SpawnPlayerBee(Vector3.zero);
	}

	public static void PlayerReady(){
		if(instance.playerBee != null){
			beeTrail.transform.position = instance.playerBee.transform.position;
			beeTrail.gameObject.SetActive(true);
		}
	}

	public void SpawnScore(int points, Vector3 pos){
		Score scoreSpawn = Instantiate(scorePrefab);
		scoreSpawn.transform.position = pos;
		scoreSpawn.SetPoints(points);
	}


}
