using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

public class MazeScript : MonoBehaviour {

	int dotPointValue = 1;
	int powerDotPointValue = 20;

	public  int dotsRemaining;
	public  int powerDotsRemaining;

    public Sprite ghostScared;

	//0 = null space, used to pad the left and bottom sides of the maze
	//1 = wall
	//2 = blank space containing a dot
	//3 = 'ghost zone', is inaccessible to pacman
	//4 = blank space with no dot present
	//5 = blank space containing a power dot
	//6 = blank space with no power dot present

	//the origin of the map is the bottom left
	//map[2][2] is the bottom left most dot, and is at position x = 2, y = 2
	//with map[x,y]...
	//	Direction up = x+1,y
	//	direction down = x-1,y
	//	direction right = x,y+1
	//	direction left = x,y-1
	static int[,] map = new int[,]{
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
	
	// Use this for initialization
	void Start () {
		//initialize the associate arrays
		dotsList = GameObject.FindGameObjectsWithTag("dot");
		powerDotsList = GameObject.FindGameObjectsWithTag("powerDot");
		for(int i = 0; i < dotsList.GetLength(0); i++){
			Position p = new Position((int)dotsList[i].transform.position.x, (int)dotsList[i].transform.position.y);
			dots[p] = dotsList[i];
		}

		for (int i = 0; i < powerDotsList.GetLength(0); i++) {
			Position p = new Position((int)powerDotsList[i].transform.position.x, (int)powerDotsList[i].transform.position.y);
			powerDots[p] = powerDotsList[i];
		}

        ghosts.Add(GameObject.FindGameObjectWithTag("blinky"));
        ghosts.Add(GameObject.FindGameObjectWithTag("pinky"));
        ghosts.Add(GameObject.FindGameObjectWithTag("inky"));
        ghosts.Add(GameObject.FindGameObjectWithTag("clyde"));

		dotsRemaining = dotsList.Length;
		powerDotsRemaining = powerDotsList.Length;
	}

	public  bool validPacManMove(Vector2 position, PacmanMove.Direction dir){

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
			if(value == 2 || value == 4 || value == 5 || value == 6){
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

	static void setValue(Vector2 position, int value){
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

	public void eatDot(Vector2 position){
		if(isInDotTile(position)){
			setValue(position,4);
			//set the game object to not active
			Position p = new Position((int)position.x, (int)position.y);
			dots[p].SetActive(false);
			PacmanMove.player1Score+= dotPointValue;
			dotsRemaining--;
		}
		else if(isInPowerDotTile(position)){
			setValue(position,6);
			Position p = new Position((int)position.x, (int)position.y);
			powerDots[p].SetActive(false);
			PacmanMove.player1Score += powerDotPointValue;
			powerDotsRemaining--;
			PacmanMove.powerMode = true;
			PacmanMove.powerModeTimeRemaining = PacmanMove.powerModeDuration;

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

		for (int i = 0; i < dotsList.Length; i++) {
			int x = (int)dotsList[i].transform.position.x;
			int y = (int)dotsList[i].transform.position.y;
			Position p = new Position(x,y);
			Vector2 v = new Vector2(x,y);

			setValue(v,2);
			dots[p].SetActive(true);
		}

		for (int i = 0; i < powerDotsList.Length; i++) {
			int x = (int)powerDotsList[i].transform.position.x;
			int y = (int)powerDotsList[i].transform.position.y;
			Position p = new Position(x,y);
			Vector2 v = new Vector2(x,y);
			setValue(v,5);
			powerDots[p].SetActive(true);
		}

		dotsRemaining = dotsList.Length;
		powerDotsRemaining = powerDotsList.Length;

	}
}
