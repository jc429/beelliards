using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
  void Awake(){
		GameController.settingsMenu = this.gameObject;
	}
  void Start(){
		GameController.settingsMenu = this.gameObject;
	}

	public void GoToTitle(){
		GameController.instance.GoToTitle();
	}

	public void SetControlsInverted(bool val){
		GameSettings.SetControlsInverted(val);
	}

	public void SetInputStyle(InputStyle style){
		GameSettings.SetInputStyle(style);
	}
}
