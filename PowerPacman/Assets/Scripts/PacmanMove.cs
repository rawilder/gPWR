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
	int ghostDistance = 2;
	int cherryDistance = 7;

    //The ghosts
    public ClydeMove clyde;
    public InkyMove inky;
    public PinkyMove pinky;
    public GhostMove blinky;
    public List<GhostMove> ghostsMoves;
    public Sprite scared;

    //public bool isPlayerTurn = true;
    public static float turnDuration = DataScript.scenario.turnTime; //length of turn in seconds
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

    private Direction movementAwayDir;
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
    public bool GhostLowerSpeedPowerup { get; set; }
    public bool FewerGhostsPowerup { get; set; }
    public bool GhostsRespawnSlowerPowerup { get; set; }
    private bool PowerBallsRespawnPowerup { get; set; }
    private bool FasterFruitRespawnPowerup { get; set; }
    private bool oneTimePowerupStep;

    public int ghostsEaten = 0;
    public int dotsEaten = 0;
    public int timesClearedMaze = 0;
    public int timesEaten = 0;
    public int cherriesEaten = 0;
    public int powerDotsEaten = 0;

	public bool garrisonScoring = false;
	public bool garrisonScoreFudgeryPart2 = true;
	public float fudgeFactor; //a number within the hard limit, different each turn so that the user doesn't see the ai always exaclty matching their score
	public int targetScore;
	public int startingScore = 0;
	public float percentTimeLeftInTurn;
	public float percentScoreToTarget;
	public float hardPercentLimit = .05f;

	int tempScore = 0;		//for use with ai score fudging

	Text bonusBox;

	// Use this for initialization
	void Start () {
		garrisonScoring = false;
		garrisonScoreFudgeryPart2 = true;
		bonusBox = GameObject.Find (side+"BonusBox").GetComponent<Text> ();
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

        ghostsMoves = new List<GhostMove>();
        ghostsMoves.Add(clyde);
        ghostsMoves.Add(inky);
        ghostsMoves.Add(pinky);
        ghostsMoves.Add(blinky);

        //if (DataScript.alloc.PlayerSpeed == 0 && isAIControlled) {
        //    PacmanSpeedPowerup = true;
        //} else if (DataScript.alloc.PlayerSpeed == 1 && !isAIControlled) {
        //    PacmanSpeedPowerup = true;
        //} else {
        //    PacmanSpeedPowerup = false;
        //}

        PacmanSpeedPowerup = DeterminePowerup(DataScript.alloc.PlayerSpeed);
        PowerModeDurationPowerup = DeterminePowerup(DataScript.alloc.LongerPowerMode);
        GhostLowerSpeedPowerup = DeterminePowerup(DataScript.alloc.GhostSpeed);
        FewerGhostsPowerup = DeterminePowerup(DataScript.alloc.FewerGhosts);
        GhostsRespawnSlowerPowerup = DeterminePowerup(DataScript.alloc.GhostRespawn);
        PowerBallsRespawnPowerup = DeterminePowerup(DataScript.alloc.PowerBallRespawn);
        FasterFruitRespawnPowerup = DeterminePowerup(DataScript.alloc.FruitRespawn);
        oneTimePowerupStep = true;


        // powerups testing
        // SpeedPowerup = true;
        // PowerModeDurationPowerup = true;


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

		bonusBox.text = "Active bonuses\n";
		if (PacmanSpeedPowerup) {
			bonusBox.text += "Player speed increase. ";
		}
		if (PowerModeDurationPowerup) {
			bonusBox.text += "Longer super mode. ";
		}
		if (GhostLowerSpeedPowerup) {
			bonusBox.text += "Enemy speed decrease. ";
		}
		if (FewerGhostsPowerup) {
			bonusBox.text += "Fewer enemies. ";
		}
		if (GhostsRespawnSlowerPowerup) {
			bonusBox.text += "Enemy slower respawn. ";
		}
		if (PowerBallsRespawnPowerup) {
			bonusBox.text += "Super balls respawn. ";
		}
		if (FasterFruitRespawnPowerup) {
			bonusBox.text += "Fruit respawns. ";
		}
		if (DataScript.alloc.DumbGhosts == 1 && !isAIControlled) {
			bonusBox.text += "\"Dumb\" enemies. ";
		} else if (DataScript.alloc.DumbGhosts == 0 && isAIControlled) {
			bonusBox.text += "\"Dumb\" enemies. ";
		}

		bool availableBonuses = (DataScript.scenario.pSpeedIncreaseAvailable || DataScript.scenario.gSpeedDecreaseAvailable || DataScript.scenario.fRespawnAvailable || DataScript.scenario.powerballRespawnAvailable || DataScript.scenario.gRespawnAvailable || DataScript.scenario.gDumbAvailale || DataScript.scenario.gFewerAvailable);
		if (DataScript.scenario.control || !availableBonuses) {
			bonusBox.enabled = false;
		}

		fudgeFactor = UnityEngine.Random.Range(0.1f, hardPercentLimit);
		if(UnityEngine.Random.Range(0,10) < 5){
			fudgeFactor *= -1.0f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		targetScore = (int)((DataScript.playerScore+DataScript.scenario.scoreThreshold)*(1+fudgeFactor));
		percentTimeLeftInTurn = turnTimeRemaining / DataScript.scenario.turnTime;
		percentScoreToTarget = player1Score / (float)targetScore;

        if (oneTimePowerupStep)
        {
            if (GhostLowerSpeedPowerup)
            {
                foreach (var ghost in maze.ghosts)
                {
                    ghost.GetComponent<GhostMove>().speed = 7.5f;
                }
            }

            if (GhostsRespawnSlowerPowerup)
            {
                foreach (var ghost in maze.ghosts)
                {
                    ghost.GetComponent<GhostMove>().eatenDelay = 2.0f;
                }
            }

            if (FasterFruitRespawnPowerup)
            {
                maze.cherryRespawnTime = 5.0f;
            }

            if (PowerBallsRespawnPowerup)
            {
                maze.powerDotRespawns = true;
            }

            if (FewerGhostsPowerup)
            {
                int randomGhostPos = UnityEngine.Random.Range(0, maze.ghosts.Count);
                maze.ghosts.ElementAt(randomGhostPos).SetActive(false);
            }

            oneTimePowerupStep = false;
        }

        if (TurnManagerScript.paused) {
            return;
        }

        if ((isAIControlled && TurnManagerScript.isPlayerTurn) || (!isAIControlled && !TurnManagerScript.isPlayerTurn)) {
            //do nothing, wait your turn
            return;
        }

        gameObject.GetComponent<Animator>().enabled = true;

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
            targetGhost = null;
            queuedMovements.Clear();
            foreach(var ghost in ghostsMoves)
            {
                ghost.isAttackMode = false;
                ghost.timeInAMode = 0.0f;
            }
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
                    if(!ghostIsThere() || nearByGhostIsScared()) {
						if (maze.validPacManMove(transform.localPosition, movementAwayDir) && movementAwayDir != Direction.None)
						{
							makePreviousMove();
						} else {
                            moveTowardsFood();
						}
						movementAwayDir = Direction.None;
                        ghostDistance = 2;
                    }
                    else {
                        targetFood = null;
						if (maze.validPacManMove(transform.localPosition, movementAwayDir) && movementAwayDir != Direction.None)
						{
							makePreviousMove();
						} else {
                            movementAwayDir = Direction.None;
                            moveAwayFromGhost();
                        }
                        ghostDistance = 4;
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

        if (maze.validPacManMove (transform.localPosition, movementDir) || (Vector2)transform.localPosition != tilePosition 
		    || (maze.validPacManMove (transform.localPosition, movementAwayDir) && movementAwayDir != Direction.None)) {
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

            gameObject.GetComponent<Animator>().enabled = false;

            if(isAIControlled){
                DataScript.aiScore = player1Score;
                DataScript.aiDotsEaten = dotsEaten;
                DataScript.aiGhostsEaten = ghostsEaten;
                DataScript.aiCherriesEaten = cherriesEaten;
                DataScript.aiPowerDotsEaten = powerDotsEaten;
                DataScript.aiTimesClearedMaze = timesClearedMaze;
                DataScript.aiTimesEaten = timesEaten;
				startingScore = player1Score;
				tempScore = player1Score;
				fudgeFactor = UnityEngine.Random.Range(0.01f, hardPercentLimit);
				if(UnityEngine.Random.Range(0,10) < 5){
					fudgeFactor *= -1.0f;
				}
				Debug.Log("Fudge factor: " + fudgeFactor);
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

    double distance(Vector2 p1, Vector2 p2){
        return Math.Sqrt (Math.Pow((p2.x - p1.x),2) + Math.Pow((p2.y-p1.y),2));
    }

    void checkForGhostCollisions(){

        //compare player tile position to the position of each of the ghosts
        foreach(var ghost in maze.ghosts)
        {
            var ghostMove = ghost.GetComponent<GhostMove>();
            if (tilePosition == ghostMove.tilePosition || distance (transform.localPosition, ghost.GetComponent<GhostMove>().transform.localPosition) < .5)
            {
                if (powerMode && ghostMove.isScared)
                {
                    ghostMove.killGhost();
					addScore(MazeScript.ghostValue);
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

	void makePreviousMove()
	{
		if (movementAwayDir == Direction.Up && maze.validPacManMove(transform.localPosition, Direction.Up))
		{
			dest = (Vector2)transform.localPosition + Vector2.up;
			destTile.y++;
		}
		if (movementAwayDir == Direction.Right && maze.validPacManMove(transform.localPosition, Direction.Right))
		{
			dest = (Vector2)transform.localPosition + Vector2.right;
			destTile.x++;
		}
		if (movementAwayDir == Direction.Down && maze.validPacManMove(transform.localPosition, Direction.Down))
		{
			dest = (Vector2)transform.localPosition - Vector2.up;
			destTile.y--;
		}
		if (movementAwayDir == Direction.Left && maze.validPacManMove(transform.localPosition, Direction.Left))
		{
			dest = (Vector2)transform.localPosition - Vector2.right;
			destTile.x--;
		}
	}
	
    void moveTowardsFood()
    {
        if(maze.cherryObject.activeSelf && !dotList.Contains(cherry)) {
            cherry = new GameObject();
            cherry.tag = "extra";
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
        if (targetFood == null || queuedMovements.Count == 0) {
            if(closeToCherry() && dotList.Contains(cherry)) {
                targetFood = cherry;
            } else if (nearByGhostIsScared()) {
                targetGhost = new GameObject();
                targetGhost = getScaredGhost();
                if(targetGhost != null) {
                    Vector3 temp = Vector3.zero;
                    temp.x = Mathf.RoundToInt(targetGhost.transform.localPosition.x);
                    temp.y = Mathf.RoundToInt(targetGhost.transform.localPosition.y);
                    targetGhost.transform.localPosition = temp;
                    targetGhost.tag = "extra";
                    targetFood = targetGhost;
                    dotList.Add(targetFood);
                } else {
                    targetFood = findClosestFood();
                }
            } else {
                targetFood = findClosestFood();
            }
        }
        if (queuedMovements.Count == 0) {
            queuedMovements = maze.AStarcalculations(targetFood, transform.localPosition);
        }
        movementDir = queuedMovements.FirstOrDefault ();
        if (queuedMovements.Count != 0) {
            queuedMovements.RemoveAt (0);
        }
        if ((Vector2)transform.localPosition == dest)
        {
            if (movementDir != Direction.None)
            {
                if (movementDir == Direction.Up && maze.validPacManMove(transform.localPosition, Direction.Up))
                {
                    destTile.y++;
                    dest = (Vector2)transform.localPosition + Vector2.up;
                }
                if (movementDir == Direction.Right && maze.validPacManMove(transform.localPosition, Direction.Right))
                {
                    destTile.x++;
                    dest = (Vector2)transform.localPosition + Vector2.right;
                }
                if (movementDir == Direction.Down && maze.validPacManMove(transform.localPosition, Direction.Down))
                {
                    destTile.y--;
                    dest = (Vector2)transform.localPosition - Vector2.up;
                }
                if (movementDir == Direction.Left && maze.validPacManMove(transform.localPosition, Direction.Left))
                {
                    destTile.x--;
                    dest = (Vector2)transform.localPosition - Vector2.right;
                }
            }
        }
    }

    bool nearByGhostIsScared()
    {
        Vector2 pos = (Vector2)transform.localPosition;
        foreach(var ghost in maze.ghosts) {
			if (Vector2.Distance(pos, (Vector2)ghost.transform.localPosition) < ghostDistance && ghost.GetComponent<GhostMove> ().isScared)
                return true;
        }
        return false;
    }

    GameObject getScaredGhost()
    {
		return maze.ghosts.FirstOrDefault(g => Vector2.Distance (transform.localPosition, (Vector2)g.transform.localPosition) < ghostDistance 
                                   && g.GetComponent<GhostMove> ().isScared);
    }

    GameObject getClosestFood()
    {
        return dotList.Aggregate ((d1, d2) 
                                  => Vector2.Distance (transform.localPosition, (Vector2)d1.transform.localPosition) 
                                  < Vector2.Distance (transform.localPosition, (Vector2)d2.transform.localPosition) ? d1 : d2);
    }
    GameObject findClosestFood()
    {
        GameObject temp = getClosestFood ();
        //it is ghost not suppose to be here
        if (temp.tag == "extra" && temp != cherry) {
            dotList.Remove (temp);
            temp = getClosestFood();
        }
        return temp;
    }

    bool closeToCherry()
    {
        Vector2 pos = (Vector2)transform.localPosition;
        if (cherry == null)
            return false;
        if (Vector2.Distance (pos, (Vector2)cherry.transform.localPosition) < cherryDistance)
            return true;
        else
            return false;
    }

    bool ghostIsThere()
    {
        Vector2 pos = (Vector2)transform.localPosition;
        foreach(var ghost in maze.ghosts) {
			if (Vector2.Distance(pos, (Vector2)ghost.transform.localPosition) < ghostDistance)
                return true;
        }
        return false;
    }
    
    void moveAwayFromGhost()
    {
        var values = new float[4];
        float max = 0;
        //find closest ghost
		GameObject closestGhost = maze.ghosts.FirstOrDefault(g => Vector2.Distance (transform.localPosition, (Vector2)g.transform.localPosition) < ghostDistance);
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
            movementAwayDir = Direction.Up;
        } else if (max == values[1] && maze.validPacManMove (transform.localPosition, Direction.Down)) {
            destTile.y--;
            dest = (Vector2)transform.localPosition - Vector2.up;
            movementAwayDir = Direction.Down;
        } else if (max == values[2] && maze.validPacManMove (transform.localPosition, Direction.Right)) {
            destTile.x++;
            dest = (Vector2)transform.localPosition + Vector2.right;
            movementAwayDir = Direction.Right;
        } else if (max == values[3] && maze.validPacManMove (transform.localPosition, Direction.Left)) {
            destTile.x--;
            dest = (Vector2)transform.localPosition - Vector2.right;
            movementAwayDir = Direction.Left;
        }
    }

    bool DeterminePowerup(int allocValue)
    {
		if (allocValue == 2) {
			return true;
		}
        if (allocValue == -1)
        {
            return false;
        }
        else
        {
            return Convert.ToBoolean(allocValue) ^ isAIControlled;
        }
    }

    public GameObject getPacmanAsGameObject()
    {
        GameObject pacman = new GameObject();
        Vector3 temp = Vector3.zero;
        temp.x = Mathf.RoundToInt(transform.localPosition.x);
        temp.y = Mathf.RoundToInt(transform.localPosition.y);
        pacman.transform.localPosition = temp;
        return pacman;
    }

	public void addScore(int itemValue){
		if (garrisonScoring && isAIControlled) {
			if (player1Score > targetScore * (1 + hardPercentLimit)) {
				player1Score += 1;
			} else {
				if (percentScoreToTarget < percentTimeLeftInTurn) {
					//the score is lagging behind
					float temp = percentTimeLeftInTurn - percentScoreToTarget;
					if (player1Score + ((int)((1 + temp) * itemValue)) > targetScore) {
						if (player1Score > targetScore) {
							player1Score += UnityEngine.Random.Range (1, 3);
						} else {
							player1Score = targetScore;
						}
					} else {
						player1Score += (int)((1 + temp) * itemValue);
					}
				} else if (percentScoreToTarget > percentTimeLeftInTurn) {
					float temp = percentTimeLeftInTurn - percentScoreToTarget;
					if (player1Score + ((int)((1 - temp) * itemValue)) > targetScore) {
						if (player1Score >= targetScore) {
							player1Score += UnityEngine.Random.Range (1, 3);
						} else {
							player1Score = targetScore;
						}
					} else {
						player1Score += (int)((1 - temp) * itemValue);
					}
				}
			}
		} else if(garrisonScoreFudgeryPart2 && isAIControlled){
			if(turnTimeRemaining == DataScript.scenario.turnTime){
				Debug.Log("adding 1");
				player1Score+=1;
			}
			else{
				Debug.Log("Fudging");
				player1Score = tempScore + (int)(((DataScript.scenario.turnTime - turnTimeRemaining) * (targetScore-tempScore)) / DataScript.scenario.turnTime);
			}
		}
		else {
			player1Score += itemValue;
		}
	}
}
