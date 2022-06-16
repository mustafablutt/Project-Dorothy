using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;
    //public GameOverScreen gameOverScreen;
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = "YOUR SCORE IS " + score.ToString() + " POINTS";


    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {

        SceneManager.LoadScene("Menu");
    }
}
