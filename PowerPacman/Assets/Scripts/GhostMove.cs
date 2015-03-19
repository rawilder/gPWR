﻿using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour {
	public Transform[] waypoints;
	int cur = 0;
	public float speed = 0.3f;

	void FixedUpdate() {
		/// Waypoint not reached yet? then move closer
		if (transform.position != waypoints[cur].position) {
			Vector2 p = Vector2.MoveTowards(transform.position,
			                                waypoints[cur].position,
			                                speed);
			GetComponent<Rigidbody2D>().MovePosition(p);
		}
		// Waypoint reached, select next one
		else cur = (cur + 1) % waypoints.Length;
		
		// Animation
		Vector2 dir = waypoints[cur].position - transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "pacman") {
			
			if(PacmanMove.powerMode){
				Destroy(gameObject);
				PacmanMove.player1Score += 100;
			}
			else{
				Destroy (co.gameObject);
			}
		}
		//this is where we can decrease lives or show a game over screen
	}
}