using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform

    void Update()
    {
        if (player != null)
        {
            // Get the player's position
            Vector3 targetPosition = player.position;

            // Set the image's position to the player's position
            transform.position = targetPosition;
        }
    }
}
