using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	GameObject[] powerDots;
    GameObject blinky;
    GameObject clyde;
    GameObject inky;
    GameObject pinky;

	public static int dotsRemaining;
	public static int powerDotsRemaining;

    private Direction movementDir;

    enum Direction { None, Up, Down, Left, Right };

	// Use this for initialization
	void Start () {
		dots = GameObject.FindGameObjectsWithTag("dot");
		powerDots = GameObject.FindGameObjectsWithTag ("powerDot");
        blinky = GameObject.Find("blinky");
        clyde = GameObject.Find("clyde");
        inky = GameObject.Find("inky");
        pinky = GameObject.Find("pinky");

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
                    if (valid(Vector2.up) && !GhostIsThere(Vector2.up))
                    {
                        dest = (Vector2)transform.position + Vector2.up;
                    }
					else if (valid(-Vector2.right) && !GhostIsThere(-Vector2.right))
					{
						dest = (Vector2)transform.position - Vector2.right;
					}
					else if (valid(-Vector2.up) && !GhostIsThere(-Vector2.up))
					{
						dest = (Vector2)transform.position - Vector2.up;
					}
					else if (valid(Vector2.right) && !GhostIsThere(Vector2.right))
					{
						dest = (Vector2)transform.position + Vector2.right;
                    }
					else 
					{
						dest = (Vector2)transform.position + MoveAwayFromGhost();
					}
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

    bool GhostIsThere(Vector2 dir)
    {
		Vector2 pos = (Vector2)transform.position + dir;
		Debug.Log (Vector2.Distance(pos, (Vector2)blinky.transform.position));
		if (Vector2.Distance(pos, (Vector2)blinky.transform.position) < 5)
			return true;
		else if (Vector2.Distance(pos, (Vector2)inky.transform.position) < 5)
			return true;
		else if (Vector2.Distance(pos, (Vector2)clyde.transform.position) < 5)
			return true;
		else if (Vector2.Distance(pos, (Vector2)pinky.transform.position) < 5)
			return true;
		else
            return false;
    }

	//Make the move that is the farest from the ghost
	Vector2 MoveAwayFromGhost()
	{
		float up, down, left, right, max;
		up = FurthestDistanceByMoving(Vector2.up);
		down = FurthestDistanceByMoving (-Vector2.up);
		//left
		//right
		max = Mathf.Max(right, Mathf.Max(left,Mathf.Max (up,down)));
		if (max == up)
			return Vector2.up;
		else if (max == down)
			return (-Vector2.up);
		else if (max == right)
			return Vector2.right;
		else
			return (-Vector2.right);
	}
}
