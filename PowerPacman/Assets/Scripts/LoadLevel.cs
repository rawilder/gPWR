using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

	public void load(int level){
		Application.LoadLevel (level);
	}

	public void checkIdInputAndLoad(string s){

		GameObject field = GameObject.FindGameObjectWithTag ("subjectIdInput");
		InputField input = field.GetComponent<InputField> ();
		string value = input.text;

		if (value != "") {
			//Debug.Log (value);

			int val = 0;
			bool res = int.TryParse(value, out val);
			if(res){
				DataScript.playerId = val;
				Debug.Log(val);
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
