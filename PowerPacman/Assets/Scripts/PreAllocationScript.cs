using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreAllocationScript : MonoBehaviour {

	public Text messageText;

	void Start(){

		if (DataScript.scenario.control) {
			bool availableBonuses = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
			if(availableBonuses){
				if(DataScript.scenario.ScoreWeightAvailable){
					messageText.text = DataScript.tutText.PreallocationScreenControlBonusAndWeightText;
				}
				else{
					messageText.text = DataScript.tutText.PreallocationScreenControlBonusNoWeightText;
				}
			}
			else{
				if(DataScript.scenario.ScoreWeightAvailable){
					messageText.text = DataScript.tutText.PreallocationScreenControlNoBonusWeightText;
				}
				else{
					messageText.text = DataScript.tutText.PreallocationScreenControlNoBonusNoWeightText;
				}
			}

		} else {
			if(DataScript.scenario.playerHasHighPower){
				messageText.text = DataScript.tutText.PreallocationScreenHighPowerText;
			}
			else{
				messageText.text = DataScript.tutText.PreallocationScreenLowPowerText;
			}
		}

	}

	public void advanceStage(){

		if (DataScript.scenario.control) {
			bool availableBonuses = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
			bool weightsActive = DataScript.scenario.ScoreWeightAvailable;
			if(availableBonuses){
				Application.LoadLevel(7);
			}
			else if(weightsActive){
				Application.LoadLevel(11);
			}
			else{
				Application.LoadLevel (2);
			}
		} else {
			//advance to allocation stage
			Application.LoadLevel(7);
		}

	}

}
