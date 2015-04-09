using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Position : IEquatable<Position>{
	public int x;
	public int y;
	public Position(int _x, int _y){
		x = _x;
		y = _y;
	}

	public override int GetHashCode(){
		return x + (100 * y);
	}

	public override bool Equals(System.Object o){
		if (o == null)
			return false;
		Position p = o as Position;
		if ((System.Object)p == null)
			return false;
		if (x == p.x && y == p.y)
			return true;
		return false;
	}

	public bool Equals(Position p){
		if (p == null)
			return false;
		if (x == p.x && y == p.y)
			return true;
		return false;
	}
}

public class Node {
	public Vector2 vectorDirection;
	public Node parent;
	public PacmanMove.Direction direction;
	public float hueristic;

	public List<PacmanMove.Direction> convertVectorPathToDirections()
	{
		var path = new List<PacmanMove.Direction> ();
		Node node = this;
		while (node.parent != null) {
			path.Insert (0, covertVectorToDirection(node));
			node = node.parent;
		}
		return path;
	}

	public PacmanMove.Direction covertVectorToDirection(Node node)
	{
		Vector2 nodeDirection = node.vectorDirection - node.parent.vectorDirection;
		var direction = PacmanMove.Direction.None;
		if (nodeDirection == Vector2.up) {
			direction = PacmanMove.Direction.Up;
		} else if (nodeDirection == (-Vector2.up)) {
			direction = PacmanMove.Direction.Down;
		} else if (nodeDirection == Vector2.right) {
			direction = PacmanMove.Direction.Right;
		} else if (nodeDirection == (-Vector2.right)) {
			direction = PacmanMove.Direction.Left;
		}

		return direction;
	}
}

public class MazeScript : MonoBehaviour {

	public string side;

	int dotPointValue = 1;
	int powerDotPointValue = 20;

	public int offsetX;
	public int offsetY;

	public  int dotsRemaining;
	public  int powerDotsRemaining;

    public Sprite ghostScared;
	int cherryValue = 100;

	//0 = null space, used to pad the left and bottom sides of the maze
	//1 = wall
	//2 = blank space containing a dot with no cherry
	//3 = 'ghost zone', is inaccessible to pacman
	//4 = blank space with no dot present and no cherry
	//5 = blank space containing a power dot
	//6 = blank space with no power dot present
	//7 = blank space containing a dot and a cherry
	//8 = blank space containing a cherry and no dot

	static int nullSpace = 0;
	static int wall = 1;
	static int justDot = 2;
	static int ghostPen = 3;
	static int noDotNoCherry = 4;
	static int powerDot = 5;
	static int noPowerDot = 6;
	static int dotAndCherry = 7;
	static int justCherry = 8;

	//the origin of the map is the bottom left
	//map[2][2] is the bottom left most dot, and is at position x = 2, y = 2
	//with map[x,y]...
	//	Direction up = x+1,y
	//	direction down = x-1,y
	//	direction right = x,y+1
	//	direction left = x,y-1
	int[,] map = new int[,]{
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
		{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		{0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,1},
		{0,1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1},
		{0,1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1},
		{0,1,5,2,2,1,1,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,1,1,2,2,5,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
		{0,1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1},
		{0,1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1},
		{0,1,2,2,2,1,1,2,1,1,2,2,2,2,2,2,2,2,2,2,1,1,2,1,1,2,2,2,1},
		{0,1,2,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,2,1},
		{0,1,2,1,1,1,1,2,1,1,2,1,3,3,3,3,3,3,1,2,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,2,2,2,1,1,2,1,3,3,3,3,3,3,1,2,1,1,2,2,2,2,2,2,1},
		{0,1,2,1,1,1,1,2,1,1,2,1,3,3,3,3,3,3,1,2,1,1,2,1,1,1,1,2,1},
		{0,1,2,1,1,1,1,2,1,1,2,1,1,1,3,3,1,1,1,2,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1},
		{0,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1},
		{0,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1},
		{0,1,2,2,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,1},
		{0,1,2,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,2,1},
		{0,1,2,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,5,1,1,1,1,2,1,1,1,1,1,2,2,2,2,1,1,1,1,1,2,1,1,1,1,5,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
		{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
	};

	//An associative array, uses a position as a key to find a particular dot's GameObject
	IDictionary<Position,GameObject> dots = new Dictionary<Position, GameObject>();
	IDictionary<Position,GameObject> powerDots = new Dictionary<Position, GameObject>();
    public IList<GameObject> ghosts = new List<GameObject>();

	GameObject[] dotsList;
	GameObject[] powerDotsList;
	public PacmanMove pacman;

	public Vector2 cherryLocation = new Vector2(14,11);
	public float cherryRespawnTime = 10.0f;
	float cherryRespawnTimeRemaining;
	bool cherryEaten = true;
	public GameObject cherryObject;

    //powerballs respawn
    public bool powerDotRespawns { get; set; }
    public float powerDotRespawnTime = 10.0f;
    float powerDotsRespawnTimeRemaining;

	Transform screen;
	
	// Use this for initialization
	void Start () {

		screen = transform.FindChild ("semitransparent");

		//initialize the associate arrays
		dotsList = GameObject.FindGameObjectsWithTag(side+"Dot");
		powerDotsList = GameObject.FindGameObjectsWithTag(side+"PowerDot");
		for(int i = 0; i < dotsList.GetLength(0); i++){
			Position p = new Position((int)dotsList[i].transform.localPosition.x, (int)dotsList[i].transform.localPosition.y);
			dots[p] = dotsList[i];
		}

		for (int i = 0; i < powerDotsList.GetLength(0); i++) {
			Position p = new Position((int)powerDotsList[i].transform.localPosition.x, (int)powerDotsList[i].transform.localPosition.y);
			powerDots[p] = powerDotsList[i];
		}

        ghosts.Add(GameObject.FindGameObjectWithTag(side+"blinky"));
        ghosts.Add(GameObject.FindGameObjectWithTag(side+"pinky"));
        ghosts.Add(GameObject.FindGameObjectWithTag(side+"inky"));
        ghosts.Add(GameObject.FindGameObjectWithTag(side+"clyde"));

		dotsRemaining = dotsList.Length;
		powerDotsRemaining = powerDotsList.Length;
		cherryObject = GameObject.FindGameObjectWithTag (side+"cherry");
		cherryObject.SetActive (false);

        cherryRespawnTimeRemaining = cherryRespawnTime;

        powerDotsRespawnTimeRemaining = powerDotRespawnTime;

        powerDotRespawns = false;
	}

	void FixedUpdate(){

		if (TurnManagerScript.paused) {
			return;
		}

		if (pacman.isAIControlled && TurnManagerScript.isPlayerTurn) {
			//do nothing if these are AI ghosts and it is the other players turn
			if(!screen.gameObject.activeSelf)
				screen.gameObject.SetActive(true);
			return;
		}
		
		if (!pacman.isAIControlled && !TurnManagerScript.isPlayerTurn) {
			//do nothing if these are ghosts for the real player, and it is the AI turn
			if(!screen.gameObject.activeSelf)
				screen.gameObject.SetActive(true);
			return;
		}

		//this maze is in turn, turn off the transparency
		if (screen.gameObject.activeSelf) {
			screen.gameObject.SetActive(false);
		}

		if (cherryRespawnTimeRemaining < 0) {
			respawnCherry ();
		}
		else if (cherryEaten ) {
			cherryRespawnTimeRemaining -= Time.deltaTime;
		}


        if (powerDotRespawns)
        {
            if (powerDotsRespawnTimeRemaining < 0)
            {
                restorePowerDots();
                powerDotsRespawnTimeRemaining = powerDotRespawnTime;
            }
            else
            {
                powerDotsRespawnTimeRemaining -= Time.deltaTime;
            }
        }
	}

	public bool validPacManMove(Vector2 position, PacmanMove.Direction dir){

		int x = (int)Math.Round (position.x, 0);
		int y = (int)Math.Round (position.y, 0);

		try{
			int value = 0;
			if(dir == PacmanMove.Direction.Up){
				value = map[y+1,x];
			}
			else if(dir == PacmanMove.Direction.Down){
				value = map[y-1,x];
			}
			else if(dir == PacmanMove.Direction.Right){
				value = map[y,x+1];
			}
			else if(dir == PacmanMove.Direction.Left){
				value = map[y,x-1];
			}
			else{
				//direction is None
				return true;
			}
			if(value == justDot || value == noDotNoCherry || value == powerDot || value == noPowerDot || value == dotAndCherry || value == justCherry){
				return true;
			}
			else{
				return false;
			}
		}
		catch(System.Exception e){
			Debug.Log ("Exception in validPacManMove: " + e.Message);
			return false;
		}

	}

	//checks all directions from the current position to see if moving there is possible
	//returns a list of 4 bools, representing a move up,right,down, and left (in that order)
	public List<bool> getAvailableDirections(Vector2 position){

		List<bool> directionsList = new List<bool>();
		if (validPacManMove (position, PacmanMove.Direction.Up))
			directionsList.Add (true);
		else
			directionsList.Add (false);

		if (validPacManMove (position, PacmanMove.Direction.Right))
			directionsList.Add (true);
		else
			directionsList.Add (false);

		if (validPacManMove (position, PacmanMove.Direction.Down))
			directionsList.Add (true);
		else
			directionsList.Add (false);

		if (validPacManMove (position, PacmanMove.Direction.Left))
			directionsList.Add (true);
		else
			directionsList.Add (false);

		return directionsList;

	}

	//returns the tile value at position
	int getValue(Vector2 position){
		int x = (int)Math.Round (position.x, 0);
		int y = (int)Math.Round (position.y, 0);
		return map [y, x];
	}

	void setValue(Vector2 position, int value){
		//might want to make sure nothing but dots are being changed
		int x = (int)Math.Round (position.x, 0);
		int y = (int)Math.Round (position.y, 0);
		map [y, x] = value;
	}

	public bool isInGhostPen(Vector2 position){
		int value = getValue (position);
		if (value == 3)
			return true;
		return false;
	}

	public bool isInDotTile(Vector2 position){
		int value = getValue (position);
		if (value == 2)
			return true;
		return false;
	}

	public bool isInPowerDotTile(Vector2 position){
		int value = getValue (position);
		if (value == 5)
			return true;
		return false;
	}

	public bool isInCherryTile(Vector2 position){
		int value = getValue (position);
		if (value == 7 || value == 8) {
			return true;
		}
		return false;
	}

    public bool isInGoodOccupiedTile(Vector2 position)
    {
        return (isInCherryTile(position) || isInDotTile(position) || isInPowerDotTile(position));
    }

	//also handle cherry tiles
	public void eatDot(Vector2 position){
		int value = getValue (position);
		//just a cherry
		if (value == justCherry) {
			setValue(position,noDotNoCherry);
			pacman.player1Score+=cherryValue;
			cherryEaten = true;
			cherryRespawnTimeRemaining = cherryRespawnTime;
			pacman.cherriesEaten++;
			cherryObject.SetActive(false);
		}
		if (value == dotAndCherry) {
			setValue(position,noDotNoCherry);
			pacman.player1Score += (cherryValue + dotPointValue);
			cherryEaten = true;
			cherryRespawnTimeRemaining = cherryRespawnTime;
			cherryObject.SetActive(false);
			Position p = new Position((int)position.x, (int)position.y);
			dots[p].SetActive(false);
			dotsRemaining--;
			pacman.dotsEaten++;
			pacman.cherriesEaten++;
		}
		if(isInDotTile(position)){
			setValue(position,noDotNoCherry);
			//set the game object to not active
			Position p = new Position((int)position.x, (int)position.y);
			dots[p].SetActive(false);
			pacman.player1Score += dotPointValue;
			dotsRemaining--;
			pacman.dotsEaten++;
		}
		else if(isInPowerDotTile(position)){
			setValue(position,6);
			Position p = new Position((int)position.x, (int)position.y);
			powerDots[p].SetActive(false);
			pacman.player1Score += powerDotPointValue;
			powerDotsRemaining--;
			pacman.powerMode = true;
			pacman.powerModeTimeRemaining = pacman.powerModeDuration;
			pacman.powerDotsEaten++;
            //change ghosts
            foreach(var ghost in ghosts)
            {
                ghost.GetComponent<Animator>().enabled = false;
                ghost.GetComponent<SpriteRenderer>().sprite = ghostScared;
                ghost.GetComponent<GhostMove>().isScared = true;
            }

		}

		if (dotsRemaining == 0 && powerDotsRemaining == 0) {
			restoreDots();
		}
	}

	public void restoreDots(){
		pacman.timesClearedMaze++;
		for (int i = 0; i < dotsList.Length; i++) {
			int x = (int)dotsList[i].transform.localPosition.x;
			int y = (int)dotsList[i].transform.localPosition.y;
			Position p = new Position(x,y);
			Vector2 v = new Vector2(x,y);

			setValue(v,2);
			dots[p].SetActive(true);
		}

        restorePowerDots();

		dotsRemaining = dotsList.Length;
		powerDotsRemaining = powerDotsList.Length;
		pacman.dotList = new List<GameObject> (pacman.dots);
		pacman.dotList.AddRange (pacman.powerDot);

	}

    public void restorePowerDots()
    {
        for (int i = 0; i < powerDotsList.Length; i++)
        {
            int x = (int)powerDotsList[i].transform.localPosition.x;
            int y = (int)powerDotsList[i].transform.localPosition.y;
            Position p = new Position(x, y);
            Vector2 v = new Vector2(x, y);
            setValue(v, 5);
            powerDots[p].SetActive(true);
        }
    }

	void respawnCherry(){
		int value = getValue (cherryLocation);
		if (value == noDotNoCherry) {
			setValue(cherryLocation,justCherry);
		}
		if(value == justDot){
			setValue (cherryLocation,dotAndCherry);
		}
		cherryEaten = false;
		cherryObject.SetActive (true);
	}
}
