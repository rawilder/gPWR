using UnityEngine;
using System.Collections;
using System;

public class GhostMove : MonoBehaviour {
	public Transform[] waypoints;
	int cur = 0;
	public float speed = 0.3f;

	float eatenDelayRemaining = 0.0f;

	public static float eatenDelay = 3.0f;

	Vector2 origin;
	public Vector2 tilePosition;
	
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
			                                speed * Time.deltaTime);
				GetComponent<Rigidbody2D> ().MovePosition (p);
			}
		// Waypoint reached, select next one
		else
				cur = (cur + 1) % waypoints.Length;
		
			// Animation
			Vector2 dir = waypoints [cur].position - transform.position;
			GetComponent<Animator> ().SetFloat ("DirX", dir.x);
			GetComponent<Animator> ().SetFloat ("DirY", dir.y);

			tilePosition.x = (int)Math.Round(transform.position.x,0);
			tilePosition.y = (int)Math.Round(transform.position.y,0);
		}
	}

	public void killGhost(){
		transform.position = origin;
		cur = 0;
		eatenDelayRemaining = GhostMove.eatenDelay;
	}
}
