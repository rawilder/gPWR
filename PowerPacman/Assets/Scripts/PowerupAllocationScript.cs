using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PowerupAllocationScript : MonoBehaviour {

	Text messageText;
	public Text Title;
	public Transform sliderPanel;
	public GameObject allocationSliderPrefab;

	IList<GameObject> sliders = new List<GameObject>();

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


		//create the sliders
		createSliders ();



		if (!DataScript.scenario.playerHasHighPower){
			foreach (var slider in sliders) {
				slider.GetComponentInChildren<Slider>().enabled = false;
				Graphic[] g = slider.GetComponentInChildren<Slider>().GetComponentsInChildren<Graphic>();
				for(int i = 0; i < g.Length; i++){
					g[i].CrossFadeAlpha(.2f,.2f,true);
				}
			}
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

	void createSliders(){

		if (DataScript.scenario.pSpeedIncreaseAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Player Speed Increase";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.PlayerSpeed = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}
		
		if (DataScript.scenario.gSpeedDecreaseAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Enemy Speed Decrease";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.GhostSpeed = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}
		
		if (DataScript.scenario.fRespawnAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Fruit Respawn Increase";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.FruitRespawn = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}

		if (DataScript.scenario.longerPowerModeAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Longer Power Mode";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.LongerPowerMode = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}

		if (DataScript.scenario.longerPowerModeAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Longer Power Mode";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.LongerPowerMode = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}
		if (DataScript.scenario.powerballRespawnAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Power Balls Respawn";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.PowerBallRespawn = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}

		if (DataScript.scenario.gRespawnAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Enemy Slower Respawn";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.GhostRespawn = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}

		if (DataScript.scenario.gDumbAvailale) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "\"Dumb\" Enemies";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.DumbGhosts = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}

		if (DataScript.scenario.gFewerAvailable) {
			GameObject newSlider = (GameObject)Instantiate (allocationSliderPrefab);
			newSlider.GetComponentInChildren<Text> ().text = "Fewer Enemies";
			newSlider.GetComponentInChildren<Slider> ().onValueChanged.AddListener ((float value) => {
				
				if (DataScript.scenario.playerHasHighPower) {
					int v = updateSliderValue (newSlider.GetComponentInChildren<Slider> ());
					DataScript.alloc.FewerGhosts = v;
				}
			}
			);
			newSlider.transform.SetParent (sliderPanel, false);
			sliders.Add (newSlider);
		}


	}

	int updateSliderValue(Slider s){
		if(s.value <= .5){
			s.value = 0.0f;
			return 0;
		}
		else{
			s.value = 1.0f;
			return 1;
		}
	}
}
