using UnityEngine;
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

	Vector2 origin;

	GameObject[] dots;
	List<GameObject> dotList;
	GameObject[] ghosts;
	GameObject targetFood = null;

	public static int dotsRemaining;
	public static int powerDotsRemaining;

    private Direction movementDir;
    private Direction queuedDir;

	Vector2 position;
	Vector2 tilePosition;	//this is the tile that pacman currently occupies, his "center"

    public enum Direction { None, Up, Down, Left, Right };

	// Use this for initialization
	void Start () {
		dots = GameObject.FindGameObjectsWithTag("dot");
		dotList = new List<GameObject> (dots);
		ghosts = GameObject.FindGameObjectsWithTag("ghost");

		dest = transform.position;
		destTile = transform.position;
		origin = transform.position;
		//position = new Vector2 (14, 14);
		tilePosition = new Vector2 (14, 14);
        queuedDir = Direction.None;
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
	                    if (Input.GetKey(KeyCode.UpArrow) && MazeScript.validPacManMove(transform.position, Direction.Up))
	                    {
	                        movementDir = Direction.Up;
	                    }
	                    if (Input.GetKey(KeyCode.RightArrow) && MazeScript.validPacManMove(transform.position, Direction.Right))
	                    {
	                        movementDir = Direction.Right;
	                    }  
	                    if (Input.GetKey(KeyCode.DownArrow) && MazeScript.validPacManMove(transform.position, Direction.Down))
	                    {
	                        movementDir = Direction.Down;
	                    }
	                    if (Input.GetKey(KeyCode.LeftArrow) && MazeScript.validPacManMove(transform.position, Direction.Left))
	                    { 
	                        movementDir = Direction.Left;
	                    }
	                    if (movementDir == Direction.Up && MazeScript.validPacManMove(transform.position, Direction.Up))
	                    {
	                        dest = (Vector2)transform.position + Vector2.up;
	                        destTile.y++;
	                    }
	                    if (movementDir == Direction.Right && MazeScript.validPacManMove(transform.position, Direction.Right))
	                    {
	                        dest = (Vector2)transform.position + Vector2.right;
	                        destTile.x++;
	                    }
	                    if (movementDir == Direction.Down && MazeScript.validPacManMove(transform.position, Direction.Down))
	                    {
	                        dest = (Vector2)transform.position - Vector2.up;
	                        destTile.y--;
	                    }
	                    if (movementDir == Direction.Left && MazeScript.validPacManMove(transform.position, Direction.Left))
	                    {
	                        dest = (Vector2)transform.position - Vector2.right;
	                        destTile.x--;
	                    }
	                }
				}
				else 
				{
					if(!GhostIsThere())
						MoveTowardsFood();
					else
						MoveAwayFromGhost();
				}
			}
			else{
				//handle corners?
				if(Math.Abs (transform.position.x - dest.x) < corneringDistance && Math.Abs (transform.position.y - dest.y) < corneringDistance)
                {
					//The player is close to the destination, might be close to a corner
                    if (Input.GetKey(KeyCode.UpArrow) && MazeScript.validPacManMove(dest, Direction.Up))
                    {
						queuedDir = Direction.Up;
					}
                    if (Input.GetKey(KeyCode.RightArrow) && MazeScript.validPacManMove(dest, Direction.Right))
                    {
                        queuedDir = Direction.Right;
					}
                    if (Input.GetKey(KeyCode.DownArrow) && MazeScript.validPacManMove(dest, Direction.Down))
                    {
                        queuedDir = Direction.Down;
					}
                    if (Input.GetKey(KeyCode.LeftArrow) && MazeScript.validPacManMove(dest, Direction.Left))
                    {
                        queuedDir = Direction.Left;
					}
				}
			}
		}

		// Animation Parameters
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
		//Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);

		if (MazeScript.validPacManMove (transform.position, movementDir) || (Vector2)transform.position != tilePosition) {
			Vector2 p = Vector2.MoveTowards(transform.position, destTile, speed*Time.deltaTime);
			transform.position = p;

			//round to the nearest tile
			tilePosition.x = (int)Math.Round(transform.position.x,0);
			tilePosition.y = (int)Math.Round(transform.position.y,0);

		} else {
			//not a valid move
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
			}
			else{
				powerMode = false;
			}
		}

	}

	void checkForGhostCollisions(){

		//compare player tile position to the position of each of the ghosts

		//clyde
		if (tilePosition == clyde.tilePosition) {
			if(powerMode){
				clyde.killGhost();
				player1Score+=100;
			}
			else{
				pacmanEaten = true;
				return;
			}
		}

		if (tilePosition == inky.tilePosition) {
			if(powerMode){
				inky.killGhost();
				player1Score+=100;
			}
			else{
				pacmanEaten = true;
				return;
			}
		}

		if (tilePosition == pinky.tilePosition) {
			if(powerMode){
				pinky.killGhost();
				player1Score+=100;
			}
			else{
				pacmanEaten = true;
				return;
			}
		}

		if (tilePosition == blinky.tilePosition) {
			if(powerMode){
				blinky.killGhost();
				player1Score+=100;
			}
			else{
				pacmanEaten = true;
				return;
			}
		}

	}

	void checkPacDots(){
		if (MazeScript.isInDotTile (tilePosition) || MazeScript.isInPowerDotTile(tilePosition)) {
			MazeScript.eatDot(tilePosition);
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
		int distance = 1;
		if (targetFood != null) {
			if (targetFood.transform.position.x == transform.position.x && targetFood.transform.position.y == transform.position.y) {
				dotList.Remove (targetFood);
				targetFood = null;
			}
		}
		while(targetFood == null) {
			targetFood = dotList.FirstOrDefault( d => Vector2.Distance(transform.position, (Vector2)d.transform.position) < distance );
			distance++;
		}
		Debug.Log (distance);
		Debug.Log (targetFood.transform.position);
		Debug.Log (transform.position);
		if (MazeScript.validPacManMove (transform.position, movementDir) && movementDir != Direction.None && getCloserToFood (movementDir)) {
		} else {
			if (targetFood.transform.position.y >= transform.position.y && MazeScript.validPacManMove (transform.position, Direction.Up))
				movementDir = Direction.Up;
			else if (targetFood.transform.position.y <= transform.position.y && MazeScript.validPacManMove (transform.position, Direction.Down))
				movementDir = Direction.Down;
			else if (targetFood.transform.position.x >= transform.position.x && MazeScript.validPacManMove (transform.position, Direction.Right))
				movementDir = Direction.Right;
			else if (targetFood.transform.position.x <= transform.position.x && MazeScript.validPacManMove (transform.position, Direction.Left))
				movementDir = Direction.Left;
		}

		Debug.Log (movementDir);
		if (movementDir == Direction.Up && MazeScript.validPacManMove(transform.position, Direction.Up)) {
			destTile.y++;
			dest = (Vector2)transform.position + Vector2.up;
		}
		if (movementDir == Direction.Right && MazeScript.validPacManMove(transform.position, Direction.Right)) {
			destTile.x++;
			dest = (Vector2)transform.position + Vector2.right;
		}
		if (movementDir == Direction.Down && MazeScript.validPacManMove(transform.position, Direction.Down)) {
			destTile.y--;
			dest = (Vector2)transform.position - Vector2.up;
		}
		if (movementDir == Direction.Left && MazeScript.validPacManMove(transform.position, Direction.Left)) {
			destTile.x--;
			dest = (Vector2)transform.position - Vector2.right;
		}
	}

    bool GhostIsThere()
    {
		Vector2 pos = (Vector2)transform.position;
		for (int i = 0; i < ghosts.Length; ++i) {
			if (Vector2.Distance(pos, (Vector2)ghosts[i].transform.position) < 3)
				return true;
		}
		return false;
    }
	
	void MoveAwayFromGhost()
	{
		var values = new float[4];
		float max = 0;
		//find closest ghost
		GameObject closestGhost = ghosts.FirstOrDefault(g => Vector2.Distance (transform.position, (Vector2)g.transform.position) < 3);
		//make move that creates the most distance between ghosts
		if (closestGhost != null) {
			if (MazeScript.validPacManMove (transform.position, Direction.Up)) {
				values [0] = Vector2.Distance ((Vector2)transform.position + Vector2.up, (Vector2)closestGhost.transform.position);
			}
			if (MazeScript.validPacManMove (transform.position, Direction.Down)) {
				values [1] = Vector2.Distance ((Vector2)transform.position - Vector2.up, (Vector2)closestGhost.transform.position);
			}
			if (MazeScript.validPacManMove (transform.position, Direction.Right)) {
				values [2] = Vector2.Distance ((Vector2)transform.position + Vector2.right, (Vector2)closestGhost.transform.position);
			}
			if (MazeScript.validPacManMove (transform.position, Direction.Left)) {
				values [3] = Vector2.Distance ((Vector2)transform.position - Vector2.right, (Vector2)closestGhost.transform.position);
			}
		}
		//make that move that create the most distance
		max = values.Max();
		if (max == values[0] && MazeScript.validPacManMove (transform.position, Direction.Up)) {
			destTile.y++;
			dest = (Vector2)transform.position + Vector2.up;
		} else if (max == values[1] && MazeScript.validPacManMove (transform.position, Direction.Down)) {
			destTile.y--;
			dest = (Vector2)transform.position - Vector2.up;
		} else if (max == values[2] && MazeScript.validPacManMove (transform.position, Direction.Right)) {
			destTile.x++;
			dest = (Vector2)transform.position + Vector2.right;
		} else if ( max == values[3] && MazeScript.validPacManMove (transform.position, Direction.Left)) {
			destTile.x--;
			dest = (Vector2)transform.position - Vector2.right;
		}
	}

	bool getCloserToFood(PacmanMove.Direction dir)
	{
		Vector2 direction;
		if (dir == Direction.Up) {
			direction = Vector2.up;
		} else if (dir == Direction.Down) {
			direction = (-Vector2.up);
		} else if (dir == Direction.Right) {
			direction = Vector2.right;
		} else if (dir == Direction.Left) {
			direction = (-Vector2.right);
		} else {
			direction = Vector2.zero;
		}
		Vector2 pos = (Vector2)transform.position + direction;
		if (Vector2.Distance (transform.position, targetFood.transform.position) > Vector2.Distance (pos, targetFood.transform.position))
			return true;
		else
			return false;
	}
}
