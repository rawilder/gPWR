using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class PacmanMove : MonoBehaviour {

	public string side;
	public bool isAIControlled;
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

	//public bool isPlayerTurn = true;
	public static float turnDuration = 15.0f; //length of turn in seconds
	public static float turnTimeRemaining = turnDuration;

	public bool powerMode = false;  //flag for power mode (eating enemies)
	public float powerModeDuration = 5.0f; //number of seconds to stay in power mode
	public float powerModeTimeRemaining;

	public int player1Score = 0;
	public bool pacmanEaten = false;
	float eatenTimeDelay = 0.5f; //the amount of time the player is frozen after being eaten
	float eatenDelayRemaining = 0;
	float flickerTime = .1f;
	float flickerTimeRemaining;

	Vector2 origin;

	public GameObject[] dots;
	public GameObject[] powerDot;
	GameObject cherry;
	GameObject targetGhost;
	public List<GameObject> dotList;
	GameObject targetFood = null;
	List<Direction> queuedMovements;

	public int dotsRemaining;

    private Direction movementDir;
    private Direction queuedDir;
	public GameObject m;
    private MazeScript maze;

	Vector2 position;
	Vector2 tilePosition;	//this is the tile that pacman currently occupies, his "center"
    public enum Direction { None, Up, Down, Left, Right };

    // powerup bools
    private bool PacmanSpeedPowerup { get; set; }
    private bool PowerModeDurationPowerup { get; set; }
	private bool GhostLowerSpeedPowerup { get; set; }

	public int ghostsEaten = 0;
	public int dotsEaten = 0;
	public int timesClearedMaze = 0;
	public int timesEaten = 0;
	public int cherriesEaten = 0;
	public int powerDotsEaten = 0;

	// Use this for initialization
	void Start () {
		powerDot = GameObject.FindGameObjectsWithTag(side+"PowerDot");
		dots = GameObject.FindGameObjectsWithTag(side+"Dot");
		dotList = new List<GameObject> (dots);
		dotList.AddRange (powerDot);
		queuedMovements = new List<Direction> ();

		powerModeTimeRemaining = powerModeDuration;
		//player1Score = 0;

		dest = transform.localPosition;
		destTile = transform.localPosition;
		origin = transform.localPosition;
		tilePosition = new Vector2 (14, 14);
        queuedDir = Direction.None;
		maze = m.GetComponent<MazeScript> ();

        // powerups testing
	    // SpeedPowerup = true;
        // PowerModeDurationPowerup = true;
		if (isAIControlled) {
			GhostLowerSpeedPowerup = true;
		}

        // powerups
        // going to need to replace when powerup interface is in place
        if (PacmanSpeedPowerup)
	    {
	        speed = 16;
	    }

	    if (PowerModeDurationPowerup)
	    {
	        powerModeDuration = 10;
	    }

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//this is needed here rather than start to be sure that maze.ghosts is populated before this is run
		if (GhostLowerSpeedPowerup) {
			foreach(var ghost in maze.ghosts){
				ghost.GetComponent<GhostMove>().speed = 7.5f;
			}
		}

		if (TurnManagerScript.paused) {
			return;
		}

		if ((isAIControlled && TurnManagerScript.isPlayerTurn) || (!isAIControlled && !TurnManagerScript.isPlayerTurn)) {
			//do nothing, wait your turn
			return;
		}

		checkForGhostCollisions ();
		checkPacDots();

		if (pacmanEaten) {
			transform.localPosition = origin;
			tilePosition = origin;
			dest = transform.localPosition;
			destTile = transform.localPosition;
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
			if((Vector2)transform.localPosition == dest)
            {
				if(!isAIControlled) 
				{
	                if(queuedDir != Direction.None)
	                {
	                    movementDir = queuedDir;
	                    queuedDir = Direction.None;
	                }
	                else
	                {
						if (Input.GetKey(KeyCode.UpArrow) && maze.validPacManMove(transform.localPosition, Direction.Up))
	                    {
	                        movementDir = Direction.Up;
	                    }
						if (Input.GetKey(KeyCode.RightArrow) && maze.validPacManMove(transform.localPosition, Direction.Right))
	                    {
	                        movementDir = Direction.Right;
	                    }  
						if (Input.GetKey(KeyCode.DownArrow) && maze.validPacManMove(transform.localPosition, Direction.Down))
	                    {
	                        movementDir = Direction.Down;
	                    }
						if (Input.GetKey(KeyCode.LeftArrow) && maze.validPacManMove(transform.localPosition, Direction.Left))
	                    { 
	                        movementDir = Direction.Left;
	                    }
						if (movementDir == Direction.Up && maze.validPacManMove(transform.localPosition, Direction.Up))
	                    {
							dest = (Vector2)transform.localPosition + Vector2.up;
	                        destTile.y++;
	                    }
						if (movementDir == Direction.Right && maze.validPacManMove(transform.localPosition, Direction.Right))
	                    {
							dest = (Vector2)transform.localPosition + Vector2.right;
	                        destTile.x++;
	                    }
						if (movementDir == Direction.Down && maze.validPacManMove(transform.localPosition, Direction.Down))
	                    {
							dest = (Vector2)transform.localPosition - Vector2.up;
	                        destTile.y--;
	                    }
						if (movementDir == Direction.Left && maze.validPacManMove(transform.localPosition, Direction.Left))
	                    {
							dest = (Vector2)transform.localPosition - Vector2.right;
	                        destTile.x--;
	                    }
	                }
				}
				else 
				{
					if(!GhostIsThere() || maze.ghosts.Any(g => g.GetComponent<GhostMove>().isScared))
						MoveTowardsFood();
					else {
						targetFood = null;
						MoveAwayFromGhost();
					}
				}
			}
			else{
				//handle corners?
				if(Math.Abs (transform.localPosition.x - dest.x) < corneringDistance && Math.Abs (transform.localPosition.y - dest.y) < corneringDistance)
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
		Vector2 dir = dest - (Vector2)transform.localPosition;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);

		if (maze.validPacManMove (transform.localPosition, movementDir) || (Vector2)transform.localPosition != tilePosition) {
			Vector2 p = Vector2.MoveTowards(transform.localPosition, destTile, speed*Time.deltaTime);
			transform.localPosition = p;

			//round to the nearest tile
			tilePosition.x = (int)Math.Round(transform.localPosition.x,0);
			tilePosition.y = (int)Math.Round(transform.localPosition.y,0);

		} else {
			//not a valid move
			dest = transform.localPosition;
			destTile = tilePosition;
		}

		//update the score for player 1
		Text p1Score = GameObject.Find (side+"/Top Canvas/ScoreBox").GetComponent<Text> ();
		p1Score.text = "" + player1Score;

		//decrease turn timer
		if(turnTimeRemaining > 0){
			turnTimeRemaining -= Time.deltaTime;
			Text timeText = GameObject.Find(side+"/Top Canvas/TimeRemainingBox").GetComponent<Text>();
			timeText.text = "" + (int)Math.Ceiling (turnTimeRemaining);
		}
		else{
			Text timeText = GameObject.Find(side+"/Top Canvas/TimeRemainingBox").GetComponent<Text>();
			timeText.text = "0";
			TurnManagerScript.switchingTurnsStage = true;

			if(isAIControlled){
				DataScript.aiScore = player1Score;
				DataScript.aiDotsEaten = dotsEaten;
				DataScript.aiGhostsEaten = ghostsEaten;
				DataScript.aiCherriesEaten = cherriesEaten;
				DataScript.aiPowerDotsEaten = powerDotsEaten;
				DataScript.aiTimesClearedMaze = timesClearedMaze;
				DataScript.aiTimesEaten = timesEaten;
			}
			else{
				DataScript.playerScore = player1Score;
				DataScript.playerDotsEaten = dotsEaten;
				DataScript.playerGhostsEaten = ghostsEaten;
				DataScript.playerCherriesEaten = cherriesEaten;
				DataScript.playerPowerDotsEaten = powerDotsEaten;
				DataScript.playerTimesClearedMaze = timesClearedMaze;
				DataScript.playerTimesEaten = timesEaten;
			}

			turnTimeRemaining = turnDuration;
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
					ghostsEaten++;
					flickerTimeRemaining = flickerTime;
                }
                else
                {
					timesEaten++;
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
		Vector2 pos = transform.localPosition;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}

	//not checking if move is valid
	void MoveTowardsFood()
	{
		if(maze.cherryObject.activeSelf && !dotList.Contains(cherry)) {
			cherry = new GameObject();
			cherry.transform.localPosition = maze.cherryLocation;
			dotList.Add(cherry);
		}
		if (targetFood != null) {
			if (((int)targetFood.transform.localPosition.x == transform.localPosition.x) && ((int)targetFood.transform.localPosition.y == transform.localPosition.y)) {
				dotList.Remove (targetFood);
				targetFood = null;
				queuedMovements.Clear ();
			}
		}
		if (targetFood == null) {
			if(CloseToCherry() && dotList.Contains(cherry)) {
				targetFood = cherry;
			} else if (GhostIsThere() && maze.ghosts.First().GetComponent<GhostMove> ().isScared) {
				targetGhost = new GameObject();
				targetGhost.transform.localPosition = maze.ghosts.Single(g => Vector2.Distance (transform.localPosition, (Vector2)g.transform.localPosition) < 3).transform.localPosition;
				targetFood = targetGhost;
				dotList.Add(targetFood);
			} else {
				targetFood = dotList.Aggregate ((d1, d2) => Vector2.Distance (transform.localPosition, (Vector2)d1.transform.localPosition) < Vector2.Distance (transform.localPosition, (Vector2)d2.transform.localPosition) ? d1 : d2);
			}
		}
		if (queuedMovements.Count == 0) {
			queuedMovements = AStarcaulations (targetFood);
		}
		movementDir = queuedMovements.FirstOrDefault ();
		if (queuedMovements.Count != 0) {
			queuedMovements.RemoveAt (0);
		}
		if (movementDir != Direction.None) {
			if (movementDir == Direction.Up && maze.validPacManMove (transform.localPosition, Direction.Up)) {
				destTile.y++;
				dest = (Vector2)transform.localPosition + Vector2.up;
			}
			if (movementDir == Direction.Right && maze.validPacManMove (transform.localPosition, Direction.Right)) {
				destTile.x++;
				dest = (Vector2)transform.localPosition + Vector2.right;
			}
			if (movementDir == Direction.Down && maze.validPacManMove (transform.localPosition, Direction.Down)) {
				destTile.y--;
				dest = (Vector2)transform.localPosition - Vector2.up;
			}
			if (movementDir == Direction.Left && maze.validPacManMove (transform.localPosition, Direction.Left)) {
				destTile.x--;
				dest = (Vector2)transform.localPosition - Vector2.right;
			}
		}
	}

	List<Direction> AStarcaulations(GameObject targetFruit)
	{
		var froniter = new List<Node> ();
		var visitedList = new List<Node> ();
		froniter.Add (new Node() { 
			vectorDirection = transform.localPosition, 
			parent = null,
			hueristic = Vector2.Distance (transform.localPosition, (Vector2)targetFruit.transform.localPosition)
		});
		Node currentNode;
		Vector2 current = Vector2.zero;
		var path = new List<Direction> ();
		int distanceConstant = 1;

		while (froniter.Any()) {
			//find node with least distance in froniter
			currentNode = froniter.OrderBy(t => t.hueristic).First();
			current = currentNode.vectorDirection;
			//if node is targetFruit
			if(current.x == (int)targetFruit.transform.localPosition.x && current.y == (int)targetFruit.transform.localPosition.y) {
				//return the Vectors coverted into Directions
				path = currentNode.convertVectorPathToDirections();
				break;
			}
			//gerernate nodes children
			//if node is not in visited list
				//add children to froniter
			if (maze.validPacManMove (current, Direction.Up) && !visitedList.Any(n => n.vectorDirection == current + Vector2.up)) {
				froniter.Add(new Node() { 
					vectorDirection = current + Vector2.up, 
					parent = currentNode, 
					hueristic = Vector2.Distance (current + Vector2.up, (Vector2)targetFruit.transform.localPosition) - distanceConstant
				});
			}
			if (maze.validPacManMove (current, Direction.Right) && !visitedList.Any(n => n.vectorDirection == current + Vector2.right)) {
				froniter.Add(new Node() { 
					vectorDirection = current + Vector2.right, 
					parent = currentNode, 
					hueristic = Vector2.Distance (current + Vector2.right, (Vector2)targetFruit.transform.localPosition) - distanceConstant });
			}
			if (maze.validPacManMove (current, Direction.Down) && !visitedList.Any(n => n.vectorDirection == current - Vector2.up)) {
				froniter.Add(new Node() { 
					vectorDirection = current - Vector2.up, 
					parent = currentNode, 
					hueristic = Vector2.Distance (current - Vector2.up, (Vector2)targetFruit.transform.localPosition) - distanceConstant });
			}
			if (maze.validPacManMove (current, Direction.Left) && !visitedList.Any(n => n.vectorDirection == current - Vector2.right)) {
				froniter.Add(new Node() { 
					vectorDirection = current - Vector2.right, 
					parent = currentNode, 
					hueristic = Vector2.Distance (current - Vector2.right, (Vector2)targetFruit.transform.localPosition) - distanceConstant });
			}
				
			//remove current from froniter
			froniter.Remove(currentNode);
			//add to visitedList
			visitedList.Add(currentNode);
		}

		return path;
	}

	bool CloseToCherry()
	{
		Vector2 pos = (Vector2)transform.localPosition;
		if (cherry == null)
			return false;
		if (Vector2.Distance (pos, (Vector2)cherry.transform.localPosition) < 3)
			return true;
		else
			return false;
	}

    bool GhostIsThere()
    {
		Vector2 pos = (Vector2)transform.localPosition;
		foreach(var ghost in maze.ghosts) {
			if (Vector2.Distance(pos, (Vector2)ghost.transform.localPosition) < 3)
				return true;
		}
		return false;
    }
	
	void MoveAwayFromGhost()
	{
		var values = new float[4];
		float max = 0;
		//find closest ghost
		GameObject closestGhost = maze.ghosts.FirstOrDefault(g => Vector2.Distance (transform.localPosition, (Vector2)g.transform.localPosition) < 3);
		//make move that creates the most distance between ghosts
		if (closestGhost != null) {
			if (maze.validPacManMove (transform.localPosition, Direction.Up)) {
				values [0] = Vector2.Distance ((Vector2)transform.localPosition + Vector2.up, (Vector2)closestGhost.transform.localPosition);
			}
			if (maze.validPacManMove (transform.localPosition, Direction.Down)) {
				values [1] = Vector2.Distance ((Vector2)transform.localPosition - Vector2.up, (Vector2)closestGhost.transform.localPosition);
			}
			if (maze.validPacManMove (transform.localPosition, Direction.Right)) {
				values [2] = Vector2.Distance ((Vector2)transform.localPosition + Vector2.right, (Vector2)closestGhost.transform.localPosition);
			}
			if (maze.validPacManMove (transform.localPosition, Direction.Left)) {
				values [3] = Vector2.Distance ((Vector2)transform.localPosition - Vector2.right, (Vector2)closestGhost.transform.localPosition);
			}
		}
		//make that move that create the most distance
		max = values.Max();
		if (max == values[0] && maze.validPacManMove (transform.localPosition, Direction.Up)) {
			destTile.y++;
			dest = (Vector2)transform.localPosition + Vector2.up;
		} else if (max == values[1] && maze.validPacManMove (transform.localPosition, Direction.Down)) {
			destTile.y--;
			dest = (Vector2)transform.localPosition - Vector2.up;
		} else if (max == values[2] && maze.validPacManMove (transform.localPosition, Direction.Right)) {
			destTile.x++;
			dest = (Vector2)transform.localPosition + Vector2.right;
		} else if (max == values[3] && maze.validPacManMove (transform.localPosition, Direction.Left)) {
			destTile.x--;
			dest = (Vector2)transform.localPosition - Vector2.right;
		}
	}
}
