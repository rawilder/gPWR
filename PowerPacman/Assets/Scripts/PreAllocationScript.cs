using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreAllocationScript : MonoBehaviour {

	Text messageText;

	void Start(){

		messageText = GameObject.Find ("Message").GetComponent<Text> ();

		if (DataScript.scenario.control) {

			messageText.text = "This is some placeholder text letting the user know that the game will be beginning soon (no allocation)";

		} else {
			if(DataScript.scenario.playerHasHighPower){
				messageText.text = "This is some placeholder text telling the user that they will be allocating power ups to themselves and the other player";
			}
			else{
				messageText.text = "This is some placeholder text telling the user that they will have powerups allocated to them by the other player";
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
