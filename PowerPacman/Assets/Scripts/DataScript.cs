using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;

//This script stores data about the player and the choices made, and also has functions for exporting that data.

public class Allocation{

	//0 = ai, 1 = player, -1 = not available

	public int PlayerSpeed;
	public int GhostSpeed;
	public int FruitRespawn;
	public int LongerPowerMode;
	public int PowerBallRespawn;
	public int GhostRespawn;
	public int DumbGhosts;
	public int FewerGhosts;
	public float scoreWeight;

	public Allocation(){
		PlayerSpeed = -1;
		GhostSpeed = -1;
		FruitRespawn = -1;
		LongerPowerMode = -1;
		PowerBallRespawn = -1;
		GhostRespawn = -1;
		DumbGhosts = -1;
		FewerGhosts = -1;
		scoreWeight = 0.5f;
	}

	public Allocation(Allocation a){
		PlayerSpeed = a.PlayerSpeed;
		GhostSpeed = a.GhostSpeed;
		FruitRespawn = a.FruitRespawn;
		LongerPowerMode = a.LongerPowerMode;
		PowerBallRespawn = a.PowerBallRespawn;
		GhostRespawn = a.GhostRespawn;
		DumbGhosts = a.DumbGhosts;
		FewerGhosts = a.FewerGhosts;
		scoreWeight = a.scoreWeight;
	}

}

public class DataScript : MonoBehaviour {

	static string fileName = "data.csv";

	public static int playerId;
	public static string computerId;
	public static int totalScore;
	
	public static int playerScore;
	public static int playerGhostsEaten = 0;
	public static int playerDotsEaten = 0;
	public static int playerTimesClearedMaze = 0;
	public static int playerTimesEaten = 0;
	public static int playerCherriesEaten = 0;
	public static int playerPowerDotsEaten = 0;

	public static int aiScore;
	public static int aiGhostsEaten = 0;
	public static int aiDotsEaten = 0;
	public static int aiTimesClearedMaze = 0;
	public static int aiTimesEaten = 0;
	public static int aiCherriesEaten = 0;
	public static int aiPowerDotsEaten = 0;

	public static Scenario scenario;

	public static Allocation alloc = new Allocation();
	public static TutorialText tutText = new TutorialText ();


	Text aiScoreText;
	Text playerScoreText;
	Text totalScoreText;

	public static void exportData(){

		//totalScore = playerScore + aiScore;
		totalScore = (int) (((1-DataScript.alloc.scoreWeight) * aiScore) + (DataScript.alloc.scoreWeight * playerScore));

		if (!File.Exists (fileName)) {
			StreamWriter f = File.CreateText (fileName);
			f.WriteLine("Date,Time,PlayerId,ComputerId,PlayerScore,aiScore,totalScore,playerGhostsEaten,aiGhostsEaten,playerDotsEaten,aiDotsEaten,playerTimesClearedMaze,aiTimesClearedMaze,playerTimesEaten,aiTimesEaten,playerCherriesEaten,aiCherriesEaten,playerPowerDotsEaten,aiPowerDotsEaten");
			f.Close();
		}

		using (StreamWriter sw = File.AppendText(fileName)) {

			sw.WriteLine(""+DateTime.Now.ToString("M/d/yyyy") +"," + DateTime.Now.ToString ("HH:mm:ss tt") + "," + playerId + "," + computerId + "," + playerScore + "," + aiScore + "," + totalScore + "," + playerGhostsEaten + "," + aiGhostsEaten + "," + playerDotsEaten + "," + aiDotsEaten + "," + playerTimesClearedMaze + "," + aiTimesClearedMaze + "," + playerTimesEaten + "," + aiTimesEaten + "," + playerCherriesEaten + "," + aiCherriesEaten + "," + playerPowerDotsEaten + "," + aiPowerDotsEaten);

		}


	}

	void Start(){

		playerScoreText = GameObject.Find ("playerScoreText").GetComponent<Text>();
		totalScoreText = GameObject.Find ("FinalScoreText").GetComponent<Text>();
		aiScoreText = GameObject.Find ("aiScoreText").GetComponent<Text>();

		aiScoreText.text = "" + aiScore;
		playerScoreText.text = "" + playerScore;
		totalScoreText.text = "" + (1-DataScript.alloc.scoreWeight) + " * " + aiScore + " + " + (DataScript.alloc.scoreWeight) + " * " + playerScore + " = " +   totalScore;

		exportData ();
	}

}
