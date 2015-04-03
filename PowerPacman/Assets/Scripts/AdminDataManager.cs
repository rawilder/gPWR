using UnityEngine;
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

	public Scenario(){
		name = "";
		id = 0;
		control = false;
		playerHasHighPower = false;
		turnTime = 30;	//default
		totalTime = 10;	//default
	}

	public Scenario(Scenario s){
		name = s.name;
		id = s.id;
		control = s.control;
		playerHasHighPower = s.playerHasHighPower;
		turnTime = s.turnTime;
		totalTime = s.totalTime;
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
	string scenarioDataFileName = "scenarioData.txt";

	public Transform dropdownPanel;
	
	public GameObject DropDownPrefab;

	ScenarioList scenList;
	Scenario tempScenario; //the scenario that is currently open for editing, not saved in the list
	InputField nameBox;
	InputField turnTimeBox;
	InputField totalTimeBox;
	Toggle controlToggle;
	Toggle playerAllocatesToggle;
	
	void Start () {
	
		scenList = new ScenarioList ();
		tempScenario = new Scenario ();
		
		nameBox = GameObject.Find ("NameInput").GetComponent<InputField> ();
		turnTimeBox = GameObject.Find ("TurnTimeInput").GetComponent<InputField> ();
		totalTimeBox = GameObject.Find ("TotalTimeInput").GetComponent<InputField> ();
		controlToggle = GameObject.Find ("ControlToggle").GetComponent<Toggle> ();
		playerAllocatesToggle = GameObject.Find ("AllocationToggle").GetComponent<Toggle> ();

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
					tempScenario = new Scenario(scenList.Scenarios[index]);
					Debug.Log ("Loading scenario");
				}
			);
			button.transform.SetParent(dropdownPanel,false);
		}

	}

	public void saveChanges(){

		updateTempScenario ();

		Debug.Log (tempScenario.name);

		if (tempScenario.name == "") {

			//nothing to save
			return;

		}

		bool overwriting = false;

		for(int i = 0; i < scenList.Scenarios.Count; i++){

			if(scenList.Scenarios[i].name == tempScenario.name){
				//maybe have an overwrite notification?
				//Debug.Log("overwiting " + scenList.Scenarios[i].name + " with " + tempScenario.name);
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

	}
}
