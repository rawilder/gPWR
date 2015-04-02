using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartnerSliderScript : MonoBehaviour {

	Slider s;
	Text message;
	Button continueButton;

	float timeElapsed = 0.0f;
	float timeToFinishedWaiting = 4.0f;
	float timeToConnected = 7.0f;

	// Use this for initialization
	void Start () {
		s = gameObject.GetComponent<Slider> ();
		message = GameObject.Find ("TextMessage").GetComponent<Text> ();
		continueButton = GameObject.Find ("continueButton").GetComponent<Button> ();

		s.value = 0;
		message.text = "Waiting for partner";
		continueButton.interactable = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timeElapsed += Time.deltaTime;
		if (timeElapsed > timeToFinishedWaiting && s.value < 0.3f) {
			s.value += .01f;
			message.text = "Connecting to partner";
		}
		if (timeElapsed < timeToConnected && timeElapsed > timeToFinishedWaiting) {
			s.value += .001f;
		}
		if(timeElapsed > timeToConnected && s.value < 1.0f){
			s.value += .03f;
			message.text = "Connected";
			continueButton.interactable = true;
		}
	}
}
