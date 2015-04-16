using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;

public class ScoreWeightScreenScript : MonoBehaviour {

	Slider weightSlider;
	Text partnerWeightText, playerWeightText;
	Text titleText,topMessageText, bottomMessageText;
	Button continueButton;

	float delay = 4.0f;


	// Use this for initialization
	void Start () {
	
		weightSlider = GameObject.Find ("WeightSlider").GetComponent<Slider> ();
		partnerWeightText = GameObject.Find ("ParterWeight").GetComponent<Text> ();
		playerWeightText = GameObject.Find ("PlayerWeight").GetComponent<Text> ();
		titleText = GameObject.Find ("Title").GetComponent<Text> ();
		topMessageText = GameObject.Find ("MessageText").GetComponent<Text> ();
		bottomMessageText = GameObject.Find ("BottomMessageText").GetComponent<Text> ();
		continueButton = GameObject.Find ("ContinueButton").GetComponent<Button> ();

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
			bottomMessageText.text = DataScript.tutText.ScoreWeightScreenBottomMessageControl;
			topMessageText.text = DataScript.tutText.ScoreWeightScreenTopMessageControl;
		} else {
			if(DataScript.scenario.playerHasHighPower){
				bottomMessageText.text = DataScript.tutText.ScoreWeightScreenBottomMessageHighPower;
				topMessageText.text = DataScript.tutText.ScoreWeightScreenTopMessageHighPower;
			}
			else{
				bottomMessageText.text = DataScript.tutText.ScoreWeightScreenBottomMessageLowPower;
				topMessageText.text = DataScript.tutText.ScoreWeightScreenTopMessageLowPower;
				buttonSetEnabled(continueButton,false);
			}
		}

		weightSliderUpdate ();

	}

	void FixedUpdate(){
		if (delay > 0) {
			delay -= Time.deltaTime;
		} else {
			if (!DataScript.scenario.playerHasHighPower && DataScript.scenario.ScoreWeightAvailable && !DataScript.scenario.control) {
				weightSlider.value = DataScript.alloc.scoreWeight;
				bottomMessageText.text = DataScript.tutText.ScoreWeightScreenSelectionCompleteMessage;
			}
			buttonSetEnabled(continueButton, true);
		}
	}

	public void weightSliderUpdate(){

		//make sure the value is always rounded to the nearest .05 (5%)
		float value = weightSlider.value;
		float newVal = (float) Math.Round (value * 20) / 20;
		partnerWeightText.text = "" + ((1-newVal)*100) + "%";
		playerWeightText.text = "" + (newVal*100) + "%";
		weightSlider.value = newVal;

		if (DataScript.scenario.playerHasHighPower) {
			DataScript.alloc.scoreWeight = newVal;
		}
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
}
