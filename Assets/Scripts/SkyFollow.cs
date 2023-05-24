using UnityEngine;

public class SkyFollow : MonoBehaviour
{
    public Transform target; // reference to the player's Transform component

    void Update()
    {
        // move the image to the target position
        transform.position = new Vector2(target.position.x, target.position.y);
    }
}
