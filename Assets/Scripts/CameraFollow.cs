using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // reference to the player's Transform component
    public float xOffset = 0f; // horizontal offset from the player's position
    public float yOffset = 0f; // vertical offset from the player's position
    public float minX = -10f; // minimum x-coordinate for camera position
    public float maxX = 10f; // maximum x-coordinate for camera position
    public float minY = -10f; // minimum y-coordinate for camera position
    public float maxY = 10f; // maximum y-coordinate for camera position
    public float speed = 4f; // speed of movement

    void Update()
    {
        // calculate the camera's target position
        Vector3 targetPos = new Vector3(target.position.x + xOffset, target.position.y + yOffset, transform.position.z);

        // clamp the target position to the set boundaries
        float cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);
        targetPos.x = Mathf.Clamp(targetPos.x, minX + cameraHalfWidth, maxX - cameraHalfWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, minY + GetComponent<Camera>().orthographicSize, maxY - GetComponent<Camera>().orthographicSize);

        // smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
