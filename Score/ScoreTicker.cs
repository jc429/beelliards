using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTicker : MonoBehaviour
{

	public ScoreTick tickPrefab;

	Vector3 tickSpacing = new Vector3(0, 0, 0);

	List<ScoreTick> tickList = new List<ScoreTick>();


	// Start is called before the first frame update
	void Awake()
	{
		GameController.scoreTicker = this;
	}

	// Update is called once per frame
	void Update()
	{
			
	}

	public void SpawnTick(int score){
		ScoreTick tick = Instantiate(tickPrefab);

		tick.transform.SetParent(this.transform);
		tick.SetTickerPosition(tickSpacing * tickList.Count, true);
		tick.SetScore(score);
		tick.StartAnim();

		tickList.Add(tick);
	}

	public void RemoveTick(ScoreTick t){
		tickList.Remove(t);
		Recalculate();
	}

	void Recalculate(){
		for(int i = 0; i < tickList.Count; i++){
			tickList[i].SetTickerPosition(tickSpacing * i);
		}
	}
}
