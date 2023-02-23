using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject gameSuccessCanvas;
    [SerializeField] GameObject gameOverCanvas;
 
    public void GameSuccess()
    {
        Debug.Log("You won the game!");
        gameSuccessCanvas.SetActive(true);
    }

    public void GameOver()
    {
        Debug.Log("You failed!");
        gameOverCanvas.SetActive(true);

    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }


}
