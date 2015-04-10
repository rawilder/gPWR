using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreAllocationScript : MonoBehaviour {

	public Text messageText;

	void Start(){

		if (DataScript.scenario.control) {

			messageText.text = DataScript.tutText.PreallocationScreenControlText;

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
			//head right into the game
			Application.LoadLevel (2);
		} else {
			//advance to allocation stage
			Application.LoadLevel(7);
		}

	}

}
