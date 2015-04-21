using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreAllocationScript : MonoBehaviour {

	public Text messageText;

	void Start(){
		bool availableBonuses = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
		if (!DataScript.scenario.powerUpsPredetermined) {
			if (DataScript.scenario.control) {
				if (availableBonuses) {
					if (DataScript.scenario.ScoreWeightAvailable) {
						messageText.text = DataScript.tutText.PreallocationScreenControlBonusAndWeightText;
					} else {
						messageText.text = DataScript.tutText.PreallocationScreenControlBonusNoWeightText;
					}
				} else {
					if (DataScript.scenario.ScoreWeightAvailable) {
						messageText.text = DataScript.tutText.PreallocationScreenControlNoBonusWeightText;
					} else {
						messageText.text = DataScript.tutText.PreallocationScreenControlNoBonusNoWeightText;
					}
				}

			} else {
				if (DataScript.scenario.playerHasHighPower) {
					if (availableBonuses) {
						if (DataScript.scenario.ScoreWeightAvailable) {
							if (DataScript.scenario.ScoreWeightPredetermined) {
								messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesNoWeight;
							} else {
								messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesAndWeight;
							}
						} else {
							messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesNoWeight;
						}
					} else {
						if (DataScript.scenario.ScoreWeightAvailable) {
							if (DataScript.scenario.ScoreWeightPredetermined) {
								//cant have high power and no bonuses and predetermined weights, that's just a control
							} else {
								messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextWeightNoBonuses;
							}
						} else {
							messageText.text = DataScript.tutText.PreallocationScreenNoWeightsNoBonuses;
						}
					}
				} else {
					//player has low power
					if (availableBonuses) {
						if (DataScript.scenario.ScoreWeightAvailable) {
							if (DataScript.scenario.ScoreWeightPredetermined) {
								messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesNoWeight;
							} else {
								messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesAndWeight;
							}
						} else {
							messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesNoWeight;
						}
					} else {
						if (DataScript.scenario.ScoreWeightAvailable) {
							if (DataScript.scenario.ScoreWeightPredetermined) {
								//cant have high power and no bonuses and predetermined weights, that's just a control
							} else {
								messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextWeightNoBonuses;
							}
						} else {
							messageText.text = DataScript.tutText.PreallocationScreenNoWeightsNoBonuses;
						}
					}
				}
			}
		} else {
			if(!DataScript.scenario.playerHasHighPower && !DataScript.scenario.control && availableBonuses){
				messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextBonusesPredetermined;
			}
			else if(!DataScript.scenario.playerHasHighPower && !DataScript.scenario.control && !availableBonuses){
				messageText.text = DataScript.tutText.PreallocationScreenLowPowerTextNoBonusesPredetermined;
			}
			else if(DataScript.scenario.playerHasHighPower && !DataScript.scenario.control && availableBonuses){
				messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextBonusesPredetermined;
			}
			else if(DataScript.scenario.playerHasHighPower && !DataScript.scenario.control && !availableBonuses){
				messageText.text = DataScript.tutText.PreallocationScreenHighPowerTextNoBonusesPredetermined;
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
				if(DataScript.scenario.powerUpsPredetermined){
					//set the power ups

					if(DataScript.scenario.pSpeedIncreaseAvailable){
						DataScript.alloc.PlayerSpeed = (int) DataScript.scenario.AiAllocatePlayerSpeed;
						Debug.Log("pspeed: " + DataScript.alloc.PlayerSpeed);
					}
					else{
						DataScript.alloc.PlayerSpeed = -1;
					}

					if(DataScript.scenario.gSpeedDecreaseAvailable){
						DataScript.alloc.GhostSpeed = (int) DataScript.scenario.AiAllocateGhostSpeed;
						Debug.Log("gspeed: " + DataScript.alloc.GhostSpeed);
					}
					else{
						DataScript.alloc.GhostSpeed = -1;
					}
	

					if(DataScript.scenario.fRespawnAvailable){
						DataScript.alloc.FruitRespawn = (int) DataScript.scenario.AiAllocateFruitRespawn;
						Debug.Log("frespawn: " + DataScript.alloc.FruitRespawn);
					}
					else{
						DataScript.alloc.FruitRespawn = -1;
					}
		
					if(DataScript.scenario.longerPowerModeAvailable){
						DataScript.alloc.LongerPowerMode = (int) DataScript.scenario.AiAllocateLongerPowerMode;
						Debug.Log("longerpm: " + DataScript.alloc.LongerPowerMode);
					}
					else{
						DataScript.alloc.LongerPowerMode = -1;
					}

					if(DataScript.scenario.powerballRespawnAvailable){
						DataScript.alloc.PowerBallRespawn = (int) DataScript.scenario.AiAllocatePowerBallRespawn;
						Debug.Log("powerballrespawn: " + DataScript.alloc.PowerBallRespawn);
					}
					else{
						DataScript.alloc.PowerBallRespawn = -1;
					}

					if(DataScript.scenario.gRespawnAvailable){
						DataScript.alloc.GhostRespawn = (int) DataScript.scenario.AiAllocateGhostRespawn;
						Debug.Log("ghsotre: " + DataScript.alloc.GhostRespawn);
					}
					else{
						DataScript.alloc.GhostRespawn = -1;
					}

					if(DataScript.scenario.gDumbAvailale){
						DataScript.alloc.DumbGhosts = (int) DataScript.scenario.AiAllocateDumbGhosts;
						Debug.Log("dumb: " + DataScript.alloc.DumbGhosts);
					}
					else{
						DataScript.alloc.DumbGhosts = -1;
					}

					if(DataScript.scenario.gFewerAvailable){
						DataScript.alloc.FewerGhosts = (int) DataScript.scenario.AiAllocateFewerGhosts;
						Debug.Log("few: " + DataScript.alloc.FewerGhosts);
					}
					else{
						DataScript.alloc.FewerGhosts = -1;
					}

					if(DataScript.scenario.ScoreWeightAvailable && DataScript.scenario.ScoreWeightPredetermined){
						DataScript.alloc.scoreWeight = DataScript.scenario.AiAllocateWeight;
					}

					Application.LoadLevel(8);
				}
				else{
					Application.LoadLevel(7);
				}
				return;
			}

			if(DataScript.scenario.ScoreWeightAvailable){
				Application.LoadLevel(11);
				return;
			}

			Application.LoadLevel(12);
		}

	}

}
