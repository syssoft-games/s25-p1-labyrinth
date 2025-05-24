using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Szene neu laden
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}