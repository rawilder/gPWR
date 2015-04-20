using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class GhostMove : MonoBehaviour {
    public float speed { get; set; }
    float eatenDelayRemaining = 0.0f;

    public float eatenDelay { get; set; }

    Vector2 origin;
    Vector2 destTile = Vector2.zero;
    public Vector2 tilePosition;
    public PacmanMove.Direction moveDir = PacmanMove.Direction.None;
    public bool isScared;
    public bool isAttackMode = false;
    public float timeInAMode = 0.0f;
    int attackTimeLimit = 20;
    int scatterTimeLimit = 7;
    
    Vector2 dest;
    public GameObject m; //the maze gameobject
    private MazeScript maze;
    List<PacmanMove.Direction> queuedMovements;
    private PacmanMove.Direction queuedDir = PacmanMove.Direction.None;

    void Start(){
        speed = 8.0f;
        eatenDelay = 1.0f;
        origin = transform.localPosition;
        dest = origin;
        //maze = GameObject.FindGameObjectWithTag("maze").GetComponent<MazeScript>();
        queuedMovements = new List<PacmanMove.Direction>();
        maze = m.GetComponent<MazeScript> ();
        isScared = false;
        destTile = transform.localPosition;

    }

    void FixedUpdate() {

        if (TurnManagerScript.paused) {
            return;
        }

        if (maze.pacman.isAIControlled && TurnManagerScript.isPlayerTurn) {
            //do nothing if these are AI ghosts and it is the other players turn
            return;
        }

        if (!maze.pacman.isAIControlled && !TurnManagerScript.isPlayerTurn) {
            //do nothing if these are ghosts for the real player, and it is the AI turn
            return;
        }

        if (eatenDelayRemaining > 0) 
        {
            eatenDelayRemaining -= Time.deltaTime;
        } 
        else
        {
            timeInAMode += Time.deltaTime;
            if (timeInAMode > scatterTimeLimit && !isAttackMode) {
                foreach (var ghost in maze.pacman.ghostsMoves)
                {
                    ghost.isAttackMode = true;
                    ghost.timeInAMode = 0.0f;
                }
            }
            else if ((timeInAMode > attackTimeLimit && isAttackMode) || maze.pacman.pacmanEaten)
            {
                foreach (var ghost in maze.pacman.ghostsMoves)
                {
                    ghost.isAttackMode = false;
                    ghost.timeInAMode = 0.0f;
                }
            }
            if((Vector2)transform.localPosition == dest){
                //check if the ghost is inside the pen
                if (maze.isInGhostPen(transform.localPosition))
                {
                    //exit the pen
                    moveDir = PacmanMove.Direction.Up;
                    dest = new Vector2(14, 20);
                    destTile = new Vector2(14, 20);
                }
                else
                {   
                    /*-------------------------------------------------------*/
                    //this section is where the intelligent path planning is
                    if (isAttackMode && !isScared && DataScript.alloc.DumbGhosts != 1) //allow for dumb ai
                    {
                        if (queuedMovements.Count == 0)
                        {
                            queuedMovements = maze.AStarcalculations(maze.pacman.getPacmanAsGameObject(), transform.localPosition);
                        }
                        moveDir = queuedMovements.FirstOrDefault();
                        if (queuedMovements.Count != 0)
                        {
                            queuedMovements.RemoveAt(0);
                        }
                    }
                    else
                    {
                        queuedMovements.Clear();
                        //dumb ai: check all directions to see if there is a turn that could be made
                        List<bool> availableDirections = maze.getAvailableDirections(transform.localPosition);
                        for (int i = 0; i < 4; i++)
                        {
                            //if there is an available direction to travel
                            if (availableDirections[i])
                            {
                                //take this turn with a 25% probability

                                if (UnityEngine.Random.value <= .25)
                                {
                                    //dont allow him to reverse direction
                                    if (i == 0 && moveDir != PacmanMove.Direction.Down) moveDir = PacmanMove.Direction.Up;
                                    if (i == 1 && moveDir != PacmanMove.Direction.Left) moveDir = PacmanMove.Direction.Right;
                                    if (i == 2 && moveDir != PacmanMove.Direction.Up) moveDir = PacmanMove.Direction.Down;
                                    if (i == 3 && moveDir != PacmanMove.Direction.Right) moveDir = PacmanMove.Direction.Left;
                                }
                            }
                        }
                    }
                    
                    /*-------------------------------------------------------*/

                    if (moveDir == PacmanMove.Direction.Up && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Up))
                    {
                        dest = (Vector2)transform.localPosition + Vector2.up;
                        destTile.y++;
                    }
                    if (moveDir == PacmanMove.Direction.Right && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Right))
                    {
                        dest = (Vector2)transform.localPosition + Vector2.right;
                        destTile.x++;
                    }
                    if (moveDir == PacmanMove.Direction.Down && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Down))
                    {
                        dest = (Vector2) transform.localPosition - Vector2.up;
                        destTile.y--;
                    }
                    if (moveDir == PacmanMove.Direction.Left && maze.validPacManMove(transform.localPosition, PacmanMove.Direction.Left))
                    {
                        dest = (Vector2)transform.localPosition - Vector2.right;
                        destTile.x--;
                    }
                }
            }

            //set the movement direction for the sprite
            Vector2 dir = new Vector2(0,0);
            if (moveDir == PacmanMove.Direction.Up) {
                dir.y = 1;
            }
            if (moveDir == PacmanMove.Direction.Down) {
                dir.y = -1;
            }
            if (moveDir == PacmanMove.Direction.Right) {
                dir.x = 1;
            }
            if (moveDir == PacmanMove.Direction.Left) {
                dir.x = -1;
            }
            
            // Animation
            GetComponent<Animator> ().SetFloat ("DirX", dir.x);
            GetComponent<Animator> ().SetFloat ("DirY", dir.y);

            if (maze.validGhostMove(transform.localPosition, moveDir) || (Vector2)transform.localPosition != tilePosition)
            {
                Vector2 p = Vector2.MoveTowards(transform.localPosition, destTile, speed * Time.deltaTime);
                transform.localPosition = p;

                //round to the nearest tile
                tilePosition.x = (int)Math.Round(transform.localPosition.x, 0);
                tilePosition.y = (int)Math.Round(transform.localPosition.y, 0);

            }
            else
            {
                //not a valid move
                dest = transform.localPosition;
                destTile = tilePosition;
            }

            
        }
    }

    public void killGhost(){
        transform.localPosition = origin;
        tilePosition = origin;
        dest = origin;
        eatenDelayRemaining = eatenDelay;
        GetComponent<Animator>().enabled = true;
        isScared = false;
    }    
}
