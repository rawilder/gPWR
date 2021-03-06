﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2Script : MonoBehaviour {

	public Text Tut2Body;
	public Text PointValues;

	// Use this for initialization
	void Start () {
		Tut2Body.text = DataScript.tutText.ObjectiveScreenBody.Replace("[turnTime]",""+DataScript.scenario.turnTime);
		PointValues = GameObject.Find ("PointValues").GetComponent<Text> ();
		PointValues.text = "= " + MazeScript.dotPointValue + "\n\n= " + MazeScript.powerDotPointValue + "\n\n= " + MazeScript.cherryValue + "\n\n= " + MazeScript.ghostValue;
	}

	public void advanceTutorialStage(){
		bool bonusesAvailable = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
		if (!bonusesAvailable) {
			//dont go to the bonus stage
			Application.LoadLevel(5);
		} else {
			Application.LoadLevel(4);
		}
	}
}
