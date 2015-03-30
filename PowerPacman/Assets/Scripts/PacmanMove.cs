﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class PacmanMove : MonoBehaviour {
	//public float speed = 0.4f;
	public float speed = 11.0f; //11 tiles per second
	Vector2 dest = Vector2.zero;
	Vector2 destTile = Vector2.zero;
	float corneringDistance = 0.5f; //the distance before a intersection when you can initiate a turn. MUST be less than .5

	//The ghosts
	public ClydeMove clyde;
	public InkyMove inky;
	public PinkyMove pinky;
	public GhostMove blinky;
	public Sprite scared;

	public static bool isPlayer1Turn = false;
	public static float turnDuration = 60.0f; //length of turn in seconds
	public static float turnTimeRemaining = turnDuration;

	public static bool powerMode = false;  //flag for power mode (eating enemies)
	public static float powerModeDuration = 5.0f; //number of seconds to stay in power mode
	public static float powerModeTimeRemaining = powerModeDuration;

	public static int player1Score = 0;
	public static bool pacmanEaten = false;
	float eatenTimeDelay = 0.5f; //the amount of time the player is frozen after being eaten
	float eatenDelayRemaining = 0;
	float flickerTime = .1f;
	float flickerTimeRemaining;

	Vector2 origin;

	public static GameObject[] dots;
	public static GameObject[] powerDot;
	public static List<GameObject> dotList;
	GameObject targetFood = null;
	List<Direction> queuedMovements;

	public static int dotsRemaining;

    private Direction movementDir;
    private Direction queuedDir;
    private MazeScript maze;

	Vector2 position;
	Vector2 tilePosition;	//this is the tile that pacman currently occupies, his "center"

    public enum Direction { None, Up, Down, Left, Right };

	// Use this for initialization
	void Start () {
		powerDot = GameObject.FindGameObjectsWithTag("powerDot");
		dots = GameObject.FindGameObjectsWithTag("dot");
		dotList = new List<GameObject> (dots);
		dotList.AddRange (powerDot);
		queuedMovements = new List<Direction> ();

		dest = transform.position;
		destTile = transform.position;
		origin = transform.position;
		tilePosition = new Vector2 (14, 14);
        queuedDir = Direction.None;
        maze = GameObject.FindGameObjectWithTag("maze").GetComponent<MazeScript>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		checkForGhostCollisions ();
		checkPacDots();

		if (pacmanEaten) {
			transform.position = origin;
			tilePosition = origin;
			dest = transform.position;
			destTile = transform.position;
			pacmanEaten = false;
			eatenDelayRemaining = eatenTimeDelay;
            movementDir = Direction.None;
            queuedDir = Direction.None;
			targetFood = null;
			queuedMovements.Clear();
		}

		// Move closer to Destination
		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			//Check for input if not moving
			if((Vector2)transform.position == dest)
            {
				if(isPlayer1Turn) 
				{
	                if(queuedDir != Direction.None)
	                {
	                    movementDir = queuedDir;
	                    queuedDir = Direction.None;
	                }
	                else
	                {
	                    if (Input.GetKey(KeyCode.UpArrow) && maze.validPacManMove(transform.position, Direction.Up))
	                    {
	                        movementDir = Direction.Up;
	                    }
						if (Input.GetKey(KeyCode.RightArrow) && maze.validPacManMove(transform.position, Direction.Right))
	                    {
	                        movementDir = Direction.Right;
	                    }  
						if (Input.GetKey(KeyCode.DownArrow) && maze.validPacManMove(transform.position, Direction.Down))
	                    {
	                        movementDir = Direction.Down;
	                    }
						if (Input.GetKey(KeyCode.LeftArrow) && maze.validPacManMove(transform.position, Direction.Left))
	                    { 
	                        movementDir = Direction.Left;
	                    }
						if (movementDir == Direction.Up && maze.validPacManMove(transform.position, Direction.Up))
	                    {
	                        dest = (Vector2)transform.position + Vector2.up;
	                        destTile.y++;
	                    }
						if (movementDir == Direction.Right && maze.validPacManMove(transform.position, Direction.Right))
	                    {
	                        dest = (Vector2)transform.position + Vector2.right;
	                        destTile.x++;
	                    }
						if (movementDir == Direction.Down && maze.validPacManMove(transform.position, Direction.Down))
	                    {
	                        dest = (Vector2)transform.position - Vector2.up;
	                        destTile.y--;
	                    }
						if (movementDir == Direction.Left && maze.validPacManMove(transform.position, Direction.Left))
	                    {
	                        dest = (Vector2)transform.position - Vector2.right;
	                        destTile.x--;
	                    }
	                }
				}
				else 
				{
					if(!GhostIsThere() || maze.ghosts.First().GetComponent<GhostMove>().isScared == true)
						MoveTowardsFood();
					else {
						targetFood = null;
						MoveAwayFromGhost();
					}
				}
			}
			else{
				//handle corners?
				if(Math.Abs (transform.position.x - dest.x) < corneringDistance && Math.Abs (transform.position.y - dest.y) < corneringDistance)
                {
					//The player is close to the destination, might be close to a corner
                    if (Input.GetKey(KeyCode.UpArrow) && maze.validPacManMove(dest, Direction.Up))
                    {
						queuedDir = Direction.Up;
					}
                    if (Input.GetKey(KeyCode.RightArrow) && maze.validPacManMove(dest, Direction.Right))
                    {
                        queuedDir = Direction.Right;
					}
                    if (Input.GetKey(KeyCode.DownArrow) && maze.validPacManMove(dest, Direction.Down))
                    {
                        queuedDir = Direction.Down;
					}
                    if (Input.GetKey(KeyCode.LeftArrow) && maze.validPacManMove(dest, Direction.Left))
                    {
                        queuedDir = Direction.Left;
					}
				}
			}
		}

		// Animation Parameters
		/*
		Vector2 dir = new Vector2 ();
		if (movementDir == Direction.Up) {
			dir.y = 1;
		}
		if (movementDir == Direction.Down) {
			dir.y = -1;
		}
		if (movementDir == Direction.Right) {
			dir.x = 1;
		}
		if (movementDir == Direction.Left) {
			dir.x = -1;
		}
		if (movementDir == Direction.None) {
			//full circle?
		}
		*/
		Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);

		if (maze.validPacManMove (transform.position, movementDir) || (Vector2)transform.position != tilePosition) {
			Vector2 p = Vector2.MoveTowards(transform.position, destTile, speed*Time.deltaTime);
			transform.position = p;

			//round to the nearest tile
			tilePosition.x = (int)Math.Round(transform.position.x,0);
			tilePosition.y = (int)Math.Round(transform.position.y,0);

		} else {
			//not a valid move
			dest = transform.position;
			destTile = tilePosition;
		}

		//update the score for player 1
		Text p1Score = GameObject.Find ("Top Canvas/ScoreBox").GetComponent<Text> ();
		p1Score.text = "" + player1Score;

		//decrease turn timer
		if (isPlayer1Turn) {
			if(turnTimeRemaining > 0){
				turnTimeRemaining -= Time.deltaTime;
				Text timeText = GameObject.Find("Top Canvas/TimeRemainingBox").GetComponent<Text>();
				timeText.text = "" + turnTimeRemaining;
			}
			else{
				Text timeText = GameObject.Find("Top Canvas/TimeRemainingBox").GetComponent<Text>();
				timeText.text = "0";        
			}
		}

		//if powermode, decrease timer
		if (powerMode) {
			if(powerModeTimeRemaining > 0){
				powerModeTimeRemaining -= Time.deltaTime;
				//in the last 2 seconds of power mode, make the ghost flicker
				if(powerModeTimeRemaining < 2){

					if(flickerTimeRemaining > 0){
						flickerTimeRemaining -= Time.deltaTime;
					}
					else{
						flickerTimeRemaining = flickerTime;
						foreach(var ghost in maze.ghosts)
						{
							if(ghost.GetComponent<GhostMove>().isScared == true){
								if(ghost.GetComponent<Animator>().enabled == false){
									ghost.GetComponent<Animator>().enabled = true;
								}
								else{
									ghost.GetComponent<SpriteRenderer>().sprite = scared;
									ghost.GetComponent<Animator>().enabled = false;
								}
							}
						}
					}

				}
			}
			else{
				powerMode = false;
                foreach(var ghost in maze.ghosts)
                {
                    ghost.GetComponent<Animator>().enabled = true;
                    ghost.GetComponent<GhostMove>().isScared = false;
                }
			}
		}

	}

	void checkForGhostCollisions(){

		//compare player tile position to the position of each of the ghosts
        foreach(var ghost in maze.ghosts)
        {
            var ghostMove = ghost.GetComponent<GhostMove>();
            if (tilePosition == ghostMove.tilePosition)
            {
                if (powerMode && ghostMove.isScared)
                {
                    ghostMove.killGhost();
                    player1Score += 100;
					flickerTimeRemaining = flickerTime;
                }
                else
                {
                    pacmanEaten = true;
                    return;
                }
            }
        }


	}

	void checkPacDots(){
		if (maze.isInGoodOccupiedTile(tilePosition)) {
			maze.eatDot(tilePosition);
		}
	}

	bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}

	//not checking if move is valid
	void MoveTowardsFood()
	{
		//find closestFood
		if (targetFood != null) {
			if ((targetFood.transform.position.x == transform.position.x) && (targetFood.transform.position.y == transform.position.y)) {
				dotList.Remove (targetFood);
				targetFood = null;
				queuedMovements.Clear ();
			}
		}
		if(targetFood == null) {
			targetFood = dotList.Aggregate ((d1, d2) => Vector2.Distance (transform.position, (Vector2)d1.transform.position) < Vector2.Distance (transform.position, (Vector2)d2.transform.position) ? d1 : d2);
		}
		if (queuedMovements.Count == 0) {
			queuedMovements = BFScaulations (targetFood);
		}
		movementDir = queuedMovements.FirstOrDefault ();
		if (queuedMovements.Count != 0) {
			queuedMovements.RemoveAt (0);
		}
		if (movementDir != Direction.None) {
			if (movementDir == Direction.Up && maze.validPacManMove (transform.position, Direction.Up)) {
				destTile.y++;
				dest = (Vector2)transform.position + Vector2.up;
			}
			if (movementDir == Direction.Right && maze.validPacManMove (transform.position, Direction.Right)) {
				destTile.x++;
				dest = (Vector2)transform.position + Vector2.right;
			}
			if (movementDir == Direction.Down && maze.validPacManMove (transform.position, Direction.Down)) {
				destTile.y--;
				dest = (Vector2)transform.position - Vector2.up;
			}
			if (movementDir == Direction.Left && maze.validPacManMove (transform.position, Direction.Left)) {
				destTile.x--;
				dest = (Vector2)transform.position - Vector2.right;
			}
		}
	}

	List<Direction> BFScaulations(GameObject targetFruit)
	{
		var froniter = new List<Node> ();
		var visitedList = new List<Node> ();
		froniter.Add (new Node() { vectorDirection = transform.position, parent = null });
		Node currentNode;
		Vector2 current = Vector2.zero;
		var path = new List<Direction> ();

		while (froniter.Any()) {
			//find node with least distance in froniter
			currentNode = froniter.First ();
			current = currentNode.vectorDirection;
			//if node is targetFruit
			if(current.x == targetFruit.transform.position.x && current.y == targetFruit.transform.position.y) {
				//return the Vectors coverted into Directions
				path = currentNode.convertVectorPathToDirections();
				break;
			}
			//gerernate nodes children
			//if node is not in visited list
				//add children to froniter
			if (maze.validPacManMove (current, Direction.Up) && !visitedList.Any(n => n.vectorDirection == current + Vector2.up)) {
				froniter.Add(new Node() { vectorDirection = current + Vector2.up, parent = currentNode });
			}
			if (maze.validPacManMove (current, Direction.Right) && !visitedList.Any(n => n.vectorDirection == current + Vector2.right)) {
				froniter.Add(new Node() { vectorDirection = current + Vector2.right, parent = currentNode });
			}
			if (maze.validPacManMove (current, Direction.Down) && !visitedList.Any(n => n.vectorDirection == current - Vector2.up)) {
				froniter.Add(new Node() { vectorDirection = current - Vector2.up, parent = currentNode });
			}
			if (maze.validPacManMove (current, Direction.Left) && !visitedList.Any(n => n.vectorDirection == current - Vector2.right)) {
				froniter.Add(new Node() { vectorDirection = current - Vector2.right, parent = currentNode });
			}
				
			//remove current from froniter
			froniter.Remove(currentNode);
			//add to visitedList
			visitedList.Add(currentNode);
		}

		return path;
	}

    bool GhostIsThere()
    {
		Vector2 pos = (Vector2)transform.position;
		foreach(var ghost in maze.ghosts) {
			if (Vector2.Distance(pos, (Vector2)ghost.transform.position) < 3)
				return true;
		}
		return false;
    }
	
	void MoveAwayFromGhost()
	{
		var values = new float[4];
		float max = 0;
		//find closest ghost
		GameObject closestGhost = maze.ghosts.FirstOrDefault(g => Vector2.Distance (transform.position, (Vector2)g.transform.position) < 3);
		//make move that creates the most distance between ghosts
		if (closestGhost != null) {
			if (maze.validPacManMove (transform.position, Direction.Up)) {
				values [0] = Vector2.Distance ((Vector2)transform.position + Vector2.up, (Vector2)closestGhost.transform.position);
			}
			if (maze.validPacManMove (transform.position, Direction.Down)) {
				values [1] = Vector2.Distance ((Vector2)transform.position - Vector2.up, (Vector2)closestGhost.transform.position);
			}
			if (maze.validPacManMove (transform.position, Direction.Right)) {
				values [2] = Vector2.Distance ((Vector2)transform.position + Vector2.right, (Vector2)closestGhost.transform.position);
			}
			if (maze.validPacManMove (transform.position, Direction.Left)) {
				values [3] = Vector2.Distance ((Vector2)transform.position - Vector2.right, (Vector2)closestGhost.transform.position);
			}
		}
		//make that move that create the most distance
		max = values.Max();
		if (max == values[0] && maze.validPacManMove (transform.position, Direction.Up)) {
			destTile.y++;
			dest = (Vector2)transform.position + Vector2.up;
		} else if (max == values[1] && maze.validPacManMove (transform.position, Direction.Down)) {
			destTile.y--;
			dest = (Vector2)transform.position - Vector2.up;
		} else if (max == values[2] && maze.validPacManMove (transform.position, Direction.Right)) {
			destTile.x++;
			dest = (Vector2)transform.position + Vector2.right;
		} else if (max == values[3] && maze.validPacManMove (transform.position, Direction.Left)) {
			destTile.x--;
			dest = (Vector2)transform.position - Vector2.right;
		}
	}
}
