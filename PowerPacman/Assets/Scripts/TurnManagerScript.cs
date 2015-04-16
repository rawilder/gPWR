using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;

public class TurnManagerScript : MonoBehaviour {

	public static bool isPlayerTurn = true;
	public static bool paused = true;
	public static bool pregame = true;
	public static bool switchingTurnsStage = false;
	public static bool stealingTurn = false;
	public static int stolenTurnCount = 0;
	bool gameOver = false;

	public static float pregameCountdownDelay = 5.0f;
	public static float pregameCountdownRemaining = 0.0f;

	public static float turnSwitchDelay = 5.0f;
	public static float turnSwitchTimeRemaining = 0.0f;

	int totalTimeLimit = DataScript.scenario.totalTime;

	float totalTimeCounter = 0.0f;

	float aiTurnsteelDelayRemaining = 2.2f;

	Text countdown;
	Text messageText;
	Text takeTurnMessage;

	public static int targetScore = 0;

	void Start(){
		pregameCountdownRemaining = pregameCountdownDelay;
		turnSwitchTimeRemaining = turnSwitchDelay;
		countdown = GameObject.Find ("CountdownText").GetComponent<Text> ();
		messageText = GameObject.Find ("MessageText").GetComponent<Text> ();
		takeTurnMessage = GameObject.Find ("TakeTurnMessage").GetComponent<Text> ();

		takeTurnMessage.enabled = false;
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

			if (totalTimeCounter > totalTimeLimit * 60) {
				messageText.text = DataScript.tutText.GameEndMessage;
				paused = true;
				gameOver = true;
				countdown.text = "";
				return;
			}

			if (isPlayerTurn && !DataScript.scenario.control && DataScript.scenario.playerHasHighPower && stolenTurnCount < DataScript.scenario.turnStealLimit) {
				takeTurnMessage.enabled = true;
				if(Input.GetKey(KeyCode.F)){
					stealingTurn = true;
					takeTurnMessage.text = DataScript.tutText.GameTakeTurnYesMessageHighPower;
				}
			}else if(!isPlayerTurn && !DataScript.scenario.control && !DataScript.scenario.playerHasHighPower && stolenTurnCount < DataScript.scenario.turnStealLimit){
				//AI chooses whether to steal a turn or not
				takeTurnMessage.enabled = true;

				//add a bit of delay
				if(aiTurnsteelDelayRemaining > 0.0f){
					aiTurnsteelDelayRemaining -= Time.deltaTime;
				}
				else{
					if(DataScript.aiScore < (1.05 * DataScript.playerScore)){
						takeTurnMessage.text = DataScript.tutText.GameTakeTurnYesMessageLowPower;
						stealingTurn = true;
					}
				}

			}else {
				takeTurnMessage.enabled = false;
			}

			messageText.text = "Turn beginning in";
			paused = true;
			if (turnSwitchTimeRemaining > 0) {
				turnSwitchTimeRemaining -= Time.deltaTime;
				countdown.text = "" + (int)Math.Ceiling (turnSwitchTimeRemaining);
			} else {
				switchingTurnsStage = false;
				turnSwitchTimeRemaining = turnSwitchDelay; //reset for next switch
				messageText.text = "";
				countdown.text = "";
				paused = false;
				if(!stealingTurn){
					if (isPlayerTurn) {
						isPlayerTurn = false;
					} else {
						isPlayerTurn = true;
					}
				}
				else{
					stealingTurn = false;
					stolenTurnCount++;
				}
			}
		} else {
			if(DataScript.scenario.playerHasHighPower){
				takeTurnMessage.text = DataScript.tutText.GameTakeTurnMessageHighPower;
			}
			else{
				takeTurnMessage.text = DataScript.tutText.GameTakeTurnMessageLowPower;
			}
			takeTurnMessage.enabled = false;
			aiTurnsteelDelayRemaining = 2.2f;
		}
	}

}
