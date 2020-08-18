using UnityEngine;
using System.Collections;

public enum PColor {
	None,
	W,
	B
}
public class PieceScript : MonoBehaviour {
	private enum PieceRotation {
		White = 0,
		Black = 180
	}
	[SerializeField]
	private PColor currentColor;
	private Animator anim;
	private GameScript game;
	// Use this for initialization
	void Start ()
  {
		anim = GetComponent<Animator>();
		game = GameObject.Find("Game Camera").GetComponent<GameScript>();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) FlipThePiece();
	}
	public void setColor(PColor color) {
		currentColor = color;
	}
  //FlipThePiece opposite
	public PColor FlipThePiece() {
		if (currentColor == PColor.W) {
			anim.SetTrigger("Wht2BlkFlipThePiece");
      	game.upPlayer();
			currentColor = PColor.B;
		}
		else {
			anim.SetTrigger("Blk2WhtFlipThePiece");
      			game.upOpponent();
			currentColor = PColor.W;
		}
		return currentColor;
	}
}
