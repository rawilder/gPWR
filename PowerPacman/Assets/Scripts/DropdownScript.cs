using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour {

	public Transform dropdown;

	public GameObject DropDownPrefab;

	void Start(){

		for (int i = 0; i < 5; i++) {
			GameObject button = (GameObject) Instantiate(DropDownPrefab);
			button.GetComponentInChildren<Text>().text = "Button " + i;
			int index = i;
			button.GetComponent<Button>().onClick.AddListener(
				() => {Debug.Log ("Button " + index + " clicked");}
			);
			button.transform.SetParent(dropdown,false);
		}

	}


	/*
	public void toggle(){
		if (dropdown.activeSelf == true) {
			dropdown.SetActive (false);
		} else {
			dropdown.SetActive(true);
		}
	}
	*/


}
