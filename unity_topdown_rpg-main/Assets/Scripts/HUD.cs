using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    int score;
    const string ScorePrefix = "Score: ";

    // Start is called before the first frame update
    void Start()
    {
        int score = 0;
        scoreText.text = ScorePrefix + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoint(int points)
    {
        if (DataManager.instance.isLoadGame)
        {
            score = DataManager.instance.data.score;
            scoreText.text = ScorePrefix + score.ToString();
            Debug.Log("score" + score);
            DataManager.instance.isLoadGame = false;
        }
        else
        {
            score += points;
            scoreText.text = ScorePrefix + score.ToString();
            DataManager.instance.data.score = score;
        }
    }
}
