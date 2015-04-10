using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerupAllocationScript : MonoBehaviour {

	Text messageText;
	public Text Title;

	void Start () {
	
		Title.text = DataScript.tutText.AllocationScreenTitle;

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

		if (!DataScript.scenario.playerHasHighPower) {
			if(!DataScript.scenario.AiAllocationIsRandom){

				//apply the power ups
				if(DataScript.scenario.pSpeedIncreaseAvailable){
					if(DataScript.scenario.AiAllocatePlayerSpeed == 0){
						DataScript.alloc.PlayerSpeed = 0;
					}
					else{
						DataScript.alloc.PlayerSpeed = 1;
					}
				}

				if(DataScript.scenario.gSpeedDecreaseAvailable){
					if(DataScript.scenario.AiAllocateGhostSpeed == 0){
						DataScript.alloc.GhostSpeed = 0;
					}
					else{
						DataScript.alloc.GhostSpeed = 1;
					}
				}

				if(DataScript.scenario.fRespawnAvailable){
					if(DataScript.scenario.AiAllocateFruitRespawn == 0){
						DataScript.alloc.FruitRespawn = 0;
					}
					else{
						DataScript.alloc.FruitRespawn = 1;
					}
				}

				if(DataScript.scenario.longerPowerModeAvailable){
					if(DataScript.scenario.AiAllocateLongerPowerMode == 0){
						DataScript.alloc.LongerPowerMode = 0;
					}
					else{
						DataScript.alloc.LongerPowerMode = 1;
					}
				}

				if(DataScript.scenario.powerballRespawnAvailable){
					if(DataScript.scenario.AiAllocatePowerBallRespawn == 0){
						DataScript.alloc.PowerBallRespawn = 0;
					}
					else{
						DataScript.alloc.PowerBallRespawn = 1;
					}
				}

				if(DataScript.scenario.gRespawnAvailable){
					if(DataScript.scenario.AiAllocateGhostRespawn == 0){
						DataScript.alloc.GhostRespawn = 0;
					}
					else{
						DataScript.alloc.GhostRespawn = 1;
					}
				}

				if(DataScript.scenario.gDumbAvailale){
					if(DataScript.scenario.AiAllocateDumbGhosts == 0){
						DataScript.alloc.DumbGhosts = 0;
					}
					else{
						DataScript.alloc.DumbGhosts = 1;
					}
				}

				if(DataScript.scenario.gFewerAvailable){
					if(DataScript.scenario.AiAllocateFewerGhosts == 0){
						DataScript.alloc.FewerGhosts = 0;
					}
					else{
						DataScript.alloc.FewerGhosts = 1;
					}
				}
			}
			else{
				//random allocation
				//TODO


			}
		}
	}
}
