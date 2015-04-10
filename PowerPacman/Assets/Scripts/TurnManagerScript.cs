using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;

public class TurnManagerScript : MonoBehaviour {

	public static bool isPlayerTurn = false;
	public static bool paused = true;
	public static bool pregame = true;
	public static bool switchingTurnsStage = false;
	bool gameOver = false;

	public static float pregameCountdownDelay = 5.0f;
	public static float pregameCountdownRemaining = 0.0f;

	public static float turnSwitchDelay = 0.0f;
	public static float turnSwitchTimeRemaining = 0.0f;

	int totalTimeLimit = 10; //minutes

	float totalTimeCounter = 0.0f;

	Text countdown;
	Text messageText;

	void Start(){
		pregameCountdownRemaining = pregameCountdownDelay;
		turnSwitchTimeRemaining = turnSwitchDelay;
		countdown = GameObject.Find ("CountdownText").GetComponent<Text> ();
		messageText = GameObject.Find ("MessageText").GetComponent<Text> ();
	}

	void FixedUpdate(){

		if (gameOver) {
			Thread.Sleep(5000);
			Application.LoadLevel(10);
		}

		totalTimeCounter += Time.deltaTime;

		if (pregame) {
			if (pregameCountdownRemaining > 0) {
				pregameCountdownRemaining -= Time.deltaTime;
				countdown.text = "" + (int)Math.Ceiling (pregameCountdownRemaining);
			} else {
				pregame = false;
				paused = false;
				countdown.text = "";
				messageText.text = "";
				countdown.text = "";
			}
		}

		if (switchingTurnsStage) {

			if(totalTimeCounter > totalTimeLimit*60){
				messageText.text = "The game has ended";
				paused = true;
				gameOver = true;
				countdown.text = "";
				return;
			}

			messageText.text = "Turn beginning in";
			paused = true;
			if(turnSwitchTimeRemaining > 0){
				turnSwitchTimeRemaining -= Time.deltaTime;
				countdown.text = "" + (int)Math.Ceiling(turnSwitchTimeRemaining);
			}
			else{
				switchingTurnsStage = false;
				turnSwitchTimeRemaining = turnSwitchDelay; //reset for next switch
				messageText.text = "";
				paused = false;
				if(isPlayerTurn){
					isPlayerTurn = false;
				}
				else{
					isPlayerTurn = false;
				}
			}
		}
	}

}
