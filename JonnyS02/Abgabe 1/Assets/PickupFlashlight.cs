using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    public GameObject flashlightObject;      

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Floor")
        {
            if (flashlightObject != null)
            {
                Destroy(flashlightObject);
                Debug.Log("Taschenlampe entfernt.");
            }

            GameObject player = GameObject.Find("Player(Clone)");

            if (player != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                
                if (playerMovement != null)
                {
                    playerMovement.PickupFlashlight();
                }
                else
                {
                    Debug.LogWarning("PlayerMovement-Skript nicht gefunden!");
                }
            }
            else
            {
                Debug.LogWarning("Kein Spieler mit dem Namen 'Player(Clone)' gefunden!");
            }
        }
    }
}
