using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour {
	public Transform[] waypoints;
	int cur = 0;
	public float speed = 0.3f;

	float eatenDelayRemaining = 0.0f;

	public static float eatenDelay = 3.0f;

	Vector2 origin;
	
	void Start(){
		origin = transform.position;
	}

	void FixedUpdate() {

		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			/// Waypoint not reached yet? then move closer
			if (transform.position != waypoints [cur].position) {
				Vector2 p = Vector2.MoveTowards (transform.position,
			                                waypoints [cur].position,
			                                speed);
				GetComponent<Rigidbody2D> ().MovePosition (p);
			}
		// Waypoint reached, select next one
		else
				cur = (cur + 1) % waypoints.Length;
		
			// Animation
			Vector2 dir = waypoints [cur].position - transform.position;
			GetComponent<Animator> ().SetFloat ("DirX", dir.x);
			GetComponent<Animator> ().SetFloat ("DirY", dir.y);
		}
	}

	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "pacman") {
			
			if(PacmanMove.powerMode){
				transform.position = origin;
				cur = 0;
				eatenDelayRemaining = GhostMove.eatenDelay;
				PacmanMove.player1Score += 100;
			}
			else{
//				Destroy (co.gameObject);
				PacmanMove.pacmanEaten = true;
			}
		}
		//this is where we can decrease lives or show a game over screen
	}
}
