using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tutorialTurnStealScript : MonoBehaviour {

	Text title;
	Text playerGoesFirstText, turnStealText, continueText;

	// Use this for initialization
	void Start () {

		title = GameObject.Find ("Title").GetComponent<Text> ();
		playerGoesFirstText = GameObject.Find ("PlayerGoesFirstText").GetComponent<Text> ();
		turnStealText = GameObject.Find ("TurnStealText").GetComponent<Text> ();
		continueText = GameObject.Find ("ContinueText").GetComponent<Text> ();


		playerGoesFirstText.text = DataScript.tutText.PregamePlayerGoesFirstText;
		if (DataScript.scenario.hpStealsTurnsAvailable && DataScript.scenario.playerHasHighPower && !DataScript.scenario.control) {
			turnStealText.text = DataScript.tutText.PregamePlayerStealsTurnsText.Replace ("[turnSteals]", "" + DataScript.scenario.turnStealLimit);
		} else if (DataScript.scenario.hpStealsTurnsAvailable && !DataScript.scenario.playerHasHighPower && !DataScript.scenario.control) {
			turnStealText.text = DataScript.tutText.PregamePlayerStealsTurnsLowPowerText.Replace ("[turnSteals]", "" + DataScript.scenario.turnStealLimit);
		} else {
			turnStealText.text = "";
		}
		continueText.text = DataScript.tutText.PregameContinueText;

	}
}
