﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine.UI;

public class Scenario{
	//more to come
	public int id;
	public string name;
	public bool control;
	public bool playerHasHighPower;
	public int turnTime; //the time in seconds that each turn lasts
	public int totalTime; //the time in minutes that the two players spend taking turns

	public bool pSpeedIncreaseAvailable;
	public bool gSpeedDecreaseAvailable;
	public bool fRespawnAvailable;
	public bool longerPowerModeAvailable;
	public bool powerballRespawnAvailable;
	public bool gRespawnAvailable;
	public bool gDumbAvailale;
	public bool gFewerAvailable;
	public bool hpStealsTurnsAvailable;//not an allocatable powerup, but it is an ability
	
	public Scenario(){
		name = "";
		id = 0;
		control = false;
		playerHasHighPower = false;
		turnTime = 30;	//default
		totalTime = 10;	//default


		bool pSpeedIncreaseAvailable = false;
		bool gSpeedDecreaseAvailable = false;
		bool fRespawnAvailable = false;
		bool longerPowerModeAvailable = false;
		bool powerballRespawnAvailable = false;
		bool gRespawnAvailable = false;
		bool gDumbAvailale = false;
		bool gFewerAvailable = false;
		bool hpStealsTurnsAvailable = false;

	}

	public Scenario(Scenario s){
		name = s.name;
		id = s.id;
		control = s.control;
		playerHasHighPower = s.playerHasHighPower;
		turnTime = s.turnTime;
		totalTime = s.totalTime;

		pSpeedIncreaseAvailable = s.pSpeedIncreaseAvailable;
		gSpeedDecreaseAvailable = s.gSpeedDecreaseAvailable;
		fRespawnAvailable = s.fRespawnAvailable;
		longerPowerModeAvailable = s.longerPowerModeAvailable;
		powerballRespawnAvailable = s.powerballRespawnAvailable;
		gRespawnAvailable = s.gRespawnAvailable;
		gDumbAvailale = s.gDumbAvailale;
		gFewerAvailable = s.gFewerAvailable;
		hpStealsTurnsAvailable = s.hpStealsTurnsAvailable;
	}
}

[XmlRoot("ScenarioList")]
public class ScenarioList{

	[XmlArray("Scenarios")]
	[XmlArrayItem("Scenario")]
	public List<Scenario> Scenarios = new List<Scenario>();

}

public class AdminDataManager : MonoBehaviour {

	//should probably be in some folder somewhere
	public static string scenarioDataFileName = "scenarioData.txt";

	public Transform dropdownPanel;
	
	public GameObject DropDownPrefab;

	ScenarioList scenList;
	Scenario tempScenario; //the scenario that is currently open for editing, not saved in the list
	InputField nameBox;
	InputField turnTimeBox;
	InputField totalTimeBox;
	Toggle controlToggle;
	Toggle playerAllocatesToggle;

	//power up toggles
	Toggle pSpeed;
	Toggle gSpeed;
	Toggle fRespawn;
	Toggle longerPowerMode;
	Toggle powerballRespawn;
	Toggle gRespawn;
	Toggle gDumb;
	Toggle gFewer;
	Toggle hpStealTurns;

	List<GameObject> buttons = new List<GameObject>();

	void Start () {
	
		scenList = new ScenarioList ();
		tempScenario = new Scenario ();
		
		nameBox = GameObject.Find ("NameInput").GetComponent<InputField> ();
		turnTimeBox = GameObject.Find ("TurnTimeInput").GetComponent<InputField> ();
		totalTimeBox = GameObject.Find ("TotalTimeInput").GetComponent<InputField> ();
		controlToggle = GameObject.Find ("ControlToggle").GetComponent<Toggle> ();
		playerAllocatesToggle = GameObject.Find ("AllocationToggle").GetComponent<Toggle> ();

		pSpeed = GameObject.Find ("PlayerSpeedIncreaseToggle").GetComponent<Toggle> ();
		gSpeed = GameObject.Find ("GhostSpeedDecreaseToggle").GetComponent<Toggle> ();
		fRespawn = GameObject.Find ("FasterFruitToggle").GetComponent<Toggle> ();
		longerPowerMode = GameObject.Find ("LongerPowerModeToggle").GetComponent<Toggle> ();
		powerballRespawn = GameObject.Find ("PowerBallRespawnToggle").GetComponent<Toggle> ();
		gRespawn = GameObject.Find ("GhostsRespawnSlowerToggle").GetComponent<Toggle> ();
		gDumb = GameObject.Find ("DumbGhostsToggle").GetComponent<Toggle> ();
		gFewer = GameObject.Find ("FewerGhostsToggle").GetComponent<Toggle> ();
		hpStealTurns = GameObject.Find ("TurnTakingToggle").GetComponent<Toggle> ();


		//check for scenario data file
		if (!File.Exists (scenarioDataFileName)) {
			FileStream s = File.Create(scenarioDataFileName);
			s.Close();
		}

		try{
			var serializer = new XmlSerializer (typeof(ScenarioList));
			var stream = new FileStream (scenarioDataFileName, FileMode.Open);
			scenList = serializer.Deserialize (stream) as ScenarioList;
			stream.Close ();
		}
		catch(XmlException){

		}

		//populate the drop down menu

		updateDropdown ();

	}

	public void saveChanges(){

		updateTempScenario ();

		if (tempScenario.name == "") {

			//nothing to save
			return;

		}

		bool overwriting = false;

		for(int i = 0; i < scenList.Scenarios.Count; i++){

			if(scenList.Scenarios[i].name == tempScenario.name){
				//maybe have an overwrite notification?
				scenList.Scenarios[i] = new Scenario(tempScenario);
				overwriting = true;
				break;
			}
		}

		if (!overwriting) {
			scenList.Scenarios.Add(new Scenario(tempScenario));
		}

		//write to file
		var serializer = new XmlSerializer (typeof(ScenarioList));
		var stream = new FileStream (scenarioDataFileName, FileMode.Create);
		serializer.Serialize (stream, scenList);
		stream.Close ();

		updateDropdown ();

	}

	public void newScenario(){

		tempScenario = new Scenario ();

		//TODO pick new id
		tempScenario.id = 0;
		tempScenario.name = "New Scenario";

		nameBox.text = tempScenario.name;

	}

	public void updateTempScenario(){

		tempScenario.name = nameBox.text;
		tempScenario.control = controlToggle.isOn;
		tempScenario.playerHasHighPower = playerAllocatesToggle.isOn;

		int turn = 30;
		int total = 10;
		//validate the int boxes
		try{
			turn = Convert.ToInt32(turnTimeBox.text);
		}
		catch(FormatException){
			turnTimeBox.text = "" + tempScenario.turnTime;
		}

		try{
			total = Convert.ToInt32(totalTimeBox.text);
		}
		catch(FormatException){
			totalTimeBox.text = "" + tempScenario.totalTime;
		}

		tempScenario.turnTime = turn;
		tempScenario.totalTime = total;

		tempScenario.pSpeedIncreaseAvailable = pSpeed.isOn;
		tempScenario.gSpeedDecreaseAvailable = gSpeed.isOn;
		tempScenario.fRespawnAvailable = fRespawn.isOn;
		tempScenario.longerPowerModeAvailable = longerPowerMode.isOn;
		tempScenario.powerballRespawnAvailable = powerballRespawn.isOn;
		tempScenario.gRespawnAvailable = gRespawn.isOn;
		tempScenario.gDumbAvailale = gDumb.isOn;
		tempScenario.gFewerAvailable = gFewer.isOn;
		tempScenario.hpStealsTurnsAvailable = hpStealTurns.isOn;

	}

	void updateDropdown(){

		for (int i = 0; i < buttons.Count; i++) {
			buttons[i].SetActive(false);
		}

		buttons.Clear ();

		for (int i = 0; i < scenList.Scenarios.Count; i++) {
			GameObject button = (GameObject) Instantiate(DropDownPrefab);
			button.GetComponentInChildren<Text>().text = scenList.Scenarios[i].name;
			int index = i;
			button.GetComponent<Button>().onClick.AddListener(
				() => {
				//TODO populate all fields
				nameBox.text = scenList.Scenarios[index].name;
				controlToggle.isOn = scenList.Scenarios[index].control;
				playerAllocatesToggle.isOn = scenList.Scenarios[index].playerHasHighPower;
				turnTimeBox.text = "" + scenList.Scenarios[index].turnTime;
				totalTimeBox.text = "" + scenList.Scenarios[index].totalTime;

				pSpeed.isOn = scenList.Scenarios[index].pSpeedIncreaseAvailable;
				gSpeed.isOn = scenList.Scenarios[index].gSpeedDecreaseAvailable;
				fRespawn.isOn = scenList.Scenarios[index].fRespawnAvailable;
				longerPowerMode.isOn = scenList.Scenarios[index].longerPowerModeAvailable;
				powerballRespawn.isOn = scenList.Scenarios[index].powerballRespawnAvailable;
				gRespawn.isOn = scenList.Scenarios[index].gRespawnAvailable;
				gDumb.isOn = scenList.Scenarios[index].gDumbAvailale;
				gFewer.isOn = scenList.Scenarios[index].gFewerAvailable;
				hpStealTurns.isOn = scenList.Scenarios[index].hpStealsTurnsAvailable;

				tempScenario = new Scenario(scenList.Scenarios[index]);
			}
			);
			button.transform.SetParent(dropdownPanel,false);
			buttons.Add(button);
		}

	}

	public void deleteCurrentScenario(){

		for(int i = 0; i < scenList.Scenarios.Count; i++){

			if(scenList.Scenarios[i].name == tempScenario.name){
				scenList.Scenarios.Remove(scenList.Scenarios[i]);
				break;
			}
		}

		updateDropdown ();
	}
}
