using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

	public void load(int level){
		Application.LoadLevel (level);
	}

}
