using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform target; // reference to the player's Transform component
    public float xOffset = 0f; // horizontal offset from the player's position
    public float minX = -10f; // minimum x-coordinate for camera position
    public float maxX = 10f; // maximum x-coordinate for camera position
    public float speed = 4f; // speed of movement

    void Update()
    {
        // calculate the camera's target position
        Vector3 targetPos = new Vector3(target.position.x + xOffset, transform.position.z);

        // smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
