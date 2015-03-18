using UnityEngine;
using System.Collections;

public class Pacdot : MonoBehaviour {

	public static int player1Score = 0;

	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "pacman") {
			Destroy(gameObject);
			player1Score++;
		}
		//scoring system would be here
	}
}
