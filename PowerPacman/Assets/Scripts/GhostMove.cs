using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GhostMove : MonoBehaviour {
	public float speed = 10f;
	float eatenDelayRemaining = 0.0f;

	public static float eatenDelay = 1.0f;

	Vector2 origin;
	public Vector2 tilePosition;
	public PacmanMove.Direction moveDir = PacmanMove.Direction.None;
    public bool isScared;
	
	Vector2 dest;
	public GameObject m; //the maze gameobject
    private MazeScript maze;
	
    // powerups
    private bool GhostLowerSpeedPowerup { get; set; }

	void Start(){
		origin = transform.localPosition;
		dest = origin;
        //maze = GameObject.FindGameObjectWithTag("maze").GetComponent<MazeScript>();
		maze = m.GetComponent<MazeScript> ();
        isScared = false;

        // powerups testing
        // GhostLowerSpeedPowerup = true;

        //powerups 
	    if (GhostLowerSpeedPowerup)
	    {
	        speed = 5;
	    }

	}

	void FixedUpdate() {

		if (TurnManagerScript.paused) {
			return;
		}

		if (maze.pacman.isAIControlled && TurnManagerScript.isPlayerTurn) {
			//do nothing if these are AI ghosts and it is the other players turn
			return;
		}

		if (!maze.pacman.isAIControlled && !TurnManagerScript.isPlayerTurn) {
			//do nothing if these are ghosts for the real player, and it is the AI turn
			return;
		}

		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			if((Vector2)transform.localPosition == dest){
				//check if the ghost is inside the pen
                if (maze.isInGhostPen(transform.localPosition))
                {
					//exit the pen
					moveDir = PacmanMove.Direction.Up;
					dest = new Vector2(14,20);
				}
				else{
					
					
					/*-------------------------------------------------------*/
					//this section is probably where actual intelligent path planning would go
					
					//dumb ai: check all directions to see if there is a turn that could be made
                    List<bool> availableDirections = maze.getAvailableDirections(transform.localPosition);
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

                    if (moveDir == PacmanMove.Direction.Up && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Up))
                    {
						dest = (Vector2)transform.localPosition + Vector2.up;
					}
					if (moveDir == PacmanMove.Direction.Right && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Right))
                    {
						dest = (Vector2)transform.localPosition + Vector2.right;
					}
					if (moveDir == PacmanMove.Direction.Down && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Down))
                    {
						dest = (Vector2) transform.localPosition - Vector2.up;
					}
					if (moveDir == PacmanMove.Direction.Left && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Left))
                    {
						dest = (Vector2)transform.localPosition - Vector2.right;
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
				Vector2 p = Vector2.MoveTowards(transform.localPosition, dest, speed*Time.deltaTime);
				transform.localPosition = p;
			}

			tilePosition.x = (int)Math.Round(transform.localPosition.x,0);
			tilePosition.y = (int)Math.Round(transform.localPosition.y,0);
		}
	}

	public void killGhost(){
		transform.localPosition = origin;
		tilePosition = origin;
		dest = origin;
		eatenDelayRemaining = GhostMove.eatenDelay;
        GetComponent<Animator>().enabled = true;
        isScared = false;
	}
}
