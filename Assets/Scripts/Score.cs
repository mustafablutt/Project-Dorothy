using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreText;
    public float highestPoint;
    public int lateScore;
    void Start()
    {
        highestPoint = 0;
    }

    void Update()
    {
        if (player.position.y > player.position.y - 1 && player.position.y > 0)
        {
            highestPoint = player.position.y * 10;
            if (highestPoint < 0)
            {
                highestPoint *= -1;
            }
            if (lateScore < (int)highestPoint)
            {
                finalScore((int)highestPoint);
                scoreText.text = "Score: " + highestPoint.ToString("0");
            }
        }
    }
    public int finalScore(int score)
    {
        lateScore = score;
        return lateScore;
    }
}