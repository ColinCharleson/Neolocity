using UnityEngine;

public class MapController : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 drag;
    public Transform player;

    public float minX; 
    public float maxX; 
    public float minZ;
    public float maxZ;

    void Update()
    {
        if (Time.timeScale == 0)// if scene is paused do this
        {
            if (Input.GetMouseButtonDown(0))
            {
                drag = Input.mousePosition;
                return;
            }

            if (Input.GetMouseButtonDown(1)) // check for right-click
            {
                Vector3 newPosition = player.position;
                newPosition.y = transform.position.y;
                transform.position = newPosition;
            }

            if (!Input.GetMouseButton(0)) return;

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - drag);
            Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

            Vector3 newPos = transform.position + move;

            newPos.x = Mathf.Clamp(newPos.x, minX, maxX); // clamp the new x position
            newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ); // clamp the new z position

            transform.position = newPos;
        }

        else// if scene is not paused set camera position back to player
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
    }
}