using UnityEngine;
using System.Collections;
using System.IO;

//This script stores data about the player and the choices made, and also has functions for exporting that data.
public class DataScript : MonoBehaviour {

	static string fileName = "data.txt";

	public static int playerId;

	public static void exportData(){

		if (File.Exists (fileName)) {
			//do something, maybe clear the whole thing
		} else {
			StreamWriter f = File.CreateText (fileName);
			f.WriteLine("Data for user " + playerId);
			f.Close();
		}

	}

}
