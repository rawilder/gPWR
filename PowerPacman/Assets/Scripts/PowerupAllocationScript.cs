using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using System;

public class PowerupAllocationScript : MonoBehaviour {

	Text messageText;
	public Text Title;
	public Transform sliderPanel;
	public GameObject allocationSliderPrefab;
	Slider weightSlider;
	Text partnerWeightText;
	Text playerWeightText;

	public int iterationCount = 0;

	Button continueButton;

	float countdownRemaining = 4.0f;

	IList<GameObject> sliders = new List<GameObject>();

	void Start () {
	
		weightSlider = GameObject.Find ("WeightSlider").GetComponent<Slider> ();
		partnerWeightText = GameObject.Find ("ParterWeight").GetComponent<Text> ();
		playerWeightText = GameObject.Find ("PlayerWeight").GetComponent<Text> ();
		continueButton = GameObject.Find ("Button").GetComponent<Button> ();
		Title.text = DataScript.tutText.AllocationScreenTitle;

		messageText = GameObject.Find ("TempText").GetComponent<Text> ();

		if (!DataScript.scenario.playerHasHighPower) {
			buttonSetEnabled(continueButton,false);
		}

		//create the sliders
		createSliders ();
		weightSliderUpdate ();

		if (!DataScript.scenario.playerHasHighPower){
			foreach (var slider in sliders) {
				slider.GetComponentInChildren<Slider>().interactable = false;
				Graphic[] g = slider.GetComponentInChildren<Slider>().GetComponentsInChildren<Graphic>();
				for(int i = 0; i < g.Length; i++){
					g[i].CrossFadeAlpha(.2f,.2f,true);
				}
			}

			weightSlider.GetComponentInChildren<Slider>().interactable = false;
			Graphic[] g2 = weightSlider.GetComponentInChildren<Slider>().GetComponentsInChildren<Graphic>();
			for(int i = 0; i < g2.Length; i++){
				g2[i].CrossFadeAlpha(.2f,.2f,true);
			}
			
		}


		if (!DataScript.scenario.playerHasHighPower) {
			if (!DataScript.scenario.AiAllocationIsRandom) {

				//apply the power ups
				if (DataScript.scenario.pSpeedIncreaseAvailable) {
					Debug.Log ("Player speed: " + DataScript.scenario.AiAllocatePlayerSpeed);
					if (DataScript.scenario.AiAllocatePlayerSpeed == 0) {
						DataScript.alloc.PlayerSpeed = 0;
					} else {
						DataScript.alloc.PlayerSpeed = 1;
					}
				}

				if (DataScript.scenario.gSpeedDecreaseAvailable) {
					if (DataScript.scenario.AiAllocateGhostSpeed == 0) {
						DataScript.alloc.GhostSpeed = 0;
					} else {
						DataScript.alloc.GhostSpeed = 1;
					}
				}

				if (DataScript.scenario.fRespawnAvailable) {
					if (DataScript.scenario.AiAllocateFruitRespawn == 0) {
						DataScript.alloc.FruitRespawn = 0;
					} else {
						DataScript.alloc.FruitRespawn = 1;
					}
				}

				if (DataScript.scenario.longerPowerModeAvailable) {
					if (DataScript.scenario.AiAllocateLongerPowerMode == 0) {
						DataScript.alloc.LongerPowerMode = 0;
					} else {
						DataScript.alloc.LongerPowerMode = 1;
					}
				}

				if (DataScript.scenario.powerballRespawnAvailable) {
					if (DataScript.scenario.AiAllocatePowerBallRespawn == 0) {
						DataScript.alloc.PowerBallRespawn = 0;
					} else {
						DataScript.alloc.PowerBallRespawn = 1;
					}
				}

				if (DataScript.scenario.gRespawnAvailable) {
					if (DataScript.scenario.AiAllocateGhostRespawn == 0) {
						DataScript.alloc.GhostRespawn = 0;
					} else {
						DataScript.alloc.GhostRespawn = 1;
					}
				}

				if (DataScript.scenario.gDumbAvailale) {
					if (DataScript.scenario.AiAllocateDumbGhosts == 0) {
						DataScript.alloc.DumbGhosts = 0;
					} else {
						DataScript.alloc.DumbGhosts = 1;
					}
				}

				if (DataScript.scenario.gFewerAvailable) {
					if (DataScript.scenario.AiAllocateFewerGhosts == 0) {
						DataScript.alloc.FewerGhosts = 0;
					} else {
						DataScript.alloc.FewerGhosts = 1;
					}
				}
			} else {
				//random allocation
				//TODO

				if(DataScript.scenario.pSpeedIncreaseAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.PlayerSpeed = 1;
					}
					else{
						DataScript.alloc.PlayerSpeed = 0;
					}
				}

				if(DataScript.scenario.gSpeedDecreaseAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.GhostSpeed = 1;
					}
					else{
						DataScript.alloc.GhostSpeed = 0;
					}
				}

				if(DataScript.scenario.fRespawnAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.FruitRespawn = 1;
					}
					else{
						DataScript.alloc.FruitRespawn = 0;
					}
				}

				if(DataScript.scenario.longerPowerModeAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.LongerPowerMode = 1;
					}
					else{
						DataScript.alloc.LongerPowerMode = 0;
					}
				}

				if(DataScript.scenario.powerballRespawnAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.PowerBallRespawn = 1;
					}
					else{
						DataScript.alloc.PowerBallRespawn = 0;
					}
				}

				if(DataScript.scenario.gRespawnAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.GhostRespawn = 1;
					}
					else{
						DataScript.alloc.GhostRespawn = 0;
					}
				}

				if(DataScript.scenario.gDumbAvailale){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.DumbGhosts = 1;
					}
					else{
						DataScript.alloc.DumbGhosts = 0;
					}
				}

				if(DataScript.scenario.gFewerAvailable){
					if(UnityEngine.Random.Range(0.0f, 1.0f) > .5f){
						DataScript.alloc.FewerGhosts = 1;
					}
					else{
						DataScript.alloc.FewerGhosts = 0;
					}
				}

				//score weight
				float w = UnityEngine.Random.Range(.25f, .75f);
				DataScript.alloc.scoreWeight = (float) Math.Round (w * 20) / 20;
				Debug.Log(w);

			}
		} else {



		}

	}

	void FixedUpdate(){
		if (countdownRemaining > 0) {
			countdownRemaining -= Time.deltaTime;
		}
		else{
			if (!DataScript.scenario.playerHasHighPower) {
				runAiAllocation();
			}
			countdownRemaining = UnityEngine.Random.Range(0.5f,1.5f);
		}
	}

	void createSliders(){

		if (DataScript.scenario.pSpeedIncreaseAvailable) {
			DataScript.alloc.PlayerSpeed = 1;
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
			DataScript.alloc.GhostSpeed = 1;
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
			DataScript.alloc.FruitRespawn = 1;
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
			DataScript.alloc.LongerPowerMode = 1;
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
			DataScript.alloc.PowerBallRespawn = 1;
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
			DataScript.alloc.GhostRespawn = 1;
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
			DataScript.alloc.DumbGhosts = 1;
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
			DataScript.alloc.FewerGhosts = 1;
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

	void runAiAllocation(){

		//not flexible code... this is a really dumb way to do this

		switch(iterationCount){
		case 0:
				if (DataScript.alloc.PlayerSpeed != -1) {
					foreach(var slider in sliders){
						if(slider.GetComponentInChildren<Text>().text == "Player Speed Increase"){
							slider.GetComponentInChildren<Slider>().value = DataScript.alloc.PlayerSpeed;
							break;
						}
					}
					//Thread.Sleep ((int)(1000 * UnityEngine.Random.Range (2.0f, 3.0f)));
				}
				break;
		case 1:
			if (DataScript.alloc.GhostSpeed != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "Enemy Speed Decrease"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.GhostSpeed;
						break;
					}
				}
				//Thread.Sleep((int) (1000 * UnityEngine.Random.Range(2.0f,3.0f)));
			}
			break;
		case 2:
			if (DataScript.alloc.FruitRespawn != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "Fruit Respawn Increase"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.FruitRespawn;
						break;
					}
				}
			}
			break;
		case 3:
			if (DataScript.alloc.LongerPowerMode != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "Longer Power Mode"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.LongerPowerMode;
						break;
					}
				}
			}
			break;
		case 4:
			if (DataScript.alloc.PowerBallRespawn != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "Power Balls Respawn"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.PowerBallRespawn;
						break;
					}
				}
			}
			break;
		case 5:
			if (DataScript.alloc.GhostRespawn != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "Enemy Slower Respawn"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.GhostRespawn;
						break;
					}
				}
			}
			break;
		case 6:
			if (DataScript.alloc.DumbGhosts != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "\"Dumb\" Enemies"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.DumbGhosts;
						break;
					}
				}
			}
			break;
		case 7:
			if (DataScript.alloc.FewerGhosts != -1) {
				foreach(var slider in sliders){
					if(slider.GetComponentInChildren<Text>().text == "Fewer Enemies"){
						slider.GetComponentInChildren<Slider>().value = DataScript.alloc.FewerGhosts;
						break;
					}
				}
			}
			break;
		case 8:
			//weight slider
			weightSlider.value = DataScript.alloc.scoreWeight;
			break;
		case 9:
			buttonSetEnabled(continueButton,true);
			break;
		}
		iterationCount++;
	}

	void buttonSetEnabled(Button b, bool enable){
		if (enable) {
			Graphic[] g = b.GetComponentsInChildren<Graphic> ();
			for (int i = 0; i < g.Length; i++) {
				g [i].CrossFadeAlpha (1.0f, .5f, true);
			}
			b.enabled = true;
		} else {
			Graphic[] g = b.GetComponentsInChildren<Graphic> ();
			for (int i = 0; i < g.Length; i++) {
				g [i].CrossFadeAlpha (.2f, .2f, true);
			}
			b.enabled = false;
		}
	}

	public void weightSliderUpdate(){

		//make sure the value is always rounded to the nearest .05 (5%)
		float value = weightSlider.value;
		float newVal = (float) Math.Round (value * 20) / 20;
		partnerWeightText.text = "" + ((1-newVal)*100) + "%";
		playerWeightText.text = "" + (newVal*100) + "%";
		weightSlider.value = newVal;

		DataScript.alloc.scoreWeight = newVal;

	}

	public void continueButtonPressed(){

		Application.LoadLevel (8);

	}
}
