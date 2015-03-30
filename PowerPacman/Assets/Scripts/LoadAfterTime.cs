using UnityEngine;
using System.Collections;

public class LoadAfterTime : MonoBehaviour {

    public int Delay = 5;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(Delay);
        Application.LoadLevel(2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
