using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver
}



public class GameManager : MonoBehaviour
{

    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance;

    private Link controller;

    public int collectedObjectGreen = 0;
    public int collectedObjectBlue = 0; 
    public int collectedObjectRed = 0;
    public int collectedObjectHealth = 0;
    public int collectedObjectMana = 0;


    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }

        
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Link").GetComponent<Link>();
        MenuManager.sharedInstance.HideGameOver();
        MenuManager.sharedInstance.HideScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit")&& currentGameState != GameState.inGame)
        {
            StartGames();
        } 
    }


    //Iniciar el juego
    public void StartGames()
    {
        SetGameState(GameState.inGame);
        
    }

    //Finalizar el juego
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
        

    }

    //Regresar al menu
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            //TODO: colocar la logica del menu
            MenuManager.sharedInstance.ShowMainMenu();
            
        }
        else if (newGameState == GameState.inGame)
        {
            //TODO: Preparar la escena para jugar
            MenuManager.sharedInstance.ShowMainMenu();
            LevelManager.sharedInstance.RemoveAllLevelBlock();
            LevelManager.sharedInstance.GenerateInitialBlock();
            controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameOver();
            MenuManager.sharedInstance.ShowScore();
            

        }
        else if (newGameState == GameState.gameOver)
        {
            //TODO: Preparar el juego para el Game Over
            MenuManager.sharedInstance.ShowGameOver();
            

        }

        this.currentGameState = newGameState;

    }

}
