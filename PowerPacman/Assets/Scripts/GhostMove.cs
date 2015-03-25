using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GhostMove : MonoBehaviour {
	public float speed = 0.3f;

	float eatenDelayRemaining = 0.0f;

	public static float eatenDelay = 3.0f;

	Vector2 origin;
	public Vector2 tilePosition;
	public PacmanMove.Direction moveDir = PacmanMove.Direction.None;
	
	Vector2 dest;
	
	void Start(){
		origin = transform.position;
		dest = origin;
	}

	void FixedUpdate() {

		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			if((Vector2)transform.position == dest){
				//check if the ghost is inside the pen
				if(MazeScript.getValue(transform.position) == 3){
					//exit the pen
					moveDir = PacmanMove.Direction.Up;
					dest = new Vector2(14,20);
				}
				else{
					
					
					/*-------------------------------------------------------*/
					//this section is probably where actual intelligent path planning would go
					
					//dumb ai: check all directions to see if there is a turn that could be made
					List<bool> availableDirections = MazeScript.getAvailableDirections(transform.position);
					for(int i = 0; i < 4; i++){
						//if there is an available direction to travel
						if(availableDirections[i]){
							//take this turn with a 25% probability
							
							if(UnityEngine.Random.value <= .25){
								//dont allow him to reverse direction
								if(i == 0 && moveDir != PacmanMove.Direction.Down) moveDir = PacmanMove.Direction.Up;
								if(i == 1 && moveDir != PacmanMove.Direction.Left) moveDir = PacmanMove.Direction.Right;
								if(i == 2 && moveDir != PacmanMove.Direction.Up) moveDir = PacmanMove.Direction.Down;
								if(i == 3 && moveDir != PacmanMove.Direction.Right) moveDir = PacmanMove.Direction.Left;
							}
						}
					}
					
					/*-------------------------------------------------------*/
					
					if (moveDir == PacmanMove.Direction.Up && MazeScript.validPacManMove(transform.position, PacmanMove.Direction.Up)){
						dest = (Vector2)transform.position + Vector2.up;
					}
					if (moveDir == PacmanMove.Direction.Right && MazeScript.validPacManMove(transform.position, PacmanMove.Direction.Right)){
						dest = (Vector2)transform.position + Vector2.right;
					}
					if (moveDir == PacmanMove.Direction.Down && MazeScript.validPacManMove(transform.position, PacmanMove.Direction.Down)){
						dest = (Vector2)transform.position - Vector2.up;
					}
					if (moveDir == PacmanMove.Direction.Left && MazeScript.validPacManMove(transform.position, PacmanMove.Direction.Left)){
						dest = (Vector2)transform.position - Vector2.right;
					}
				}
			}

			//set the movement direction for the sprite
			Vector2 dir = new Vector2(0,0);
			if (moveDir == PacmanMove.Direction.Up) {
				dir.y = 1;
			}
			if (moveDir == PacmanMove.Direction.Down) {
				dir.y = -1;
			}
			if (moveDir == PacmanMove.Direction.Right) {
				dir.x = 1;
			}
			if (moveDir == PacmanMove.Direction.Left) {
				dir.x = -1;
			}
			
			// Animation
			GetComponent<Animator> ().SetFloat ("DirX", dir.x);
			GetComponent<Animator> ().SetFloat ("DirY", dir.y);
			
			if(moveDir != PacmanMove.Direction.None){
				Vector2 p = Vector2.MoveTowards(transform.position, dest, speed*Time.deltaTime);
				transform.position = p;
			}

			tilePosition.x = (int)Math.Round(transform.position.x,0);
			tilePosition.y = (int)Math.Round(transform.position.y,0);
		}
	}

	public void killGhost(){
		transform.position = origin;
		tilePosition = origin;
		dest = origin;
		eatenDelayRemaining = GhostMove.eatenDelay;
	}
}
