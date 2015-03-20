using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PacmanMove : MonoBehaviour {
	public float speed = 0.4f;
	Vector2 dest = Vector2.zero;

	public static bool isPlayer1Turn = true;
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



	// Use this for initialization
	void Start () {
		dest = transform.position;
		origin = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (pacmanEaten) {
			transform.position = origin;
			dest = transform.position;
			pacmanEaten = false;
			eatenDelayRemaining = eatenTimeDelay;
		}

		// Move closer to Destination
		Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
		GetComponent<Rigidbody2D>().MovePosition(p);

		if (eatenDelayRemaining > 0) {
			eatenDelayRemaining -= Time.deltaTime;
		} else {
			// Check for Input if not moving
			if ((Vector2)transform.position == dest) {
				if (Input.GetKey (KeyCode.UpArrow) && valid (Vector2.up))
					dest = (Vector2)transform.position + Vector2.up;
				if (Input.GetKey (KeyCode.RightArrow) && valid (Vector2.right))
					dest = (Vector2)transform.position + Vector2.right;
				if (Input.GetKey (KeyCode.DownArrow) && valid (-Vector2.up))
					dest = (Vector2)transform.position - Vector2.up;
				if (Input.GetKey (KeyCode.LeftArrow) && valid (-Vector2.right))
					dest = (Vector2)transform.position - Vector2.right;
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

	bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}
}
