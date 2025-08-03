using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float zoomMin = 1;
    public float zoomMax = 10;
    public ParticleController cameraTarget;

    // Update is called once per frame
    void Update() {
        float speedMag = Math.Abs(cameraTarget.forwardVelocity);
        if (speedMag > zoomMin && speedMag < zoomMax) {
            if (gameObject.GetComponent<Camera>().orthographicSize < Math.Round(speedMag, 2) - 1)
                gameObject.GetComponent<Camera>().orthographicSize += 0.01f;
            else if (gameObject.GetComponent<Camera>().orthographicSize > Math.Round(speedMag, 2) + 1)
                gameObject.GetComponent<Camera>().orthographicSize -= 0.01f;
        }
    }

    void LateUpdate() {
        Vector3 pos = transform.position;
        pos.y = cameraTarget.transform.position.y;
        transform.position = pos;
    }
}
