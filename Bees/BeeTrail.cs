using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeTrail : MonoBehaviour
{
	// Start is called before the first frame update
	void Awake()
	{
		GameController.beeTrail = this;
	}

	// Update is called once per frame
	void Update()
	{
		if(GameController.instance.playerBee != null){
			transform.position = GameController.instance.playerBee.transform.position;
		}
	}
}
