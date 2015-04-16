using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial3Script : MonoBehaviour {

	public Text Tut3Title;
	public Text Tut3Body;

	public Transform descriptionPanel;
	public GameObject descriptionPrefab;

	// Use this for initialization
	void Start () {
	
		Tut3Title.text = DataScript.tutText.BonusesScreenTitle;
		if (DataScript.scenario.control) {
			Tut3Body.text = DataScript.tutText.BonusesScreenBodyControl;
		} else {
			if(DataScript.scenario.playerHasHighPower){
				Tut3Body.text = DataScript.tutText.BonusesScreenBodyHighPower;
			}
			else{
				Tut3Body.text = DataScript.tutText.BonusesScreenBodyLowPower;
			}
		}


		//populate the power up description panel with all available power ups
		if (DataScript.scenario.pSpeedIncreaseAvailable) {
			addNewDescription("Player Speed Increase","Increases the player's speed");
		}
		if (DataScript.scenario.gSpeedDecreaseAvailable) {
			addNewDescription("Enemy Speed Decrease","Decreases the enemy movement speed");
		}
		if (DataScript.scenario.fRespawnAvailable) {
			addNewDescription("Fruit Respawn Increase","Increases the rate at which fruits appear on the map");
		}
		if (DataScript.scenario.longerPowerModeAvailable) {
			addNewDescription("Longer Super Mode", "Increases the length of time that enemies can be eaten");
		}
		if (DataScript.scenario.powerballRespawnAvailable) {
			addNewDescription("Super Balls Respawn","Super balls will reappear after a short time");
		}
		if (DataScript.scenario.gRespawnAvailable) {
			addNewDescription("Enemy Slower Respawn","Enemies take longer to respawn after being eaten");
		}
		if (DataScript.scenario.gDumbAvailale) {
			addNewDescription("\"Dumb\" Enemies","Enemies chase less effectively");
		}
		if (DataScript.scenario.gFewerAvailable) {
			addNewDescription("Fewer Enemies","Only 3 ghosts chase the player instead of 4");
		}

	}


	void addNewDescription(string name, string desc){

		GameObject newDesc = (GameObject)Instantiate (descriptionPrefab);
		var texts = newDesc.GetComponentsInChildren<Text>();
		foreach(var textbox in texts){
			if(textbox.name == "NameBox"){
				textbox.text = name;
			}
			else if(textbox.name == "DescriptionBox"){
				textbox.text = desc;
			}
		}
		newDesc.transform.SetParent (descriptionPanel, false);

	}
}
