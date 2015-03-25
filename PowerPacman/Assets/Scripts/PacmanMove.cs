using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PacmanMove : MonoBehaviour {
	public float speed = 0.5f;
	Vector2 dest = Vector2.zero;

	public static bool isPlayer1Turn = false;
	public static float turnDuration = 50.0f; //length of turn in seconds
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
	GameObject[] powerDots;
	GameObject[] ghosts;
	GameObject targetFood = null;

	public static int dotsRemaining;
	public static int powerDotsRemaining;

    private Direction movementDir;

    enum Direction { None, Up, Down, Left, Right };

	// Use this for initialization
	void Start () {
		dots = GameObject.FindGameObjectsWithTag("dot");
		dotList = new List<GameObject> (dots);
		powerDots = GameObject.FindGameObjectsWithTag ("powerDot");
		ghosts = GameObject.FindGameObjectsWithTag("ghost");

		dest = transform.position;
		origin = transform.position;

		dotsRemaining = GameObject.FindGameObjectsWithTag("dot").Length;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (pacmanEaten) {
			transform.position = origin;
			dest = transform.position;
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
			dotList = new List<GameObject> (dots);
		}

		// Move closer to Destination
		Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
		GetComponent<Rigidbody2D>().MovePosition(p);

		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			// Check for Input if not moving
			if ((Vector2)transform.position == dest) {
                if (isPlayer1Turn)
                {
                    if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
                        movementDir = Direction.Up;
                    if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
                        movementDir = Direction.Right;
                    if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
                        movementDir = Direction.Down;
                    if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
                        movementDir = Direction.Left;

                    if (movementDir == Direction.Up && valid(Vector2.up))
                        dest = (Vector2)transform.position + Vector2.up;
                    if (movementDir == Direction.Right && valid(Vector2.right))
                        dest = (Vector2)transform.position + Vector2.right;
                    if (movementDir == Direction.Down && valid(-Vector2.up))
                        dest = (Vector2)transform.position - Vector2.up;
                    if (movementDir == Direction.Left && valid(-Vector2.right))
                        dest = (Vector2)transform.position - Vector2.right;
                }
                else
                {
					if(!GhostIsThere())
						dest = (Vector2)transform.position + MoveTowardsFood();
					else
						dest = (Vector2)transform.position + MoveAwayFromGhost();
                }
			}
		}

		// Animation Parameters
		Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);

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

	bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}

	//not checking if move is valid
	Vector2 MoveTowardsFood()
	{
		//find closestFood
		int distance = 1;
		if (targetFood != null) {
			if (targetFood.transform.position.x == transform.position.x && targetFood.transform.position.y == transform.position.y) {
				dotList.Remove (targetFood);
			}
		}
		while(targetFood == null) {
			targetFood = dotList.FirstOrDefault( d => Vector2.Distance(transform.position, (Vector2)d.transform.position) < distance );
			distance++;
		}
		Debug.Log (dotList.Count ());
		Debug.Log (targetFood.transform.position);
		if(targetFood.transform.position.y > transform.position.y && valid(Vector2.up))
			movementDir = Direction.Up;
		else if(targetFood.transform.position.y < transform.position.y && valid(-Vector2.up))
			movementDir = Direction.Down;
		else if (targetFood.transform.position.x > transform.position.x && valid(Vector2.right))
			movementDir = Direction.Right;
		else if (targetFood.transform.position.x < transform.position.x && valid(-Vector2.right))
			movementDir = Direction.Left;

		if (movementDir == Direction.Up && valid(Vector2.up))
			return Vector2.up;
		if (movementDir == Direction.Right && valid(Vector2.right))
			return Vector2.right;
		if (movementDir == Direction.Down && valid(-Vector2.up)) 
			return (-Vector2.up);
		if (movementDir == Direction.Left && valid(-Vector2.right))
			return (-Vector2.right);
		return Vector2.up;
	}

    bool GhostIsThere()
    {
		Vector2 pos = (Vector2)transform.position;
		for (int i = 0; i < ghosts.Length; ++i) {
			if (Vector2.Distance(pos, (Vector2)ghosts[i].transform.position) < 2)
				return true;
		}
		return false;
    }

	//Make the move that is the farest from the ghost
	Vector2 MoveAwayFromGhost()
	{
		var values = new float[4];
		float max = 0;
		if (valid (Vector2.up)) {
			values[0] = FurthestDistanceByMoving(Vector2.up);
		}
		if (valid (-Vector2.up)) {
			values[1] = FurthestDistanceByMoving(-Vector2.up);
		}
		if (valid (Vector2.right)) {
			values[2] = FurthestDistanceByMoving(Vector2.right);
		}
		if (valid (-Vector2.right)) {
			values[3] = FurthestDistanceByMoving(-Vector2.right);
		}
		max = values.Max();
		if (max == values[0])
			return Vector2.up;
		else if (max == values[1])
			return (-Vector2.up);
		else if (max == values[2])
			return Vector2.right;
		else
			return (-Vector2.right);
	}

	float FurthestDistanceByMoving(Vector2 dir)
	{
		Vector2 pos = (Vector2)transform.position + dir;
		float max = 0;
		float temp = 0;
		for (int i=0; i < ghosts.Length; ++i) {
			temp = Vector2.Distance(pos, (Vector2)ghosts[i].transform.position);
			if (temp > max)
				max = temp;
		}
		return max;
	}
}
