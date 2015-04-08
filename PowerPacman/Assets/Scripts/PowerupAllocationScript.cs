using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerupAllocationScript : MonoBehaviour {

	Text messageText;


	void Start () {
	
		messageText = GameObject.Find ("TempText").GetComponent<Text> ();

		if (DataScript.scenario.playerHasHighPower) {
			messageText.text = "The real player will be allocating. ";
		} else {
			messageText.text = "The ai will be allocating. ";
		}

		messageText.text += " Available power ups: ";

		if (DataScript.scenario.pSpeedIncreaseAvailable) {
			messageText.text += " Player speed increase,";
		}

		if (DataScript.scenario.gSpeedDecreaseAvailable) {
			messageText.text += " Ghost speed decrease,";
		}

		if (DataScript.scenario.fRespawnAvailable) {
			messageText.text += " Fruit respawn increase,";
		}

		if (DataScript.scenario.longerPowerModeAvailable) {
			messageText.text += " Longer power mode,";
		}

		if (DataScript.scenario.powerballRespawnAvailable) {
			messageText.text += " Power ball respawn,";
		}

		if (DataScript.scenario.gRespawnAvailable) {
			messageText.text += " Ghost slower respawn,";
		}

		if (DataScript.scenario.gDumbAvailale) {
			messageText.text += " \"Dumb\" ghosts,";
		}

		if (DataScript.scenario.gFewerAvailable) {
			messageText.text += " Fewer ghosts,";
		}

		if (DataScript.scenario.hpStealsTurnsAvailable) {
			messageText.text += " High power can steal turns,";
		}


	}
	

}
