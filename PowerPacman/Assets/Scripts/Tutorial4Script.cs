using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial4Script : MonoBehaviour {

	public Text TitleText;
	public Text MessageText;


	// Use this for initialization
	void Start () {
	
		TitleText.text = DataScript.tutText.ConnectingScreenTitle;
		MessageText.text = DataScript.tutText.ConnectingScreenText;

	}

}
