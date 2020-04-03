using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputStyle{
	Hold = 1,
	Swipe = 2
}

public static class GameSettings {

	const bool __DEBUG = true;
	public static bool DEBUG {
		get { return __DEBUG; }
	}

  static bool invertControls = false; 
	public static bool InvertControls {
		get { return invertControls; }
	}
	public static void SetControlsInverted(bool val){
		invertControls = val;
	}

	static InputStyle inputStyle = InputStyle.Hold; 
	public static InputStyle GetInputStyle(){
		return inputStyle;
	} 
	public static int GetInputStyleInt(){
		return (int)inputStyle;
	}
	public static void SetInputStyle(InputStyle style){
		inputStyle = style;
	}
	public static void SetInputStyle(int styleInt){
		styleInt = Mathf.Clamp(styleInt, 1, 2);
		inputStyle = (InputStyle)styleInt;
	}

}
