using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tutorialTurnStealScript : MonoBehaviour {

	Text title;
	Text playerGoesFirstText, turnStealText, continueText;

	// Use this for initialization
	void Start () {
		//TODO editable text

		title = GameObject.Find ("Title").GetComponent<Text> ();
		playerGoesFirstText = GameObject.Find ("PlayerGoesFirstText").GetComponent<Text> ();
		turnStealText = GameObject.Find ("TurnStealText").GetComponent<Text> ();
		continueText = GameObject.Find ("ContinueText").GetComponent<Text> ();


		playerGoesFirstText.text = "You're going to go first";
		if (DataScript.scenario.hpStealsTurnsAvailable) {
			turnStealText.text = "You're allowed to steal turns";
		}
		continueText.text = "Please press continue to begin the game";

	}
}
