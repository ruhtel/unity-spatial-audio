using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class AudioRayBasic {


    public Vector3 dir;
    private Vector3 lastDir;

    public RaycastHit hit;

    public Vector3 src;
    private Vector3 lastSrc;

    public Vector3 reflectVec;

    private bool didHit;
    private bool update;

    // Start is called before the first frame update
    public AudioRayBasic(Vector3 src, Vector3 dir) {
        this.src = src;
        this.dir = dir;
        this.didHit = false;
        this.update = true;
    }

    void Update() {
        if(src != lastSrc) {
            lastSrc = src;
            update = true;
        }

        if(dir != lastDir) {
            lastDir = dir;
            update = true;
        }
    }

    public void updateCast(Transform transform) {
        didHit = Physics.Raycast(src, transform.TransformVector(dir), out hit, Mathf.Infinity);
        if(didHit) reflectVec = getReflectionVector(hit, src);
    }

    public void draw(Transform transform) {
        if(didHit) {
            reflectVec = getReflectionVector(hit, src); 
            Debug.DrawRay(src, transform.TransformVector(dir) * hit.distance, Color.yellow);
        } else {
            reflectVec = Vector3.zero;
            Debug.DrawRay(src, transform.TransformVector(dir) * 2000, Color.red);
        }
    }

    // Given a RaycastHit and Incident vector, determine reflection
    Vector3 getReflectionVector(RaycastHit hit, Vector3 start) {
        Vector3 incident = hit.point - start;
        return incident - 2*(Vector3.Dot(incident, hit.normal)) * hit.normal; 
    }
}
