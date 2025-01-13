using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int m_CurrentScore = 0;

    // Declare the delegate and event
    public delegate void ScoreUpdateHandler(int newScore);
    public static event ScoreUpdateHandler OnScoreUpdated;

    public int currentScore
    {
        get
        {
            return m_CurrentScore;
        }
        set
        {
            m_CurrentScore = value;
            
            // Invoke the event when score changes
            if (OnScoreUpdated != null) OnScoreUpdated.Invoke(m_CurrentScore);
        }
    }

    private void Awake()
    {
        Instance = this; // Keep this instance alive across scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
