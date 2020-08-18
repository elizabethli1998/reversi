using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript: MonoBehaviour {
  
 [SerializeField]
 private PColor[, ] board;
 [SerializeField]
 private GameObject[, ] boardObjects;
 [SerializeField]
 private GameObject bPiece;
 [SerializeField]
 private GameObject wPiece;
 [SerializeField]
 private GameObject switcherooPiece;
public int opponentScore = 0;
public int playerScore = 0;
public bool playerScored = false;
public bool madeAMove = false;
 const int BOARD_SIZE = 7;
 public enum gameState {
	InUI,
	InGame,
	Done
 };
 private enum turn {
  player,
  AI
 };
 private turn thisTurn;
public gameState thisState;
 private MiniMax AI;
 private int AIDifficulty;
 [SerializeField]
 private Text PlayingText;
 [SerializeField]
 private GameObject playingScreen;
 [SerializeField]
 private GameObject doneMenu;
 // Use this for initialization
 //Start off with an empty board
 void Start() {
  board = new PColor[, ] {
   {
    PColor.None,
		PColor.None,
		PColor.None,
		PColor.None,
		PColor.None,
		PColor.None,
		PColor.None,
		PColor.None
   },
	 {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None
   },
	 {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None
   }, {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.W,
    PColor.B,
    PColor.None,
    PColor.None,
    PColor.None
   }, {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.B,
    PColor.W,
    PColor.None,
    PColor.None,
    PColor.None
   }, {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None
   }, {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None
   }, {
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None,
    PColor.None
   }
  };
  boardObjects = new GameObject[, ] {
   {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }, {
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
   }
  };
 for (int j = 0; j <= BOARD_SIZE; j++) { for (int i = 0; i <= BOARD_SIZE; i++) {
	 if (board[i, j] == PColor.B) { boardObjects[i, j] =
	 (GameObject) Instantiate(bPiece, new Vector3(i, 1, j), Quaternion.identity); playerScore++; }
	 else if (board[i, j] == PColor.W) { boardObjects[i, j] = (GameObject) Instantiate(wPiece, new Vector3(i, 1, j), Quaternion.identity); opponentScore++; } } }
  playingScreen.SetActive(false);
  doneMenu.SetActive(false);
  thisTurn = turn.player;
  thisState = gameState.InUI;
  AI = GetComponent < MiniMax > ();
 }
 // Update is called once per frame
 void Update() {
  if ( gameState.InGame == thisState) {
   if (turn.player == thisTurn && GameObject.FindGameObjectsWithTag("Switcheroo").Length == 0) { isGameOver(); }
   if (thisTurn == turn.player && Input.GetMouseButtonDown(0) ) { GameObject[] allSwitcheroo = GameObject.FindGameObjectsWithTag("Switcheroo"); foreach(GameObject gm in allSwitcheroo) { Destroy(gm); }
    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); getTurn(pos.x, pos.z);}
   if (Input.GetKeyDown(KeyCode.M)) { enemyTurn();}
  }
 }
 //Getting player input
 void getTurn(float x, float y) {
  int updateY = Mathf.RoundToInt(y);
  int updateX = Mathf.RoundToInt(x);
  turningDirectionsForPieces directionPiece = isValidMove(updateX, updateY, PColor.B, board);
 if (directionPiece.down || directionPiece.left || directionPiece.up || directionPiece.right || directionPiece.downLeft || directionPiece.upLeft || directionPiece.downRight || directionPiece.upRight) { thisTurn = turn.AI;
	 Vector3 spawnPos = new Vector3(updateX, 1, updateY); boardObjects[updateX, updateY] = (GameObject) Instantiate(bPiece, spawnPos, Quaternion.identity); board[updateX, updateY] = PColor.B; playerScore++;
   doFlipThePieces(updateX, updateY, PColor.B, directionPiece);
   PlayingText.text = "Turn: White";
   StartCoroutine("enemyMove");
  }
 }
 IEnumerator enemyMove() { yield return new WaitForSeconds(2); enemyTurn(); }
 void enemyTurn()
 {
  move makeAMove;
  if (AIDifficulty % 2 != 0) { makeAMove = AI.SecondMiniMax(board, Mathf.CeilToInt(AIDifficulty / 2.0F), true, -99999, 99999, 0, 0);}
	else { makeAMove = AI.AlgoMiniMax(board, Mathf.CeilToInt(AIDifficulty / 2.0F), true, -99999, 99999, 0, 0);}
  if (makeAMove.j != -1 && makeAMove.i != -1)
	{
   print(makeAMove.value);
   turningDirectionsForPieces directionPiece = isValidMove(makeAMove.i, makeAMove.j, PColor.W, board);
   boardObjects[makeAMove.i, makeAMove.j] = (GameObject) Instantiate(wPiece, new Vector3(makeAMove.i, 1, makeAMove.j), Quaternion.identity);
   opponentScore++;
   board[makeAMove.i, makeAMove.j] = PColor.W;
   doFlipThePieces(makeAMove.i, makeAMove.j, PColor.W, directionPiece);
   PlayingText.text = "Turn: Black";
   thisTurn = turn.player;
   if (isGameOver()) { print("Can't move. Rip"); endGame(); }
  }
  else { print(makeAMove.value + ", " + makeAMove.i + ", " + makeAMove.j); print("GAME OVER!"); endGame(); }
 }

 //Checks for a valid move for the turn of whoever is up
 public turningDirectionsForPieces isValidMove(int x, int y, PColor thisColor, PColor[, ] board) {
  turningDirectionsForPieces pieces;
 pieces.left = false; pieces.right = false; pieces.up = false; pieces.down = false; pieces.downRight = false; pieces.upRight = false; pieces.points = 0; pieces.downLeft = false; pieces.upLeft = false;
  PColor enemyColor = PColor.None;
  int toComputePoints = 0;
  if (enemyColor == PColor.None) print("ERROR");
  if (thisColor == PColor.W) enemyColor = PColor.B;
  if (thisColor == PColor.B) enemyColor = PColor.W;
  //Piece is black
  if (x >= 0 && x <= BOARD_SIZE && y >= 0 && y <= BOARD_SIZE && board[x, y] == PColor.None) {
   if (x >= 1) {
    if (board[x - 1, y] == enemyColor) {
     for (int i = x - 1; i >= 0; i--) {
      toComputePoints++;
      if (board[i, y] == PColor.None) break;
      if (board[i, y] == thisColor) {
       pieces.up = true;
       toComputePoints = 0;
       pieces.points += toComputePoints;
       break;
      }
     }
    }
   }
   //Change from white piece to black. These next... Repetitive pieces of code just depend on the direction of FlipThePieceping
   if (x <= BOARD_SIZE - 1) {
    if (board[x + 1, y] == enemyColor) {
     for (int i = x + 1; i <= BOARD_SIZE; i++) {
      toComputePoints++;
      if (board[i, y] == PColor.None) break;
      if (board[i, y] == thisColor) {
       toComputePoints = 0;
       pieces.down = true;
       pieces.points += toComputePoints;
       break;
      }
     }
    }
   }
   if (y >= 1) {
    if (board[x, y - 1] == enemyColor) {
     for (int i = y - 1; i >= 0; i--) {
      toComputePoints++;
      if (board[x, i] == PColor.None) break;
      if (board[x, i] == thisColor) {
       toComputePoints = 0;
       pieces.left = true;
       pieces.points += toComputePoints;
       break;
      }
     }
    }
   }
   if (y <= BOARD_SIZE - 1) {
    if (board[x, y + 1] == enemyColor) {
     for (int i = y + 1; i <= BOARD_SIZE; i++) {
      toComputePoints++;
      if (board[x, i] == PColor.None) break;
      if (board[x, i] == thisColor) {
       toComputePoints = 0;
       pieces.right = true;
       pieces.points += toComputePoints;
       break;
      }
     }
    }
   }
   if (y >= 1 && x >= 1) {
    if (board[x - 1, y - 1] == enemyColor) {
		 int j = y - 1;
     int i = x - 1;
     while (i >= 0 && j >= 0) {
      toComputePoints++;
      if (board[i, j] == PColor.None) break;
      if (board[i, j] == thisColor) {
       pieces.upLeft = true;
       pieces.points += toComputePoints;
       toComputePoints = 0;
       break;
      }
      j--; i--;
     }
    }
   }
   if (y >= 1 && x <= BOARD_SIZE - 1) {
    if (board[x + 1, y - 1] == enemyColor) {
         int j = y - 1; int i = x + 1;
     while (i != BOARD_SIZE && j >= 0) {
      toComputePoints++;
      if (board[i, j] == PColor.None) break;
      if (board[i, j] == thisColor) {
       pieces.downLeft = true;
       pieces.points += toComputePoints;
       toComputePoints = 0;
       break;
      }
      i++; j--;
     }
    }
   }
   if (y <= BOARD_SIZE - 1 && x >= 1) {
    if (board[x - 1, y + 1] == enemyColor) {
     int j = y + 1; int i = x - 1;
     while (i >= 0 && j != BOARD_SIZE) {
      toComputePoints++;
      if (board[i, j] == PColor.None) break;
      if (board[i, j] == thisColor) {
       pieces.upRight = true;
       pieces.points += toComputePoints;
       toComputePoints = 0;
       break;
      }
      j++; i--;
     }
    }
   }
   if (y <= BOARD_SIZE - 1 && x <= BOARD_SIZE - 1) {
    if (board[x + 1, y + 1] == enemyColor) {
     int j = y + 1; int i = x + 1;
     while (i != BOARD_SIZE && j != BOARD_SIZE) {
      toComputePoints++;
      if (board[i, j] == PColor.None) break;
      if (board[i, j] == thisColor) {
       pieces.downRight = true;
       pieces.points += toComputePoints;
       toComputePoints = 0;
       break;
      }
      i++;j++;
     }
    }
   }
  }
  return pieces;
 }
 void doFlipThePieces(int x, int y, PColor thisColor, turningDirectionsForPieces pieces) {
  PColor enemyColor = PColor.None;
  if (enemyColor == PColor.None)
  if (thisColor == PColor.W) enemyColor = PColor.B;
  if (thisColor == PColor.B) enemyColor = PColor.W;
	if (pieces.left) {
	 for (int i = y - 1; i > 0; i--) {
		if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
		else board[x, i] = boardObjects[x, i].GetComponentInChildren < PieceScript > ().FlipThePiece();
	 }
	}
	if (pieces.right) {
	 for (int i = y + 1; i <= BOARD_SIZE; i++) {
			if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
		else board[x, i] = boardObjects[x, i].GetComponentInChildren < PieceScript > ().FlipThePiece();
	 }
	}
  if (pieces.up) {
   for (int i = x - 1; i >= 0; i--) {
  	if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
    else board[i, y] = boardObjects[i, y].GetComponentInChildren < PieceScript > ().FlipThePiece();
   }
  }
  if (pieces.down) {
   for (int i = x + 1; i <= BOARD_SIZE; i++) {
    	if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
    else board[i, y] = boardObjects[i, y].GetComponentInChildren < PieceScript > ().FlipThePiece();
   }
  }
  if (pieces.upLeft) {
	 int j = y - 1;int i = x - 1;
   while (i >= 0 && j >= 0) {
	 if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
    else board[i, j] = boardObjects[i, j].GetComponentInChildren < PieceScript > ().FlipThePiece();
    i--;j--;
   }
  }
  if (pieces.upRight) {
	 int j = y + 1;int i = x - 1;
   while (i >= 0 && j != BOARD_SIZE) {
    	if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
    else board[i, j] = boardObjects[i, j].GetComponentInChildren < PieceScript > ().FlipThePiece();
    i--;j++;
   }
  }
  if (pieces.downRight) {
	 int j = y + 1; int i = x + 1;
   while (i != BOARD_SIZE && j != BOARD_SIZE) {
    	if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
    else board[i, j] = boardObjects[i, j].GetComponentInChildren < PieceScript > ().FlipThePiece();
    i++; j++;
   }
  }
  if (pieces.downLeft) {
	 int j = y - 1; int i = x + 1;
   while (i != BOARD_SIZE && j >= 0) {
  	if (board[x, i] == PColor.None ||  board[x, i] == thisColor) break;
    else board[i, j] = boardObjects[i, j].GetComponentInChildren < PieceScript > ().FlipThePiece();
    i++;j--;
   }
  }
 }
 bool isGameOver() {
	bool moveFound = true;
	for (int j = 0; j <= BOARD_SIZE; j++) {
	 for (int i = 0; i <= BOARD_SIZE; i++) {
		turningDirectionsForPieces directionPiece = isValidMove(i, j, PColor.B, board);
		if (directionPiece.down || directionPiece.left || directionPiece.up || directionPiece.right || directionPiece.downLeft || directionPiece.upLeft || directionPiece.downRight || directionPiece.upRight) {
		 moveFound = false;
		 Instantiate(switcherooPiece, new Vector3(i, 1, j), Quaternion.identity);
		}
	 }
	}
	return moveFound;
 }
 public void startGame(int difficulty) {
  AIDifficulty = difficulty;
  thisState = gameState.InGame;
  playingScreen.SetActive(true);
  isGameOver();
  print("Difficulty level: " + difficulty);
 }
 public void upPlayer() { opponentScore--; playerScore++; playerScored = true;}
 public void upOpponent() { opponentScore++; playerScore--; playerScored = false;}
 public void quitGame() { Application.Quit(); }
 void endGame() {
  thisState = gameState.Done;
  playingScreen.SetActive(false);
  doneMenu.SetActive(true);
 }
    void OnGUI()
    {
        if (thisState == gameState.InGame)
        {
            GUI.Label(new Rect(20, 100, 400, 100), "Current Score: Black: " + playerScore + "  White: " + opponentScore);
            GUI.Label(new Rect(20, 60, 200, 100), "AI Difficulty: " + AIDifficulty);
        }
        if(thisState == gameState.Done)
        {
          if (playerScore < opponentScore)
          {
            GUI.Label(new Rect(20, 100, 400, 100),"You Lose!!! RIP!!!! Git gud scrub");
          }
          else
          {
            GUI.Label(new Rect(20, 100, 400, 100),"YOU WIN!!!! GGWP!");
          }
        }
    }
 public void restartGame() {SceneManager.LoadScene(SceneManager.GetActiveScene().name);}

}
public struct turningDirectionsForPieces
{
    public bool left;
    public bool right;
    public bool up;
    public bool down;
    public bool upLeft;
    public bool upRight;
    public bool downLeft;
    public bool downRight;
    public int points;
}

