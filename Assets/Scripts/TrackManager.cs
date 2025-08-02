using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Timeline;

public class TrackManager : MonoBehaviour {
    public static TrackManager instance;

    public float sceneMinX = -10;
    public float sceneMaxX = 10;
    public float particleVelocity = 2;
    public float trackLength = 50;
    public float trackOffset = 0;

    public List<TrackedObject> trackedObjects = new List<TrackedObject>();

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        // Update track offset
        trackOffset += particleVelocity * Time.fixedDeltaTime;
        trackOffset %= trackLength;
        Vector2 globalVelocity = Vector2.left * particleVelocity;

        foreach (TrackedObject trackedObject in trackedObjects) {
            if (trackedObject == null) {
                // If the tracked object is null, that means it was destroyed
                trackedObjects.Remove(trackedObject);
                continue;
            }

            float leftBoundary = (((trackOffset + sceneMinX) % trackLength) + trackLength) % trackLength;
            if (trackedObject.isOnScreen) {
                trackedObject.UpdateObject(globalVelocity);
                if (trackedObject.transform.position.x < sceneMinX) {
                    // Instead of deleting objects, we'll just freeze them when out of bounds and teleport them
                    // i mean I guess we could simulate offscreen movement, but I don't think anyone would notice
                    trackedObject.transform.position = new Vector2(sceneMaxX, trackedObject.transform.position.y);
                    trackedObject.Freeze(); // Mark it as off-screen
                    trackedObject.spawnPosition = leftBoundary;
                }
            } else {
                if (isInTrackWindow(trackedObject.spawnPosition)) {
                    trackedObject.Unfreeze();
                }
            }
        }
    }

    bool isInTrackWindow(float pos) {
        float leftBoundary = (((trackOffset + sceneMinX) % trackLength) + trackLength) % trackLength;
        float rightBoundary = (((trackOffset + sceneMaxX) % trackLength) + trackLength) % trackLength;
        if (leftBoundary < rightBoundary) {
            return pos >= leftBoundary && pos <= rightBoundary;
        } else {
            return pos >= leftBoundary || pos <= rightBoundary;
        }
    }
}
