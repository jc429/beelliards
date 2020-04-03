using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{

	public SpawnArea[] spawnAreas;

	public Transform beeContainer;

	public Bee beePrefab;
	public Bee playerPrefab;
	public GameObject smokePrefab;

	public Timer spawnTimer = new Timer(1.325f);
	const float spawnHeight = 8f;

	// Start is called before the first frame update
	void Awake()
	{
		GameController.beeSpawner = this;
		spawnTimer.Reset();
	}

	void Update(){
		spawnTimer.AdvanceTimer(Time.deltaTime);
		if(spawnTimer.IsFinished){
			GameController.PlayerReady();
			spawnTimer.Reset();
		}
	}
	
	public Bee[] SpawnBees(SpawnZone zone){
		SpawnArea area = spawnAreas[(int)zone];
		Bee[] beeList = new Bee[area.nodes.Length];
		for(int i = 0; i < area.nodes.Length; i++){
			beeList[i] = SpawnBee(area.nodes[i].transform.position);
		}
		return beeList;
	}

	Bee SpawnBee(Vector3 pos){
		Bee bee = Instantiate(beePrefab);
		bee.transform.parent = beeContainer;
		bee.transform.position = pos;

		GameObject smoke = Instantiate(smokePrefab);
		smoke.transform.position = pos;

		return bee;
	}

	public Bee SpawnPlayerBee(Vector3 pos){
		pos.y = spawnHeight;
		Bee bee = Instantiate(playerPrefab);
		bee.transform.position = pos;
		spawnTimer.Reset();
		spawnTimer.Start();
		return bee;
	}
}
