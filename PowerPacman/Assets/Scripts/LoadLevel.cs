using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public void load(int level){
		Application.LoadLevel (level);
	}


}
