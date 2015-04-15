using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2Script : MonoBehaviour {

	public Text Tut2Body;

	// Use this for initialization
	void Start () {
		Tut2Body.text = DataScript.tutText.ObjectiveScreenBody;
	}

	public void advanceTutorialStage(){
		if (DataScript.scenario.control) {
			//dont go to the bonus stage
			Application.LoadLevel(5);
		} else {
			Application.LoadLevel(4);
		}
	}
}
