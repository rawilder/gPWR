using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;

public class ScoreWeightScreenScript : MonoBehaviour {

	Slider weightSlider;
	Text partnerWeightText, playerWeightText;
	Text titleText,topMessageText, bottomMessageText;


	// Use this for initialization
	void Start () {
	
		weightSlider = GameObject.Find ("WeightSlider").GetComponent<Slider> ();
		partnerWeightText = GameObject.Find ("ParterWeight").GetComponent<Text> ();
		playerWeightText = GameObject.Find ("PlayerWeight").GetComponent<Text> ();
		titleText = GameObject.Find ("Title").GetComponent<Text> ();
		topMessageText = GameObject.Find ("MessageText").GetComponent<Text> ();
		bottomMessageText = GameObject.Find ("BottomMessageText").GetComponent<Text> ();

		topMessageText.text = "";

		weightSliderUpdate ();

		if (!DataScript.scenario.playerHasHighPower || DataScript.scenario.ScoreWeightPredetermined || DataScript.scenario.control) {
			weightSlider.GetComponentInChildren<Slider> ().interactable = false;
			Graphic[] g2 = weightSlider.GetComponentInChildren<Slider> ().GetComponentsInChildren<Graphic> ();
			for (int i = 0; i < g2.Length; i++) {
				g2 [i].CrossFadeAlpha (.2f, .2f, true);
			}
		}

		if (DataScript.scenario.ScoreWeightPredetermined || DataScript.scenario.control) {
			DataScript.alloc.scoreWeight = DataScript.scenario.AiAllocateWeight;
			weightSlider.value = DataScript.alloc.scoreWeight;
			//TODO editable text
			bottomMessageText.text = "The program has determined that the score weights will be as follows:";
		} else {
			if(DataScript.scenario.playerHasHighPower){
				bottomMessageText.text = "Please select score weights for yourself and your partner";
			}
			else{
				bottomMessageText.text = "Please wait while your partner selects the score weight";
			}
		}

		weightSliderUpdate ();

	}

	void FixedUpate(){
		if (!DataScript.scenario.playerHasHighPower && !DataScript.scenario.ScoreWeightPredetermined) {
			Thread.Sleep(4000);
			weightSlider.value = DataScript.alloc.scoreWeight;
			topMessageText.text = "Your partner has finished choosing the weights, please continue";
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
}
