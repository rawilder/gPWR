using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2Script : MonoBehaviour {

	public Text Tut2Body;

	// Use this for initialization
	void Start () {
		Tut2Body.text = DataScript.tutText.ObjectiveScreenBody;
	}
}
