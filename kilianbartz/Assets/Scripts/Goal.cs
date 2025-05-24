using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static float timeTaken;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Win(timeTaken);

            // Optional: Disable or destroy the goal after collision
            // gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            timeTaken += Time.deltaTime;
        }
    }
}