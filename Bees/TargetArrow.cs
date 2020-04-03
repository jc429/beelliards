using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetArrow : MonoBehaviour
{
	public RectTransform arrowBase;

	public Image arrowImg;

	[Range(0, 5)]
	public float minScale, maxScale;

	public Gradient colorGradient;

	const float lockTime = 0.4f;

	// Start is called before the first frame update
	void Start()
	{
		ResetArrow();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void LateUpdate(){
		if(GameController.instance.IsCharging){
			ShowArrow();
			SetProgress(GameController.instance.ChargeProgress);
		}
		UpdateAngle();
	}

	public void SetProgress(float completion){
		completion = Mathf.Clamp(completion, 0, 1);
		SetScale(completion);
		SetColor(completion);		
	}

	public void ShowArrow(){
		arrowBase.gameObject.SetActive(true);
	}

	public void HideArrow(){
	//	arrowBase.gameObject.SetActive(false);
	}

	void SetScale(float f){
		float scale = Mathf.Lerp(minScale, maxScale, f);
		arrowBase.transform.localScale = new Vector3(1, scale, 1); 
	}

	void SetColor(float f){
		Color c = colorGradient.Evaluate(f);
		arrowImg.color = c;
	}

	public void SetRotation(float degrees){
		//Debug.Log(degrees);
		GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, degrees);
	}

	public void UpdateAngle(){
		Vector3 pos = transform.position;
		Vector3 mousePos = GameController.GetMousePos();

		Vector3 dir;
		if(GameSettings.InvertControls){
			dir = mousePos - pos;
		}
		else{
			dir = pos - mousePos;
		}	
		dir = dir.normalized;
		float angle = Vector3.Angle(dir, Vector3.forward);
		if(dir.x > 0){
			angle *= -1;
		}
		SetRotation(angle);
	}

	public void ResetArrow(){
		SetProgress(0);
	}

}
