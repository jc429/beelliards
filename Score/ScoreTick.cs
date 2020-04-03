using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TickState{
	FadeIn,
	Hang,
	FadeOut,
}

public class ScoreTick : MonoBehaviour
{
	RectTransform _rTransform{
		get { return GetComponent<RectTransform>(); }
	}

	TickState state;

  public int score;

	const float moveDuration = 0.3f;
	Timer moveTimer = new Timer(moveDuration);
	const float hangTime = 0.1f;
	const float fadeDuration = 0.4f;

	Vector3 tickStart = new Vector3(0, -350, 0);
	Vector3 tickEnd = new Vector3(0, 0, 0);
	Vector3 tickFling = new Vector3(50, 0, 0);

	public TextMeshProUGUI _text;

	Vector3 tickerPos = Vector3.zero;

	public void SetScore(int sc){
		score = sc;
		_text.text = "+" + sc;
	}

	public void StartAnim(){
		state = TickState.FadeIn;
		moveTimer.SetDuration(moveDuration);
		moveTimer.Start();
	}

	void Update(){
		if(_rTransform.localPosition != tickerPos){
			_rTransform.localPosition = Vector3.MoveTowards(_rTransform.localPosition, tickerPos, 40 * Time.deltaTime);
		}

		if(moveTimer.IsActive){
			moveTimer.AdvanceTimer(Time.deltaTime);

			switch(state){
				case TickState.FadeIn:
					_text.transform.localPosition = Vector3.Lerp(tickStart, tickEnd, moveTimer.CompletionPercentage);
					if(moveTimer.IsFinished){
						moveTimer.Reset();
						state = TickState.Hang;
						moveTimer.SetDuration(hangTime);
						moveTimer.Start();
					}
					break;
				case TickState.Hang:
					if(moveTimer.IsFinished){
						moveTimer.Reset();
						state = TickState.FadeOut;
						moveTimer.SetDuration(fadeDuration);
						moveTimer.Start();
					}
					break;
				case TickState.FadeOut:
					_text.transform.localPosition = Vector3.Lerp(tickEnd, tickFling, moveTimer.CompletionPercentage);
					_text.color = Color.Lerp(Color.white, new Color(1,1,1,0), moveTimer.CompletionPercentage);
					if(moveTimer.IsFinished){
						GameController.scoreTicker.RemoveTick(this);
						Destroy(this.gameObject);
					}
					break;

			}
		}
	}

	public void SetTickerPosition(Vector3 pos, bool forceImmediate = false){
		tickerPos = pos;
		if(forceImmediate){
			_rTransform.localPosition = pos;
		}
	}
}
