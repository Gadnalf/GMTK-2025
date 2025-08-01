using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomMin = 1;
    public float zoomMax = 10;

    // Update is called once per frame
    void Update()
    {
        float speedMag = Math.Abs(gameObject.transform.parent.GetComponent<Rigidbody2D>().linearVelocityY);
        if (speedMag > zoomMin && speedMag < zoomMax)
        {
            if (gameObject.GetComponent<Camera>().orthographicSize < Math.Round(speedMag, 2) - 1)
                gameObject.GetComponent<Camera>().orthographicSize += 0.01f;
            else if (gameObject.GetComponent<Camera>().orthographicSize > Math.Round(speedMag, 2) + 1)
                gameObject.GetComponent<Camera>().orthographicSize -= 0.01f;
        }
    }
}
