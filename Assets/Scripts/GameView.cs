using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameView : MonoBehaviour
{

    private Link controller;



    public TextMeshProUGUI rupiasGreenText, rupiasBlueText, rupiasRedText, scoreText, maxScoreText;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Link").GetComponent<Link>();
    }

    // Update is called once per frame
    void Update()
    {
      if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            int rupiaGreen = GameManager.sharedInstance.collectedObjectGreen;
            int rupiaBlue = GameManager.sharedInstance.collectedObjectBlue;
            int rupiaRed = GameManager.sharedInstance.collectedObjectRed;
            float score = controller.GetTravelledDistance();
            float maxScore = PlayerPrefs.GetFloat("maxscore",0);
            
            rupiasGreenText.text = rupiaGreen.ToString();
            rupiasBlueText.text = rupiaBlue.ToString();
            rupiasRedText.text = rupiaRed.ToString();
            scoreText.text = "Score:" + score.ToString("f1");
            maxScoreText.text = "MaxScore: " + maxScore.ToString("f1");
        }  
    }
}
