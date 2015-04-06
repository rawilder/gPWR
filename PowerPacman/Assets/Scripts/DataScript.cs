using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

//This script stores data about the player and the choices made, and also has functions for exporting that data.
public class DataScript : MonoBehaviour {

	static string fileName = "data.txt";

	public static int playerId;
	public static string computerId;
	public static int playerScore;
	public static int aiScore;

	Text aiScoreText;
	Text playerScoreText;
	Text totalScoreText;

	public static void exportData(){

		if (File.Exists (fileName)) {
			//do something, maybe clear the whole thing
		} else {
			StreamWriter f = File.CreateText (fileName);
			f.WriteLine("Data for user " + playerId);
			f.Close();
		}

	}

	void Start(){

		aiScoreText = GameObject.Find ("aiScoreText").GetComponent<Text>();
		playerScoreText = GameObject.Find ("playerScoreText").GetComponent<Text>();
		totalScoreText = GameObject.Find ("FinalScoreText").GetComponent<Text>();

		aiScoreText.text = "" + aiScore;
		playerScoreText.text = "" + playerScore;
		totalScoreText.text = "" + (aiScore + playerScore);


	}

}
