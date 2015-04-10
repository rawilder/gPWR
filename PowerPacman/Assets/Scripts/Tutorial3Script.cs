using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial3Script : MonoBehaviour {

	public Text Tut3Title;
	public Text Tut3Body;

	// Use this for initialization
	void Start () {
	
		Tut3Title.text = DataScript.tutText.BonusesScreenTitle;
		Tut3Body.text = DataScript.tutText.BonusesScreenBody;

	}
}
