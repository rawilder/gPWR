using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreAllocationScript : MonoBehaviour {

	public Text messageText;

	void Start(){
		bool availableBonuses = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
		if (DataScript.scenario.control) {
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
				if(availableBonuses){
					if(DataScript.scenario.ScoreWeightAvailable){
						if(DataScript.scenario.ScoreWeightPredetermined){
							messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesNoWeight;
						}
						else{
							messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesAndWeight;
						}
					}
					else{
						messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesNoWeight;
					}
				}
				else{
					if(DataScript.scenario.ScoreWeightAvailable){
						if(DataScript.scenario.ScoreWeightPredetermined){
							//cant have high power and no bonuses and predetermined weights, that's just a control
						}
						else{
							messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextWeightNoBonuses;
						}
					}
					else{
						//this should never happen
					}
				}
			}
			else{
				if(availableBonuses){
					if(DataScript.scenario.ScoreWeightAvailable){
						if(DataScript.scenario.ScoreWeightPredetermined){
							messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesNoWeight;
						}
						else{
							messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesAndWeight;
						}
					}
					else{
						messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesNoWeight;
					}
				}
				else{
					if(DataScript.scenario.ScoreWeightAvailable){
						if(DataScript.scenario.ScoreWeightPredetermined){
							//cant have high power and no bonuses and predetermined weights, that's just a control
						}
						else{
							messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextWeightNoBonuses;
						}
					}
					else{
						//this should never happen
					}
				}
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
			bool availableBonuses = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
			if(availableBonuses){
				Application.LoadLevel(7);
				return;
			}

			if(DataScript.scenario.ScoreWeightAvailable){
				Application.LoadLevel(11);
			}
		}

	}

}
