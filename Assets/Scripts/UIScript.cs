using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIScript: MonoBehaviour {
 private GameScript game;
 [SerializeField]
 private GameObject SetupMenu;
 [SerializeField]
  private Text difficultyText;
  [SerializeField]
  private InputField difficultyInput;
 private int difficultyLevel = 1;
 // Use this for initialization
 void Start() {
  game = GetComponent < GameScript>();
  difficultyInput.onEndEdit.AddListener(delegate { updateDifficulty(difficultyInput); });
 }
 // Update is called once per frame
 void Update() {

 }
 public void updateDifficulty(InputField userInput)
{
    difficultyText.text = userInput.text;
    difficultyLevel = int.Parse(userInput.text);
}
 public void beginGame() {
  SetupMenu.SetActive(false);
  game.startGame(difficultyLevel);
 }
}
