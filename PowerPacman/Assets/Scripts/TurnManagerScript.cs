using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TurnManagerScript : MonoBehaviour {

	public static bool isPlayerTurn = true;
	public static bool paused = true;
	public static bool pregame = true;

	public static float pregameCountdownDelay = 5.0f;
	public static float pregameCountdownRemaining = 0.0f;

	Text countdown;

	void Start(){
		pregameCountdownRemaining = pregameCountdownDelay;

//		countdown = GameObject.Find ("CountdownText").GetComponent<Text> ();
	}

	void FixedUpdate(){
		if (pregameCountdownRemaining > 0) {
			pregameCountdownRemaining -= Time.deltaTime;
//			countdown.text = "" + (int) Math.Ceiling(pregameCountdownRemaining);
		} else {
			pregame = false;
			paused = false;
		}
	}

}
