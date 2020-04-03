using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScreen : MonoBehaviour
{
  void Awake(){
		GameController.creditScreen = this.gameObject;
	}
  void Start(){
		GameController.creditScreen = this.gameObject;
	}

	public void GoToTitle(){
		GameController.instance.GoToTitle();
	}
}
