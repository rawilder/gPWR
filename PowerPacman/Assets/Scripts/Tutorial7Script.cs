using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial7Script : MonoBehaviour {

	Text messageText;
	Text PlayerBonusesLabel, PartnerBonusesLabel, PlayerBonusesBox, PartnerBonusesBox;
	Text BottomMessageText;
	Button continueButton;

	// Use this for initialization
	void Start () {
	
		messageText = GameObject.Find ("MessageText").GetComponent<Text> ();
		PlayerBonusesLabel = GameObject.Find ("PlayerBonusesLabel").GetComponent<Text> ();
		PartnerBonusesLabel = GameObject.Find ("PartnerBonusesLabel").GetComponent<Text> ();
		PlayerBonusesBox = GameObject.Find ("PlayerBonusesBox").GetComponent<Text> ();
		PartnerBonusesBox = GameObject.Find ("PartnerBonusesBox").GetComponent<Text> ();
		BottomMessageText = GameObject.Find ("BottomMessageBox").GetComponent<Text> ();
		continueButton = GameObject.Find ("Button").GetComponent<Button> ();

		if (DataScript.scenario.control) {
			messageText.text = "The bonuses that you are your partner were assigned are listed below. Please press continue to begin the game";
		} else {
			if (DataScript.scenario.playerHasHighPower) {
				messageText.text = "The bonuses you have chosen for yourself and your partner are listed below. Please press continue to begin the game.";
			} else {
				messageText.text = "The bonuses your partner has chosen for you are listed below. Please press continue to begin the game";
			}
		}

		PlayerBonusesBox.text = "";
		PartnerBonusesBox.text = "";

		if (DataScript.alloc.PlayerSpeed == 1) {
			PlayerBonusesBox.text = "Player Speed Increase\n";
		} else if (DataScript.alloc.PlayerSpeed == 0) {
			PartnerBonusesBox.text = "Player Speed Increase\n";
		} else if (DataScript.alloc.PlayerSpeed == 2) {
			PlayerBonusesBox.text = "Player Speed Increase\n";
			PartnerBonusesBox.text = "Player Speed Increase\n";
		}

		if (DataScript.alloc.GhostSpeed == 1) {
			PlayerBonusesBox.text += "Enemy Speed Decrease\n";
		} else if (DataScript.alloc.GhostSpeed == 0) {
			PartnerBonusesBox.text += "Enemy Speed Decrease\n";
		}
		else if (DataScript.alloc.GhostSpeed == 2) {
			PlayerBonusesBox.text += "Enemy Speed Decrease\n";
			PartnerBonusesBox.text += "Enemy Speed Decrease\n";
		}

		if (DataScript.alloc.FruitRespawn == 1) {
			PlayerBonusesBox.text += "Fruit Respawn Increase\n";
		} else if (DataScript.alloc.FruitRespawn == 0) {
			PartnerBonusesBox.text += "Fruit Respawn Increase\n";
		}
		else if (DataScript.alloc.FruitRespawn == 2) {
			PlayerBonusesBox.text += "Fruit Respawn Increase\n";
			PartnerBonusesBox.text += "Fruit Respawn Increase\n";
		}

		if (DataScript.alloc.LongerPowerMode == 1) {
			PlayerBonusesBox.text += "Longer Super Mode\n";
		} else if (DataScript.alloc.LongerPowerMode == 0) {
			PartnerBonusesBox.text += "Longer Super Mode\n";
		}
		else if (DataScript.alloc.LongerPowerMode == 2) {
			PlayerBonusesBox.text += "Longer Super Mode\n";
			PartnerBonusesBox.text += "Longer Super Mode\n";
		}

		if (DataScript.alloc.PowerBallRespawn == 1) {
			PlayerBonusesBox.text += "Super Balls Respawn\n";
		} else if (DataScript.alloc.PowerBallRespawn == 0) {
			PartnerBonusesBox.text += "Super Balls Respawn\n";
		}
		else if (DataScript.alloc.PowerBallRespawn == 2) {
			PlayerBonusesBox.text += "Super Balls Respawn\n";
			PartnerBonusesBox.text += "Super Balls Respawn\n";
		}

		if (DataScript.alloc.GhostRespawn == 1) {
			PlayerBonusesBox.text += "Enemy Slower Respawn\n";
		} else if (DataScript.alloc.GhostRespawn == 0) {
			PartnerBonusesBox.text += "Enemy Slower Respawn\n";
		}
		else if (DataScript.alloc.GhostRespawn == 2) {
			PlayerBonusesBox.text += "Enemy Slower Respawn\n";
			PartnerBonusesBox.text += "Enemy Slower Respawn\n";
		}

		if (DataScript.alloc.DumbGhosts == 1) {
			PlayerBonusesBox.text += "\"Dumb\" Enemies\n";
		} else if (DataScript.alloc.DumbGhosts == 0) {
			PartnerBonusesBox.text += "\"Dumb\" Enemies\n";
		}
		else if (DataScript.alloc.DumbGhosts == 2) {
			PlayerBonusesBox.text += "\"Dumb\" Enemies\n";
			PartnerBonusesBox.text += "\"Dumb\" Enemies\n";
		}

		if (DataScript.alloc.FewerGhosts == 1) {
			PlayerBonusesBox.text += "Fewer Enemies\n";
		} else if (DataScript.alloc.FewerGhosts == 0) {
			PartnerBonusesBox.text += "Fewer Enemies\n";
		}
		else if (DataScript.alloc.FewerGhosts == 2) {
			PlayerBonusesBox.text += "Fewer Enemies\n";
			PartnerBonusesBox.text += "Fewer Enemies\n";
		}

		if (DataScript.scenario.ScoreWeightAvailable) {
			PlayerBonusesBox.text += "Score Weight: " + DataScript.alloc.scoreWeight * 100 + "%";
			PartnerBonusesBox.text += "Score Weight: " + (1 - DataScript.alloc.scoreWeight) * 100 + "%";
		}

		/*
		if (DataScript.scenario.hpStealsTurnsAvailable) {
			if (DataScript.scenario.playerHasHighPower) {
				string msg;
				msg = "In addition to these bonuses that you have chosen, we have determined to give you the ability to chose to keep playing after your turn has ended. This ability can be used <stealLimit> times.";
				BottomMessageText.text = msg.Replace ("<stealLimit>", "" + DataScript.scenario.turnStealLimit);
			} else {
				string msg = "In addition to these bonuses that your partner has chosen, we have determined that they will also have the ability to chose to continue to play once their turn has expired. They will be able to use this ability up to <stealLimit> times.";
				BottomMessageText.text = msg.Replace ("<stealLimit>", "" + DataScript.scenario.turnStealLimit);
			}
		} else {
			BottomMessageText.text = "";
		}
		*/

	}
	
	public void continueButtonClicked(){
		if (DataScript.scenario.ScoreWeightAvailable) {
			Application.LoadLevel(11);
		} else {
			Application.LoadLevel(12);
		}
	}

}
