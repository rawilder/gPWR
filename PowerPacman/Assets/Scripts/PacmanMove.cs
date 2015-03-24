using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PacmanMove : MonoBehaviour {
	//public float speed = 0.4f;
	public float speed = 11.0f; //11 tiles per second
	Vector2 dest = Vector2.zero;
	Vector2 destTile = Vector2.zero;

	//The ghosts
	public ClydeMove clyde;
	public InkyMove inky;
	public PinkyMove pinky;
	public GhostMove blinky;

	public static bool isPlayer1Turn = true;
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
	GameObject[] powerDots;
	public static int dotsRemaining;
	public static int powerDotsRemaining;

    private Direction movementDir;

	Vector2 position;
	Vector2 tilePosition;	//this is the tile that pacman currently occupies, his "center"

    public enum Direction { None, Up, Down, Left, Right };

	// Use this for initialization
	void Start () {
		dots = GameObject.FindGameObjectsWithTag("dot");
		powerDots = GameObject.FindGameObjectsWithTag ("powerDot");
		dest = transform.position;
		destTile = transform.position;
		origin = transform.position;
		position = new Vector2 (14, 14);
		tilePosition = new Vector2 (14, 14);

		dotsRemaining = GameObject.FindGameObjectsWithTag("dot").Length;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		checkForGhostCollisions ();

		if (pacmanEaten) {
			transform.position = origin;
			tilePosition = origin;
			dest = transform.position;
			destTile = transform.position;
			pacmanEaten = false;
			eatenDelayRemaining = eatenTimeDelay;
            movementDir = Direction.None;
		}

		if (dotsRemaining == 0) {
			for(int i = 0; i < dots.Length; i++){
				dots[i].SetActive(true);
			}
			for(int i = 0; i < powerDots.Length; i++){
				powerDots[i].SetActive(true);
			}
			dotsRemaining = dots.Length;
		}

		// Move closer to Destination
		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			//Check for input if not moving
			if((Vector2)transform.position == dest){

				if (Input.GetKey(KeyCode.UpArrow) && MazeScript.validPacManMove(transform.position, Direction.Up))
					movementDir = Direction.Up;
				if (Input.GetKey(KeyCode.RightArrow) && MazeScript.validPacManMove(transform.position, Direction.Right))
					movementDir = Direction.Right;
				if (Input.GetKey(KeyCode.DownArrow) && MazeScript.validPacManMove(transform.position, Direction.Down)){
					movementDir = Direction.Down;
				}
				if (Input.GetKey(KeyCode.LeftArrow) && MazeScript.validPacManMove(transform.position, Direction.Left))
					movementDir = Direction.Left;
				
				if (movementDir == Direction.Up && MazeScript.validPacManMove(transform.position, Direction.Up)){
					dest = (Vector2)transform.position + Vector2.up;
					destTile.y++;
				}
				if (movementDir == Direction.Right && MazeScript.validPacManMove(transform.position, Direction.Right)){
					dest = (Vector2)transform.position + Vector2.right;
					destTile.x++;
				}
				if (movementDir == Direction.Down && MazeScript.validPacManMove(transform.position, Direction.Down)){
					dest = (Vector2)transform.position - Vector2.up;
					destTile.y--;
				}
				if (movementDir == Direction.Left && MazeScript.validPacManMove(transform.position, Direction.Left)){
					dest = (Vector2)transform.position - Vector2.right;
					destTile.x--;
				}
			}
			else{
				//handle corners?
				if(Math.Abs (transform.position.x - dest.x) < .4 && Math.Abs (transform.position.y - dest.y) < .4){
					//The player is close to the destination, might be close to a corner
					if(Input.GetKey(KeyCode.UpArrow) && MazeScript.validPacManMove(transform.position,Direction.Up)){
						movementDir = Direction.Up;
					}
					if(Input.GetKey(KeyCode.RightArrow) && MazeScript.validPacManMove(transform.position,Direction.Right)){
						movementDir = Direction.Right;
					}
					if(Input.GetKey(KeyCode.DownArrow) && MazeScript.validPacManMove(transform.position,Direction.Down)){
						movementDir = Direction.Down;
					}
					if(Input.GetKey(KeyCode.LeftArrow) && MazeScript.validPacManMove(transform.position,Direction.Left)){
						movementDir = Direction.Left;
					}


					if(movementDir == Direction.Up && MazeScript.validPacManMove(transform.position,Direction.Up)){
						dest = (Vector2)destTile + Vector2.up;
						destTile.y++;
					}
					if(movementDir == Direction.Right && MazeScript.validPacManMove(transform.position,Direction.Right)){
						dest = (Vector2)destTile + Vector2.right;
						destTile.x++;
					}
					if(movementDir == Direction.Down && MazeScript.validPacManMove(transform.position,Direction.Down)){
						dest = (Vector2)destTile - Vector2.up;
						destTile.y--;
					}
					if(movementDir == Direction.Left && MazeScript.validPacManMove(transform.position,Direction.Left)){
						dest = (Vector2)destTile - Vector2.right;
						destTile.x--;
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
				//end turn?
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

	bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}
}
