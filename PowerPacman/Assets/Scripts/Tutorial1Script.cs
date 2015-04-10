using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial1Script : MonoBehaviour {

	public Text title;

	// Use this for initialization
	void Start () {
		title.text = DataScript.tutText.WelcomeScreenTitle;
	}
}
