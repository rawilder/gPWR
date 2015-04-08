using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

	List<GameObject> buttons = new List<GameObject>();

	ScenarioList scenList;

	public Transform dropdownPanel;
	public GameObject DropDownPrefab;
	public Text scenarioSelectionText;

	public Text errorMessage;

	void Start(){

		errorMessage = GameObject.Find ("ErrorText").GetComponent<Text> ();

		scenList = new ScenarioList ();

		//check for scenario data file
		if (!File.Exists (AdminDataManager.scenarioDataFileName)) {
			FileStream s = File.Create(AdminDataManager.scenarioDataFileName);
			s.Close();
		}
		
		try{
			var serializer = new XmlSerializer (typeof(ScenarioList));
			var stream = new FileStream (AdminDataManager.scenarioDataFileName, FileMode.Open);
			scenList = serializer.Deserialize (stream) as ScenarioList;
			stream.Close ();
		}
		catch(XmlException){
			
		}

		updateDropdown ();

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

				scenarioSelectionText.text = scenList.Scenarios[index].name;
				DataScript.scenario = new Scenario(scenList.Scenarios[index]);

				}
			);
			button.transform.SetParent(dropdownPanel,false);
			buttons.Add(button);
		}

	}

	public void checkIdInputAndLoad(){
		
		GameObject field = GameObject.FindGameObjectWithTag ("subjectIdInput");
		InputField computerId = GameObject.Find ("ComputerIdField").GetComponent<InputField> ();
		
		InputField input = field.GetComponent<InputField> ();
		
		string subjectId = input.text;
		string compIdStr = computerId.text;

		if (DataScript.scenario == null) {
			// need to select a scenario
			errorMessage.text = "Please select a scenario";
			return;
		}

		if (subjectId == "") {
			errorMessage.text = "Error: No subject ID provided";
			return;
		}

		if (compIdStr == "") {
			errorMessage.text = "Error No computer ID provided";
			return;
		}
		
		if (subjectId != "" && compIdStr != "") {
			int val = 0;
			bool intParseSuccess = int.TryParse(subjectId, out val);
			if(intParseSuccess){
				//might not even have to do this check
				DataScript.playerId = val;
				DataScript.computerId = compIdStr;
				Application.LoadLevel(1);
			}
			else{
				//not an int
			}
		} else {
			//display some error
		}
	}

}
