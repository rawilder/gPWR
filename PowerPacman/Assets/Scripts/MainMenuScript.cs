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
	public string BonusesScreenBodyHighPower;
	public string BonusesScreenBodyLowPower;
	public string BonusesScreenBodyControl;
	public string ConnectingScreenTitle;
	public string ConnectingScreenText;
	public string PreallocationScreenControlNoBonusNoWeightText;
	public string PreallocationScreenControlNoBonusWeightText;
	public string PreallocationScreenControlBonusNoWeightText;
	public string PreallocationScreenControlBonusAndWeightText;
	public string PreallocationScreenHighPowerTextBonusesAndWeight;
	public string PreallocationScreenHighPowerTextBonusesNoWeight;
	public string PreallocationScreenHighPowerTextWeightNoBonuses;
	public string PreallocationScreenLowPowerTextBonusesAndWeight;
	public string PreallocationScreenLowPowerTextBonusesNoWeight;
	public string PreallocationScreenLowPowerTextWeightNoBonuses;
	public string AllocationScreenTitle;
	public string AllocationScreenTopMessageHighPower;
	public string AllocationScreenTopMessageLowPower;
	public string AllocationScreenTopMessageControl;
	public string AllocationScreenErrorMessage;
	public string AllocationScreenLowPowerPostAllocationMessage;
	public string AllocationReviewBodyHighPower;
	public string AllocationReviewBodyLowPower;
	public string AllocationReviewBodyControl;
	public string AllocationReviewBottomMessage;
	public string ScoreWeightScreenTitle;
	public string ScoreWeightScreenTopMessageHighPower;
	public string ScoreWeightScreenTopMessageLowPower;
	public string ScoreWeightScreenTopMessageControl;
	public string ScoreWeightScreenBottomMessageHighPower;
	public string ScoreWeightScreenBottomMessageLowPower;
	public string ScoreWeightScreenBottomMessageControl;
	public string ScoreWeightScreenSelectionCompleteMessage;
	public string PregameScreenTitle;
	public string PregamePlayerGoesFirstText;
	public string PregamePlayerStealsTurnsText;
	public string PregamePlayerStealsTurnsLowPowerText;
	public string PregameContinueText;
	public string GameTakeTurnMessageHighPower;
	public string GameTakeTurnMessageLowPower;
	public string GameTakeTurnYesMessageHighPower;
	public string GameTakeTurnYesMessageLowPower;
	public string GameEndMessage;
	public string GameTimeRemainingText;
	
	public TutorialText(){
		WelcomeScreenTitle = "Welcome to Two Player Pacman";
		ObjectiveScreenBody = "In a few moments, you will be connected to another player in a different location. The objective of this game is to work together with your partner to earn as many points as possible playing Pacman, where the final score is an average of your separate scores. Each player will have their own maze to navigate. You are going to be taking turns playing for a set amount of time. Points are earned by eating dots, cherries, and blue enemies. If you are touched by an enemy, you will return to where you stared and continue your turn";
		BonusesScreenTitle = "Bonuses";
		BonusesScreenBodyLowPower = "Bonuses are special abilities that allow you to earn points more quickly. The names of the different bonuses and their effects are detailed to the right. In a few moments, your partner is going choose how to divide the bonuses between the two players.";
		BonusesScreenBodyHighPower = "Bonuses are special abilities that allow you to earn points more quickly. The names of the different bonuses and their effects are detailed to the right. In a few moments you will be given the ability to divide the bonuses between yourself and your partner.";
		BonusesScreenBodyControl = "Bonuses are special abilities that all you to earn points more quickly. The names of the different bonuses and their effects are detailed to the right. Both you and your partner will have the same bonuses.";
		ConnectingScreenTitle = "Connecting to partner";
		ConnectingScreenText = "Please wait while we match you with a partner";
		PreallocationScreenControlNoBonusNoWeightText = "We have connected you with your partner, please press the continue button to begin the game";
		PreallocationScreenControlNoBonusWeightText = "We have connected you with your partner, please press the continue button to continue to the score weight screen";
		PreallocationScreenControlBonusNoWeightText = "We have connected you with your partner, please press the continue button to continue to the bonus screen";
		PreallocationScreenControlBonusAndWeightText = "We have connected you with your partner, please press the continue button to continue to the bonus screen";
		PreallocationScreenHighPowerTextBonusesAndWeight = "We have connected you with your partner, and the game has determined that you will be the one who assigns bonuses and score weights. Please press the continue button";
		PreallocationScreenHighPowerTextBonusesNoWeight = "We have connected you with your partner, and the game has determined that you will be the one who assigns bonuses. Please press the continue button";
		PreallocationScreenHighPowerTextWeightNoBonuses = "We have connected you with your partner, and the game has determined that you will be the one who assigns the score weights. Please press the continue button";
		PreallocationScreenLowPowerTextBonusesAndWeight = "We have connected you with your partner, and the game has determined that your partner will be the one who assigns bonuses and the score weights. Please press the continue button";
		PreallocationScreenLowPowerTextBonusesNoWeight = "We have connected you with your partner, and the game has determined that your partner will be the one who assigns the bonuses. Please press the continue button";
		PreallocationScreenLowPowerTextWeightNoBonuses = "We have connected you with your partner, and the game has determined that your partner will be the one who assigns the score weights. Please press the continue button";
		AllocationScreenTitle = "Bonus allocation";
		AllocationScreenTopMessageHighPower = "Please use the sliders to allocate a bonus to yourself or your partner";
		AllocationScreenTopMessageLowPower = "Please wait while your partner allocates the bonuses";
		AllocationScreenTopMessageControl = "You and your partner will have the same bonuses. Please click the continue button";
		AllocationScreenErrorMessage = "Please assign all bonuses before continuing";
		AllocationScreenLowPowerPostAllocationMessage = "Your partner has finished assigning all bonuses, please review them and then press the continue button";
		AllocationReviewBodyHighPower = "The bonuses you have chosen for yourself and your partner are listed below. Please press the continue button.";
		AllocationReviewBodyLowPower = "The bonuses your partner has chosen for you are listed below. Please review them and then press the continue";
		AllocationReviewBodyControl = "The bonuses that you are your partner were assigned are listed below. Please review them and then press the continue button";
		AllocationReviewBottomMessage = "";
		ScoreWeightScreenTopMessageHighPower = "The final score of the game is calculated as a weighted average of your and your parter's scores. The higher your percent, the more your score contributes.";
		ScoreWeightScreenTopMessageLowPower = "The final score of the game is calculated as a weighted average of your and your parter's scores. The higher your percent, the more your score contributes.";
		ScoreWeightScreenTopMessageControl = "The final score of the game is calculated as a weighted average of your and your parter's scores. The higher your percent, the more your score contributes.";
		ScoreWeightScreenBottomMessageHighPower = "Please use the slider to select score weights for you and your partner";
		ScoreWeightScreenBottomMessageLowPower = "Please wait while your partner selects score weights for themself and you.";
		ScoreWeightScreenBottomMessageControl = "The score weights that were determined for you and your partner are displayed on the slider below. Please read them and then press the continue button";
		ScoreWeightScreenSelectionCompleteMessage = "Your partner has finished determining the score weights, please press the continue button";
		PregameScreenTitle = "Pregame";
		PregamePlayerGoesFirstText = "The program has decided that you will have the first turn of the game.";
		PregamePlayerStealsTurnsText = "The game has determined that between turns, you will have the ability to skip your partner's turn and immediately play again. Your partner does not have this ability. You will be limited to [turnSteals] turn skips";
		PregamePlayerStealsTurnsLowPowerText = "The game has determined that between turns, your partner will have the ability to skip your turn and immediately play again. You will not have this ability. Your partner will be limited to [turnSteals] turn skips";
		PregameContinueText = "Please press the continue button to begin the game";
		GameTakeTurnMessageHighPower = "Press 'F' to take another turn";
		GameTakeTurnMessageLowPower = "Your partner is deciding whether to take another turn";
		GameTakeTurnYesMessageHighPower = "You have chosen to take another turn";
		GameTakeTurnYesMessageLowPower = "Your partner has chosen to take another turn";
		GameEndMessage = "The game has ended, please wait";
		GameTimeRemainingText = "Time remaining in the game";
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
			Debug.Log("Exception in deserializing tutText file");
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
