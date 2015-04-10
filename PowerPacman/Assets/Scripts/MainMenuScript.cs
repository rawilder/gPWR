using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine.UI;


[XmlRoot("TutorialText")]
public class TutorialText{
	
	public string WelcomeScreenTitle;
	public string ObjectiveScreenBody;
	public string BonusesScreenTitle;
	public string BonusesScreenBody;
	public string ConnectingScreenTitle;
	public string ConnectingScreenText;
	public string PreallocationScreenControlText;
	public string PreallocationScreenHighPowerText;
	public string PreallocationScreenLowPowerText;
	public string AllocationScreenTitle;
	
	public TutorialText(){
		WelcomeScreenTitle = "Welcome to Two Player Pacman";
		ObjectiveScreenBody = "The objective of this game is to work together with your partner to earn as many points as possible playing Pacman. Each player will have their own maze to navigate. You will take turns playing for a set amount of time. Score counters and the amount of time left on each players turn is displayed above the separate mazes.";
		BonusesScreenTitle = "Bonuses";
		BonusesScreenBody = "Bonuses are special abilities that allow you to earn points quicker...";
		ConnectingScreenTitle = "Connecting to partner";
		ConnectingScreenText = "Please wait while we match you with a partner";
		PreallocationScreenControlText = "This is some placeholder text letting the user know that the game will be beginning soon (no allocation)";
		PreallocationScreenHighPowerText = "This is some placeholder text telling the user that they will be allocating power ups to themselves and the other player";
		PreallocationScreenLowPowerText = "This is some placeholder text telling the user that they will have powerups allocated to them by the other player";
		AllocationScreenTitle = "Bonus allocation";

	}
	
}

public class MainMenuScript : MonoBehaviour {

	List<GameObject> buttons = new List<GameObject>();

	ScenarioList scenList;

	public Transform dropdownPanel;
	public GameObject DropDownPrefab;
	public Text scenarioSelectionText;

	public Text errorMessage;

	public static string tutorialTextFileName = "tutorialText.txt";

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

		//tutorial text
		if(!File.Exists(tutorialTextFileName)){

			//if the file doesnt exist, make a new one with default values
			FileStream s = File.Create (tutorialTextFileName);
			s.Close();
			
			var serializer = new XmlSerializer (typeof(TutorialText));
			var stream = new FileStream (tutorialTextFileName, FileMode.Create);
			serializer.Serialize (stream, DataScript.tutText);
			stream.Close ();
			
		}
		
		try{
			
			var serializer = new XmlSerializer (typeof(TutorialText));
			var stream = new FileStream (tutorialTextFileName, FileMode.Open);
			DataScript.tutText = serializer.Deserialize (stream) as TutorialText;
			stream.Close ();
			
		}
		catch(XmlException){
			
		}

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
