﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MazeScript : MonoBehaviour {

	//0 = null space, used to pad the left and bottom sides of the maze
	//1 = wall
	//2 = blank space
	//3 = 'ghost zone', is inaccessible to pacman

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
		{0,1,2,2,2,1,1,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,1,1,2,2,2,1},
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
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,2,2,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,1,1,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1,2,1,1,1,1,2,1},
		{0,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
		{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
	};


	// Use this for initialization
	void Start () {

	}

	public static bool validPacManMove(Vector2 position, PacmanMove.Direction dir){

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
			if(value == 2){
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
	public static List<bool> getAvailableDirections(Vector2 position){

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
	public static int getValue(Vector2 position){
		int x = (int)Math.Round (position.x, 0);
		int y = (int)Math.Round (position.y, 0);
		return map [y, x];
	}



}
