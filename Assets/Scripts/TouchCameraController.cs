using UnityEngine;

public class TouchCameraController : MonoBehaviour
{
    public float dragSpeed = 2.0f;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed, 0, -pos.y * dragSpeed); // Inverse direction
            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition; // Update drag origin

            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
            clampedPosition.z = Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y);
            transform.position = clampedPosition;
        }
    }
}