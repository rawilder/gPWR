using UnityEngine;
using System.Collections;

public class PowerDot : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D co) {
		Debug.Log ("Collision detected");
		if (co.name == "pacman") {
			Debug.Log("Picked up power dot");
			PacmanMove.powerMode = true;
			PacmanMove.player1Score += 20;
			PacmanMove.powerModeTimeRemaining = PacmanMove.powerModeDuration;
			Destroy(gameObject);
		}
		//scoring system would be here
	}
}
