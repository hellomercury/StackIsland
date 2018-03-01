using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_Text.text = "Score: " + m_Score.ToString();
    }

    public void SetScore(int score)
    {
        m_Score += score;
    }

    public Text m_Text;
    private int m_Score = 0;
}
