using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

	public void load(int level){
		Application.LoadLevel (level);
	}

	public void checkIdInputAndLoad(string s){

		GameObject field = GameObject.FindGameObjectWithTag ("subjectIdInput");
		InputField computerId = GameObject.Find ("ComputerIdField").GetComponent<InputField> ();

		InputField input = field.GetComponent<InputField> ();

		string value = input.text;
		string compIdStr = computerId.text;

		if (value != "" && compIdStr != "") {
			int val = 0;
			bool res = int.TryParse(value, out val);
			if(res){
				//might not even have to do this check
				DataScript.playerId = val;
				DataScript.computerId = compIdStr;
				Debug.Log("User id: " + val);
				Debug.Log ("Computer id: " + compIdStr);
				load (1);
			}
			else{
				//not an int
			}
		} else {
			//display some error
		}
	}


}
