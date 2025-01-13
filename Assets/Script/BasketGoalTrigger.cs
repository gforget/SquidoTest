using UnityEngine;

public class BasketGoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Balloon"))
        {
            GameManager.Instance.currentScore++;
        }
    }
}