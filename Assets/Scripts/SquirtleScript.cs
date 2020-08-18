using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquirtleScript: MonoBehaviour
{
  public AnimationClip SquirtleAnim;
  private Animator myAnimator;
  float waitTime;
  private GameScript game;
  public float time;
  void Start ()
  {
    myAnimator = GetComponent<Animator>();
    game = GameObject.Find("Game Camera").GetComponent < GameScript > ();

  }
void Update ()
{
    if(game.thisState == GameScript.gameState.InGame)
    {
            myAnimator.SetBool("isDancing", true);
    }
    else
    {
        myAnimator.SetBool("isDancing", false);
    }
    if(game.thisState == GameScript.gameState.Done)
    {
        myAnimator.SetTrigger("isSpinning");
    }
}

}
