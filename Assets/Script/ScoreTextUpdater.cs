using UnityEngine;
using TMPro;
public class ScoreTextUpdater: MonoBehaviour
{
    private TMP_Text m_TextField;

    void Awake()
    {
        m_TextField = GetComponent<TMP_Text>();
    }

    void Start()
    {
        GameManager.OnScoreUpdated += OnScoreUpdated;
    }
    
    void OnDestroy()
    {
        // Don't forget to unsubscribe when the object is destroyed
        GameManager.OnScoreUpdated -= OnScoreUpdated;
    }
    
    void OnScoreUpdated(int newScore)
    {
        m_TextField.text = newScore.ToString();
    }
}
