using UnityEngine;

public class BasketGoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Balloon"))
        {
            // Get the balloon's rigidbody to check its velocity
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate dot product between velocity and up vector
                // If dot product is negative, the balloon is moving downward
                float dotProduct = Vector3.Dot(rb.velocity, Vector3.up);
                if (dotProduct < 0)
                {
                    GameManager.Instance.currentScore++;
                }
            }
        }
    }
}